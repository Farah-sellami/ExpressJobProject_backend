using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace JobExpressBack.Models.Repositories.RepoNotification
{
    public class FirebaseService
    {
        public FirebaseService()
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("../Firebase/expressjobnotification-3f007c36ed24.json"),
            });
        }

        public async Task<string> SendNotificationAsync(string token)
        {
            var message = new Message()
            {
                Token = token,
                Notification = new Notification
                {
                    Title = "Nouvelle demande de service",
                    Body = "Vous avez une nouvelle demande de service "
                },
            };

            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }
    }
}
