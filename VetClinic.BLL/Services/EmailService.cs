using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using VetClinic.Core.Interfaces.Services;

namespace VetClinic.BLL.Services
{
    public class EmailService : IEmailService
    {
        public void Send(string from, string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.mailtrap.io", 2525, SecureSocketOptions.StartTls);
            smtp.Authenticate("login", "password");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
