using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace ServiceBus.Producer.Services
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly ITopicClient _topicClient;
        //private readonly IQueueClient _queueClient;

        public MessagePublisher(ITopicClient topicClient/*, IQueueClient queueClient*/)
        {
            _topicClient = topicClient;
            //_queueClient = queueClient;
        }

        public Task Publish<T>(T obj)
        {
            var objAsText = JsonConvert.SerializeObject(obj);
            var message = new Message(Encoding.UTF8.GetBytes(objAsText));
            //return _queueClient.SendAsync(message);
            message.UserProperties["messageType"] = typeof(T).Name;
            return _topicClient.SendAsync(message);
        }

        public Task Publish(string raw)
        {
            var message = new Message(Encoding.UTF8.GetBytes(raw));
            //return _queueClient.SendAsync(message);
            message.UserProperties["messageType"] = "Raw";
            return _topicClient.SendAsync(message);
        }
    }
}