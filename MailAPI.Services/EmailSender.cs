using System.Net;
using System.Net.Mail;

namespace MailAPI.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message) 
        {
            // Адрес отправителя
            string fromEmail = "vladred2016@gmail.com";
            // Пароль от почты отправителя
            string password = "xxgm atru tips dzdd";

            // Создание экземпляра почтового сообщения
            MailMessage mail = new MailMessage(fromEmail, email);

            // Тема письма
            mail.Subject = subject;
            // Текст письма
            mail.Body = message;

            // Настройка SMTP клиента для Gmail
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                // Порт для Gmail SMTP
                smtpClient.Port = 587;
                // Используется SSL
                smtpClient.EnableSsl = true;
                // Аутентификация с использованием учетных данных
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);

                try
                {
                    // Отправка письма асинхронно
                    await smtpClient.SendMailAsync(mail);
                    Console.WriteLine("Письмо отправленно");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
                }
            }
        }
    }
}
