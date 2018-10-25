namespace Sales.TestNotifications
{
    using System;
    using Microsoft.Azure.NotificationHubs;

    public class Program
    {
        private static NotificationHubClient hub;

        public static void Main(string[] args)
        {
            hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://salesprep.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=xIAGRRWWWe7GNbkMVYmi67X3d+hjXAN/bWV96xmbmwQ=", "SalesPrep");

            do
            {
                Console.WriteLine("Type a new message:");
                var message = Console.ReadLine();
                SendNotificationAsync(message);
                Console.WriteLine("The message was sent...");
            } while (true);
        }

        private static async void SendNotificationAsync(string message)
        {
            await hub.SendGcmNativeNotificationAsync("{ \"data\" : {\"Message\":\"" + message + "\"}}");
        }

    }
}
