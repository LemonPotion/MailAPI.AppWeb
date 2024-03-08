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
        public EmailService(DbContextOptions<DataContext> dbContext)
        {
            this.dbContextOptions = dbContext;
        }
        public async Task GetMessageHistory()
        {
            try
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    await dataContext.MessageHistory.ToListAsync();
                };
                Console.WriteLine("Получен список истории сообщений");

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task<bool> SendEmailDb(string email, string subject, string message)
        {
            try
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    await dataContext.Message.AddAsync(new Message
                    {
                        MailAdress = email,
                        Subject = subject,
                        Body = message,
                        DateSent = DateTime.Now
                    });
                    await dataContext.SaveChangesAsync();
                };
                Console.WriteLine("Сообщение добавлено в бд");
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        public async Task SendEmailAsync(string email, string subject, string message) 
        {
            // Адрес отправителя
            string fromEmail = "vladred2016@gmail.com";

            string password = "xxgm atru tips dzdd";
            string bodyhtml =GetHTML(email,subject,message);
            // Создание экземпляра почтового сообщения
            MailMessage mail = new MailMessage(fromEmail, email);
            mail.IsBodyHtml = true;

            mail.Subject =  subject;

            mail.Body = bodyhtml;

            

            // Настройка SMTP клиента для Gmail
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {

                smtpClient.Port = 587;

                smtpClient.EnableSsl = true;
                // Аутентификация с использованием учетных данных
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);

                try
                {
                    await smtpClient.SendMailAsync(mail);
                    Console.WriteLine("Письмо отправленно");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
                }
            }
        }
        public string GetHTML(string email, string subject, string message)
        {
            //Убирает @mail.com и оставляет только имя
            string MailName = "";
            int atIndex = email.IndexOf('@');
            if (atIndex != -1) 
            {
                MailName = email.Substring(0, atIndex);
            }
            else
            {

            }
            //
            string htmlbody = @$"<!DOCTYPE html>
                <html>
                  <body>
                    <table width=""80%"" cellpadding=""0"" cellspacing=""0"" border=""0"" align=""center"" style=""border: 1px solid #ddd; border-radius: 5px; padding: 20px;"">
                      <tbody>
                        <tr>
                          <td>
                            <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                              <tbody>
                                <tr>
                                  <td style=""text-align: left;"">
                                    <h1>{subject}</h1>
                                  </td>
                                </tr>
                              </tbody>
                            </table>
                          </td>
                        </tr>
                        <tr>
                          <td style=""padding: 30px 0;"">
                            <p>Дорогой {MailName},</p>
                            <p>{message}</p>
                          </td>
                        </tr>
                        <tr>
                          <td style=""text-align: center;"">
                            <p>Copyright © 2024 MailASP. All rights reserved.</p>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </body>
                </html>";
            return htmlbody;
        }
    }
}
