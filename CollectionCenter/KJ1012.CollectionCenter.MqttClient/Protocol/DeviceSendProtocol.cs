using KJ1012.CollectionCenter.SocketSend;
using KJ1012.Domain.Enums;

namespace KJ1012.CollectionCenter.MqttClient.Protocol
{
    public class DeviceSendProtocol : ISendProtocol
    {
        private readonly ISocketSendServer _socketSendServer;

        public DeviceSendProtocol(ISocketSendServer socketSendServer)
        {
            _socketSendServer = socketSendServer;
        }

        public bool IsMatch(int protocolId)
        {
            return protocolId >= 22 && protocolId <= 29;
        }

        public void Receive(byte[] bytes)
        {
            var id = (int)bytes[0];
            var deviceType = (int)bytes[1];
            var deviceNum = (int)bytes[2];
            //新增和删除分站时将数据修改为发送分站状态协议数据
            if (id == 22 || id == 23)
            {
                byte[] sendBytes = {
                    3,
                    4,
                    0,
                    bytes[3],
                    0
                };
                //发送到地面信息接口
                _socketSendServer.SendMessage((int)DeviceTypeEnum.UpDataInterface, 0, sendBytes);
                //发送井下数据接口
                _socketSendServer.SendMessage((int)DeviceTypeEnum.DownDataInterface, 0, sendBytes);
            }
            else
            {
                _socketSendServer.SendMessage(deviceType, deviceNum, bytes);
            }
        }
    }
}
