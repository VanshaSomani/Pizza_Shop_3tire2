using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration){
        _configuration= configuration;
    }
    public async Task SendAsyncEmail(string EmailTo, string Subject, string EmailBody)
    {
        string MailServer = _configuration["EmailSettings:MailServer"];
        int MailPort = int.Parse(_configuration["EmailSettings:MailPort"]);
        string SenderName = _configuration["EmailSettings:SenderName"];
        string EmailFrom = _configuration["EmailSettings:FromEmail"];
        string Pass = _configuration["EmailSettings:Password"];
        //creating smtp object
        SmtpClient smtp = new SmtpClient(MailServer, MailPort);
        //setting credentials
        smtp.Credentials = new NetworkCredential(EmailFrom, Pass);
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;

        //seeting sender email
        MailAddress Sender = new MailAddress(EmailFrom , SenderName);

        //setting mail and revicer info
        MailMessage message = new MailMessage();
        message.From = Sender;
        message.Subject = Subject;
        message.Body = EmailBody;
        message.IsBodyHtml = true;
        message.To.Add(EmailTo);
        
        await smtp.SendMailAsync(message);
    }

}
