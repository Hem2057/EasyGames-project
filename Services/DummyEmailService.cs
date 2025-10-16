using System.Diagnostics;

namespace EasyGames.Services
{
    public class DummyEmailService : IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            Debug.WriteLine($"[EMAIL LOG] To: {to} | Subject: {subject} | Body: {body}");
            return Task.CompletedTask;
        }
    }
}
