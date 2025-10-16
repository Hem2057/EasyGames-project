using System;
using System.Threading.Tasks;

namespace EasyGames.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            Console.WriteLine($"📧 Email To: {to}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body: {body}");
            return Task.CompletedTask;
        }
    }
}
