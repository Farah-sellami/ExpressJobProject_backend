
using NETCore.MailKit.Core;

using System.Text;

namespace JobExpressBack.Models.Repositories.RepoNotification
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IEmailService emailService;

        public NotificationRepository(IEmailService emailService)
        {
            this.emailService = emailService;
        }

       
    }
}

