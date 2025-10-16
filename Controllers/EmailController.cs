using Microsoft.AspNetCore.Mvc;
using EasyGames.Services;   // your email service interface
using System.Threading.Tasks;

namespace EasyGames.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // GET: /Email
        [HttpGet]
        public IActionResult Index()
        {
            return View();   // this renders Views/Email/Index.cshtml
        }

        // POST: /Email/Send
        [HttpPost]
        public async Task<IActionResult> Send(string tier, string subject, string message)
        {
            // In your real app, you’d filter users by tier, but here just log or simulate
            await _emailService.SendEmailAsync("test@easygames.local", subject, message);
            
            ViewBag.Status = $"Email sent successfully (Tier: {tier ?? "All"})!";
            return View("Index");
        }
    }
}
