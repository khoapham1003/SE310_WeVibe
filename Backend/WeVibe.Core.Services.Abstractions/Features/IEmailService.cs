namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
