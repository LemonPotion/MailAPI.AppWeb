using Azure.Core;
using MailAPI.Data;
using MailAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Net;
using System.Net.Mail;

namespace MailAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly DbContextOptions<DataContext> dbContextOptions;
        private readonly EmailSender emailSender;

        public EmailService(DbContextOptions<DataContext> dbContext, EmailSender emailSender)
        {
            this.dbContextOptions = dbContext;
            this.emailSender = emailSender;

        }
        public async Task<List<Message>> GetMessageHistory(int id, int pageNumber, int pageSize)
        {
            try
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    var messagehistory = await dataContext.Message
                        .Where(m => m.UserID == id)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                    return messagehistory;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Message>();
            }
        }

        public async Task<bool> SendEmailDb(int userId, string email, string subject, string message)
        {
            try
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                   //добавить проверку на существование пользователя
                        await dataContext.Message.AddAsync(new Message
                        {
                            UserID = userId,
                            MailAdress = email,
                            Subject = subject,
                            Body = message,
                            DateSent = DateTime.Now
                        });
                    await dataContext.SaveChangesAsync();
                        Console.WriteLine("Сообщение добавлено в бд");
                        return true;
                };

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> SendEmail(int userId,string email, string subject, string message)//отправка сообщения на почту и бд
        {
            try
            {

                var sended = await emailSender.SendEmailAsync(email, subject, message); //может быть такое что письмо отправлено но адреса не существует
                if (sended)
                {
                    var posted = await SendEmailDb(userId,email, subject, message);
                    if (posted)
                    {
                        return true;
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
        public async Task<Message> GetMessageById(int Id)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var message = await dataContext.Message.FirstOrDefaultAsync(x => x.MessageID== Id);
                if (message != null)
                return message;
                else
                return new Message();
            }
        }

        public async Task<Message> EditMessage(int Id,string body,string subject)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var message = await GetMessageById(Id);
                message.Body = body;
                message.Subject = subject;
                // Помечаем объект как измененный
                dataContext.Update(message);

                // Сохраняем изменения в базе данных
                await dataContext.SaveChangesAsync();
                return message;
            }
        }
        public async Task<bool> DeleteMessage(int id)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var message = await GetMessageById(id);
                if (message != null)
                {
                    dataContext.Remove(message);
                    await dataContext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
