using System.Threading.Tasks;

namespace EasyGames.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
