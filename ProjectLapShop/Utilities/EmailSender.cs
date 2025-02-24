using MailKit.Net.Smtp;
using MimeKit;
using ProjectLapShop.Utilities;
namespace ProjectLapShop.Utilities
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message);
    }
    public class EmailSender: IEmailSender
    {   
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Ahmed Mansour", "ah8455545@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlMessage };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("ah8455545@gmail.com", "fwcl hwnh dcqz ahca");
                await client.SendAsync(message);    
                await client.DisconnectAsync(true);
            }   
        }    

    }
}
