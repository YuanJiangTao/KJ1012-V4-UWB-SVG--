using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Protocol;
using Newtonsoft.Json;

namespace KJ1012.Core.Mqtt
{
   public static class ApplicationMessagePublisherExtension
    {
        public static Task PublishAsync(this IApplicationMessagePublisher publisher, string topic, byte[] payload, MqttQualityOfServiceLevel qualityOfServiceLevel)
        {
            if (publisher == null)
                throw new ArgumentNullException(nameof(publisher));
            if (topic == null)
                throw new ArgumentNullException(nameof(topic));
            return publisher.PublishAsync(new[]
            {
                new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(payload).WithQualityOfServiceLevel(qualityOfServiceLevel).Build()
            });
        }
        public static Task PublishAsync(this IApplicationMessagePublisher publisher, string topic, object data, MqttQualityOfServiceLevel qualityOfServiceLevel=MqttQualityOfServiceLevel.ExactlyOnce)
        {
            try
            {
                if (publisher == null)
                    throw new ArgumentNullException(nameof(publisher));
                if (topic == null)
                    throw new ArgumentNullException(nameof(topic));
                if (data == null)
                    throw new ArgumentNullException(nameof(data));
                string value = JsonConvert.SerializeObject(data,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                        DateFormatString = "yyyy-MM-dd HH:mm:ss"
                    });
                return publisher.PublishAsync(new[]
                {
                new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(value).WithQualityOfServiceLevel(qualityOfServiceLevel).Build()
            });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
