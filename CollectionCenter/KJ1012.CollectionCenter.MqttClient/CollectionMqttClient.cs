using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Protocol;
using KJ1012.CollectionCenter.MqttClient.Protocol;
using KJ1012.Domain;
using KJ1012.Domain.Setting;

namespace KJ1012.CollectionCenter.MqttClient
{
    public class CollectionMqttClient : MQTTnet.Client.MqttClient, IHostedService
    {
        private readonly IOptionsMonitor<Setting> _kj1012Setting;
        private readonly ISendProtocolFactory _sendProtocolFactory;
        private readonly ILogger<CollectionMqttClient> _logger;

        public CollectionMqttClient(IMqttClientAdapterFactory factory,
            IMqttNetLogger mqttLogger,
            ISendProtocolFactory sendProtocolFactory,
            IOptionsMonitor<Setting> kj1012Setting,
            ILogger<CollectionMqttClient> logger) : base(factory, mqttLogger)
        {
            _sendProtocolFactory = sendProtocolFactory;
            _logger = logger;
            _kj1012Setting = kj1012Setting;
            Connected += MqttClient_Connected;
            Disconnected += MqttClient_Disconnected;
            ApplicationMessageReceived += MqttClient_ApplicationMessageReceived; ;
        }

        private async void MqttClient_Disconnected(object sender, MqttClientDisconnectedEventArgs e)
        {
            _logger.LogError($"Mqtt Client Disconnected To MqttService:{e.Exception.Message}");
            await Task.Delay(TimeSpan.FromSeconds(1));

            try
            {
                var kj1012Setting = _kj1012Setting.CurrentValue;
                if (new MqttClientOptionsBuilder()
                    .WithClientId(MqttClientId)
                    .WithTcpServer(kj1012Setting.MqttAddress)
                    .Build() is MqttClientOptions mqttClientOptions)
                {
                    mqttClientOptions.KeepAliveSendInterval = mqttClientOptions.KeepAlivePeriod * 0.5D;
                    await ConnectAsync(mqttClientOptions);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"与Mqtt消息服务建立连接失败:{ex.Message}");
            }
        }
        private string MqttClientId => string.Concat(ConstDefine.MqttCollectionClientId, new Random().Next(1, 1000));
        private async void MqttClient_Connected(object sender, MqttClientConnectedEventArgs e)
        {
            _logger.LogInformation("Mqtt Connected To MqttService");
            await this.SubscribeAsync("Sunny/+/CollectionCenter/#",MqttQualityOfServiceLevel.AtLeastOnce);
        }


        private void MqttClient_ApplicationMessageReceived(object sender,
            MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                byte[] bytes = e.ApplicationMessage.Payload;
                var protocolId = (int) bytes[0];
                ISendProtocol sendProtocol = _sendProtocolFactory.Create(protocolId);
                sendProtocol?.Receive(bytes);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.InnerException?.Message ?? exception.Message);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    await Task.Delay(2000, cancellationToken);
                    var kj1012Setting = _kj1012Setting.CurrentValue;
                    if (new MqttClientOptionsBuilder()
                        .WithClientId(MqttClientId)
                        .WithTcpServer(kj1012Setting.MqttAddress)
                        .Build() is MqttClientOptions mqttClientOptions)
                    {
                        mqttClientOptions.KeepAliveSendInterval = mqttClientOptions.KeepAlivePeriod * 0.5D;
                        await ConnectAsync(mqttClientOptions);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"与Mqtt消息服务建立连接失败:{e.Message}");
                }
            }, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (IsConnected)
            {
                await DisconnectAsync();
            }
        }

    }
}
