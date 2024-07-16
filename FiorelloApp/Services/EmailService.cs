using FiorelloApp.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace FiorelloApp.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string body, List<string> emails, string title, string subject)
        {
            //string url = Url.Action(nameof(ResetPassword), "Account",
            //new { email = appUser.Email, token }, Request.Scheme, Request.Host.ToString());

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("nadirssh@code.edu.az", title);
            foreach (var email in emails)
            {
                mailMessage.To.Add(new MailAddress(email));
            }
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;
            SmtpClient smtpClient = new()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("nadirssh@code.edu.az", "cfgw mbzy lmdr weow"),
            };
            smtpClient.Send(mailMessage);
        }
    }
}
