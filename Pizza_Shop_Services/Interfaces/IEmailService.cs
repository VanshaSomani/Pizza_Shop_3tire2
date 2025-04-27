namespace Pizza_Shop_Services.Interfaces;

public interface IEmailService
{
    public Task SendAsyncEmail(string EmailTo , string Subject , string EmailBody);
}
