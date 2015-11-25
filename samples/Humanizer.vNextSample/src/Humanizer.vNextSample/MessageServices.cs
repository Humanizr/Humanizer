using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Humanizer.vNextSample
{
    // This class is used by the application to send Email and SMS when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public static class MessageServices
    {
        public static Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
        public static Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
