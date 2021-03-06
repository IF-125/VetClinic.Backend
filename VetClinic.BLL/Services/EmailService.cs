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

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.mailtrap.io", 2525, SecureSocketOptions.StartTls);
            //TODO: remove username and password from code
            smtp.Authenticate("203499a0ee8d71", "4f68c4bc4fdc16");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
