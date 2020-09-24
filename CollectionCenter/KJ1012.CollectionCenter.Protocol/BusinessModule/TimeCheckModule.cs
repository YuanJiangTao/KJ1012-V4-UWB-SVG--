using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using KJ1012.CollectionCenter.Protocol.Protocol;
using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using KJ1012.CollectionCenter.SocketSend;

namespace KJ1012.CollectionCenter.Protocol.BusinessModule
{
    public class TimeCheckModule : IGroupSubscribe<TimeCheckGroupModel>
    {
        private readonly ILogger<TimeCheckModule> _logger;
        private readonly ISocketSendServer _socketSendServer;

        public TimeCheckModule(ILogger<TimeCheckModule> logger, ISocketSendServer socketSendServer)
        {
            _logger = logger;
            _socketSendServer = socketSendServer;
        }

        public async Task ExecReceive(TimeCheckGroupModel protocolModel)
        {
            try
            {
                if (protocolModel != null)
                {
                    var date = DateTime.Now;
                    byte[] bytes = new byte[10];
                    bytes[0] = 11;
                    bytes[1] = (byte)protocolModel.RequestDeviceType;
                    bytes[2] = (byte)protocolModel.RequestId;
                    bytes[3] = (byte)(date.Year % 2000);
                    bytes[4] = (byte)date.Month;
                    bytes[5] = (byte)date.Day;
                    bytes[6] = (byte)date.Hour;
                    bytes[7] = (byte)date.Minute;
                    bytes[8] = (byte)date.Second;
                    bytes[9] = (byte)(date.Millisecond / 10);
                    await Task.Run(() => { _socketSendServer.SendMessage((int)protocolModel.RequestDeviceType, protocolModel.RequestId, bytes); });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.Message ?? e.Message);
            }
        }

    }
}
