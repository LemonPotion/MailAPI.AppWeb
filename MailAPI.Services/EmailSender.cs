using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MailAPI.Data.Models;
using MailAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MailAPI.Services
{
    public class EmailSender
    {
        private readonly DbContextOptions<DataContext> dbContextOptions;

        public EmailSender(DbContextOptions<DataContext> dbContext)
        {
            this.dbContextOptions = dbContext;
        }
        public static bool IsValidEmail(string email)
        {

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";


            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
        public string SendEmailVerification(string email)
        {
            bool valid = IsValidEmail(email);
            if (!valid)
            {
                // Адрес отправителя
                string fromEmail = "vladred2016@gmail.com";
                Random random = new Random();
                int code = random.Next(0, 9999);
                string password = "xxgm atru tips dzdd";
                // Создание экземпляра почтового сообщения
                MailMessage mail = new MailMessage(fromEmail, email);
                mail.IsBodyHtml = true;

                mail.Subject = "Код подтверждения MailASP";

                mail.Body = GetHTML(email, mail.Subject, code.ToString());



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
                        Console.WriteLine("Письмо отправлено");
                        return mail.Body;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
                        return "";
                    }
                }
            }
            return "";
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            // Проверяем валидность email-адреса
            if (!IsValidEmail(email))
            {
                Console.WriteLine("Неверный формат email-адреса.");
                return false;
            }
            // Адрес отправителя
            string fromEmail = "vladred2016@gmail.com";

            string password = "xxgm atru tips dzdd";
            string bodyhtml = GetHTML(email, subject, message);
            // Создание экземпляра почтового сообщения
            MailMessage mail = new MailMessage(fromEmail, email);
            mail.IsBodyHtml = true;

            mail.Subject = subject;

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
                    Console.WriteLine("Письмо отправлено");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
                }
            }
            return false;
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
