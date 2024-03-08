using MailAPI.Data.Models;
using MailAPI.Data;
using MailAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Net.Mail;
using System.Net;

namespace MailAPI.Services
{
    public class UserService : IUserService
    {
        private readonly DbContextOptions<DataContext> dbContextOptions;

        public UserService(DbContextOptions<DataContext> dbContext)
        {
            this.dbContextOptions = dbContext;
        }
        public async Task CreateUser(string FirstName, string LastName, string Email, string password)
        {
            Console.WriteLine("User check");
            if (UserExists(Email, dbContextOptions))
            {
                string code = SendEmailVerification(Email);
                if (ValidateCode(code))
                {
                    var salt = GenerateSalt();
                    var PasswordHash = HashPasswordAsync(password, salt);
                    try
                    {
                        using (var dataContext = new DataContext(dbContextOptions))
                        {
                            await dataContext.Users.AddAsync(new User
                            {
                                FirstName = FirstName,
                                LastName = LastName,
                                Email = Email,
                                PasswordHash = PasswordHash.ToString(),
                                Salt = salt,
                            });
                            await dataContext.SaveChangesAsync();
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("false email");
                }
                
            }
        }
        public async Task<bool> Login(string Email, string Password)
        {
            var IsLogin=await LoginChecker(Email, Password, false);
            return IsLogin;
        }

        public async Task<bool> LoginChecker(string Email,string Password, bool DeleteUSer)
        {
            try
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    // Проверка существования пользователя с определенным адресом электронной почты
                    var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Email == Email);
                    if (user != null)
                    {
                        string salt = user.Salt;
                        string passwordHash = HashPasswordAsync(Password, salt);
                        if (passwordHash.Equals(user.PasswordHash))
                        {
                            Console.WriteLine("Успешная авторизация");
                            if (DeleteUSer)
                            {
                                dataContext.Users.Remove(user);
                                Console.WriteLine("Пользователь удален");
                                await dataContext.SaveChangesAsync();
                            }
                            return true;
                        }
                        Console.WriteLine("Неверный пароль");
                        return false;
                    }
                    Console.WriteLine("Неверный логин");

                    return false;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> DeleteUser(string Email, string Password)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var loginTask = LoginChecker(Email, Password,true); 
                bool isLoggedIn = await loginTask;
                return isLoggedIn;
            }
        }
        public bool ValidateCode(string code)
        {
            return true;
        }
        public string GenerateSalt()
        {
            byte[] saltBytes = new  byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        private string HashPasswordAsync(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashedPassword = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hashedPassword);
            }
        }
        public string SendEmailVerification(string email)
        {
            // Адрес отправителя
            string fromEmail = "vladred2016@gmail.com";
            Random random = new Random();

            string password = "xxgm atru tips dzdd";
            // Создание экземпляра почтового сообщения
            MailMessage mail = new MailMessage(fromEmail, email);
            mail.IsBodyHtml = true;

            mail.Subject = "Код подтверждения MailASP";

            mail.Body =  $"{random.Next(0,9999)}";



            // Настройка SMTP клиента для Gmail
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {

                smtpClient.Port = 587;

                smtpClient.EnableSsl = true;
                // Аутентификация с использованием учетных данных
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);

                try
                {
                    smtpClient.SendMailAsync(mail).GetAwaiter().GetResult();
                    Console.WriteLine("Письмо отправленно");
                    return mail.Body;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
                    return "";
                }
            }
        }
        public bool UserExists(string email, DbContextOptions<DataContext> dbContextOptions)
        {
            try
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    // Проверка существования пользователя с определенным адресом электронной почты
                    bool exists = dataContext.Users.Any(x => x.Email == email);
                    Console.WriteLine($"Пользователь существует - {exists}");
                    return !exists;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
