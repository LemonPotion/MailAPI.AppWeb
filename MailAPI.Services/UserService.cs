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
using MailAPI.Data.Migrations;
using System.Data;

namespace MailAPI.Services
{
    public class UserService : IUserService
    {
        private readonly DbContextOptions<DataContext> dbContextOptions;
        private readonly PasswordService passwordService;
        private readonly EmailSender emailSender;

        public UserService(DbContextOptions<DataContext> dbContext, PasswordService passwordService, EmailSender emailSender)
        {
            this.dbContextOptions = dbContext;
            this.passwordService = passwordService;
            this.emailSender = emailSender;

        }
        public async Task<bool> RegisterUser(string FirstName, string LastName, string Email, string password,int roleid)
        {
            if (UserExists(Email, dbContextOptions))
            {
                string code = emailSender.SendEmailVerification(Email);
                if (ValidateCode(code))
                {
                    var salt = passwordService.GenerateSalt();
                    var PasswordHash = passwordService.HashPassword(password, salt);
                    try
                    {
                        // Ожидаем завершения операции добавления пользователя
                        bool userAdded = await AddUser(FirstName, LastName, Email, PasswordHash, salt, roleid);
                        if (userAdded)
                        {
                            Console.WriteLine("Пользователь успешно зарегистрирован");
                            return true;
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
            return false;
        }
        public async Task<bool> AddUser(string FirstName, string LastName, string Email, string PasswordHash, string salt , int roleid)
        {
            try
            {
                
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    var role = await GetRole(roleid,dataContext);
                    if (role != null)
                    {
                        await dataContext.Users.AddAsync(new User
                        {
                            FirstName = FirstName,
                            LastName = LastName,
                            Email = Email,
                            PasswordHash = PasswordHash.ToString(),
                            Salt = salt,
                            RoleID = role.RoleId,
                            Role = role,
                        });
                        await dataContext.SaveChangesAsync();
                        return true;
                    }
                };
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<User> GetUserById(int id)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var user = await dataContext.Users.FirstOrDefaultAsync(x => x.UserID == id);
                if (user != null)
                    return user;
                else
                    return new User();
            }
        }

        public async Task<User> EditUser(int id, string Email, string Password)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var user = await GetUserById(id);
                if (user != null)
                {
                    var salt = passwordService.GenerateSalt();
                    user.Email = Email;
                    user.Salt = salt; // Генерируем новую соль
                    user.PasswordHash = passwordService.HashPassword(Password, salt); // Хешируем пароль с новой солью

                    // Помечаем объект как измененный
                    dataContext.Update(user);

                    // Сохраняем изменения в базе данных
                    await dataContext.SaveChangesAsync();
                    return user;
                }
                return new User();
            }
        }

        public async Task<User> GetUserByEmail(string Email, DataContext dataContext)
        {
            var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Email == Email);
            if (user != null)
            return user;
            else
                return new User();
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
                    var PasswordHashed = passwordService.HashPassword(Password, user.Salt); // Используем соль из базы данных для хэширования

                    if (PasswordHashed.Equals(user.PasswordHash))
                    {
                        Console.WriteLine("Успешная авторизация");
                        var tokenExists = await TokenExists(user.UserID);
                        if (!tokenExists)
                        {
                            var token = GenerateRandomToken();
                            var TokenAdded = await AddTokenToDb(user.UserID, token);
                            Console.WriteLine($"Статус токена - {TokenAdded}");
                            if (TokenAdded)
                                return true;
                        }
                        else
                        {
                            var token = await   GetToken(user.UserID);
                            if (token.ExpirationDate >DateTime.Now)
                            {
                                await Logout(user.Email);
                            }
                        }
                        return false;
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
                        var token = await GetToken(user.UserID);
                        if (token.TokenID!=0)
                        {
                            dataContext.Remove(token);
                            await dataContext.SaveChangesAsync();
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
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
                        var PasswordHashed = passwordService.HashPassword(Password, salt);
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


        public async Task<Role> GetRole(int id, DataContext dataContext)
        {

                var role = await dataContext.Role.FirstOrDefaultAsync(x => x.RoleId == id);
                if(role != null)
                return role;
                else
                return new Role();
        }
        public bool ValidateCode(string code)
        {
            return true;
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

        public async Task<Role> GetRole(int id)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var role = await dataContext.Role.FirstOrDefaultAsync(x => x.RoleId == id);
                return role ?? new Role();
            }
        }
        public async Task<bool> AddTokenToDb(int id, string token)
        {
            var TokenEx = await TokenExists(id);
            if (!TokenEx)
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    var user = await GetUserById(id);
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

        public string GenerateRandomToken()
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

        public async Task<bool> TokenExists(int id)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var user = await GetUserById(id);
                var userId = user.UserID;
                return await dataContext.MailToken.AnyAsync(x => x.UserID == userId);
            }
        }
        public async Task<MailToken> GetToken(int id)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var token = await dataContext.MailToken.FirstOrDefaultAsync(x => x.UserID == id);
                if (token != null)
                    return token;
                else
                    return new MailToken();
            }
        }
    }
}
