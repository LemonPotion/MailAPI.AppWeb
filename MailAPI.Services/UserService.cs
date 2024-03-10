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
using Microsoft.Extensions.Logging;

namespace MailAPI.Services
{
    public class UserService : IUserService
    {
        private readonly DbContextOptions<DataContext> dbContextOptions;

        public UserService(DbContextOptions<DataContext> dbContext)
        {
            this.dbContextOptions = dbContext;
        }
        public async Task RegisterUser(string FirstName, string LastName, string Email, string password)
        {
            if (UserExists(Email, dbContextOptions))
            {
                string code = SendEmailVerification(Email);
                if (ValidateCode(code))
                {
                    var salt = GenerateSalt();
                    var PasswordHash = HashPassword(password, salt);
                    try
                    {
                        // Ожидаем завершения операции добавления пользователя
                        bool userAdded = await AddUser(FirstName, LastName, Email, PasswordHash, salt);
                        if (userAdded)
                        {
                            Console.WriteLine("Пользователь успешно зарегистрирован");
                        }
                        else
                        {
                            Console.WriteLine("Ошибка при регистрации пользователя");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при регистрации пользователя: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный код подтверждения электронной почты");
                }

            }
        }
        public async Task<bool> AddUser(string FirstName, string LastName, string Email, string PasswordHash, string salt)
        {
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
                        RoleID = GetRole()
                    });
                    await dataContext.SaveChangesAsync();
                };
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<User> GetUserByEmail(string Email, DataContext dataContext)
        {
            try
            {
                var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Email == Email);
                return user;
            }
            catch
            {
                return null;
            }
        }

        public async Task DeleteUser(User user, DataContext dataContext)
        {
            try
            {
                dataContext.Remove(user);
                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении пользователя: {ex.Message}");
            }
        }
        public async Task<bool> Login(string Email, string Password)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var user = await GetUserByEmail(Email, dataContext);

                if (user != null)
                {
                    var PasswordHashed = HashPassword(Password, user.Salt);

                    if (PasswordHashed.Equals(user.PasswordHash))
                    {
                        Console.WriteLine("Успешная авторизация");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Неверный пароль");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Пользователь с указанным адресом электронной почты не найден");
                    return false;
                }
            }
        }
        public async Task<bool> Logout(string Email)
        {
            try
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    var user = await GetUserByEmail(Email, dataContext);
                    if (user != null)
                    {
                        if (user.Email.Equals(Email))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> DeleteUser(string Email, string Password)
        {
            try
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    var user = await GetUserByEmail(Email, dataContext); // Передаем dataContext в качестве аргумента
                    if (user != null)
                    {
                        var salt = user.Salt;
                        var PasswordHashed = HashPassword(Password, salt);
                        if (user.PasswordHash.Equals(PasswordHashed))
                        {
                            await DeleteUser(user, dataContext); // Передаем dataContext в качестве аргумента
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        public int GetRole()
        {
            return 1;
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
        private string HashPassword(string password, string salt)
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
                    smtpClient.SendMailAsync(mail).GetAwaiter();
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
        private string GenerateRandomToken()
        {
            const int length = 32;
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var tokenBuilder = new StringBuilder(length);
            foreach (byte b in randomBytes)
            {
                tokenBuilder.Append(allowedChars[b % allowedChars.Length]);
            }

            return tokenBuilder.ToString();
        }
        private async Task<bool> AddTokenToDb(string email, string token)
        {
            if (await TokenExists(email))
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    var user = await GetUserByEmail(email, dataContext);
                    dataContext.MailToken.Add(new MailToken
                    {
                        UserID = user.UserID,
                        Key = token,
                        ExpirationDate = DateTime.UtcNow.AddMonths(3),
                    });

                    await dataContext.SaveChangesAsync(); // Ожидание завершения операции сохранения
                }
                return true;
            }
            return false;
        }

        private async Task<bool> TokenExists(string email)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var user = await GetUserByEmail(email, dataContext);
                var userId = user.UserID;
                return await dataContext.MailToken.AnyAsync(x => x.UserID== userId);
            }
        }
    }
}
