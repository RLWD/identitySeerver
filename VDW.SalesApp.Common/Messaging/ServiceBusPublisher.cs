using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace VDW.SalesApp.Common.Messaging
{
    public class ServiceBusPublisher<T>
    {
        public async void PublishMessage(string connectionString, string topicName, T objects)
        {
            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender(topicName);
            string messageBody = JsonConvert.SerializeObject(objects);
            ServiceBusMessage message = new ServiceBusMessage(messageBody);
            await sender.SendMessageAsync(message);
        }
    }
}
