using System.Threading.Tasks;
using MQTTnet.Client;
using MQTTnet.Protocol;
using KJ1012.Core.Mqtt;
using KJ1012.Data.Entities.Base;
using KJ1012.Domain;
using KJ1012.Domain.Enums;

namespace KJ1012.Services.Publish
{
    public class DevicePublish : IDevicePublish
    {
        private readonly IMqttClient _mqttClient;

        public DevicePublish(IMqttClient mqttClient)
        {
            _mqttClient = mqttClient;
        }

        public async Task AddPublishAsync(Device device)
        {
            await AddPublishToAsync(device);
        }

        public async Task DeletePublishAsync(Device device)
        {
            await DeletePublishToAsync(device);
        }

        /// <summary>
        /// 新增设备时下发设备信息
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        private async Task AddPublishToAsync(Device device)
        {
            byte[] publishBytes = GetPublishBytes(device);
            if (publishBytes != null)
            {
                switch (device.DeviceType)
                {
                    case DeviceTypeEnum.BaseStation:
                        publishBytes[0] = 24;
                        break;
                    case DeviceTypeEnum.BeaconCard:
                        publishBytes[0] = 26;
                        break;
                    case DeviceTypeEnum.Substation:
                        publishBytes[0] = 22;
                        break;
                    case DeviceTypeEnum.Power:
                        publishBytes[0] = 28;
                        break;
                    default: publishBytes[0] = 0; break;
                }
                await _mqttClient.PublishAsync(string.Concat(ConstDefine.WebServiceToMqttTopic, "/CollectionCenter/5/1"), publishBytes, MqttQualityOfServiceLevel.AtLeastOnce);
            }
        }

        /// <summary>
        /// 删除设备时下发设备信息
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        private async Task DeletePublishToAsync(Device device)
        {
            byte[] publishBytes = GetPublishBytes(device);
            if (publishBytes != null)
            {
                switch (device.DeviceType)
                {
                    case DeviceTypeEnum.BaseStation:
                        publishBytes[0] = 25;
                        break;
                    case DeviceTypeEnum.BeaconCard:
                        publishBytes[0] = 27;
                        break;
                    case DeviceTypeEnum.Substation:
                        publishBytes[0] = 23;
                        break;
                    case DeviceTypeEnum.Power:
                        publishBytes[0] = 29;
                        break;
                    default: publishBytes[0] = 0; break;
                }
                await _mqttClient.PublishAsync(string.Concat(ConstDefine.WebServiceToMqttTopic, "/CollectionCenter/5/2"), publishBytes, MqttQualityOfServiceLevel.AtLeastOnce);
            }
        }

        private byte[] GetPublishBytes(Device device)
        {
            switch (device.DeviceType)
            {
                case DeviceTypeEnum.Substation:
                    {
                        return GetSubstationPublishBytes(device);
                    }
                case DeviceTypeEnum.BaseStation:
                    {
                        return GetBaseStationPublishBytes(device);
                    }
                //暂时先不发送
                //case DeviceType.BeaconCard:
                //    {
                //        return GeBeaconCardPublishBytes(device);
                //    }
                //case DeviceType.Power:
                //    {
                //        return GePowerPublishBytes(device);
                //    }
                default:
                    return null;
            }
        }
        /// <summary>
        /// 分站数据下发
        /// </summary>
        /// <param name="device">设备信息</param>
        /// <returns></returns>
        private byte[] GetSubstationPublishBytes(Device device)
        {
            byte[] publishBytes = new byte[4];
            publishBytes[1] = (byte)DeviceTypeEnum.UpDataInterface;
            publishBytes[2] = 0;
            publishBytes[3] = (byte)device.DeviceNum;
            return publishBytes;
        }
        /// <summary>
        /// 基站数据下发
        /// </summary>
        /// <param name="device">设备信息</param>
        /// <returns></returns>
        private byte[] GetBaseStationPublishBytes(Device device)
        {
            byte[] publishBytes = new byte[7];
            publishBytes[1] = (byte)DeviceTypeEnum.Substation; 
            publishBytes[2] = (byte)device.Substation.DeviceNum;
            publishBytes[3] = (byte)device.Substation.DeviceNum;
            publishBytes[4] = (byte)(device.DeviceNum / 256);
            publishBytes[5] = (byte)(device.DeviceNum % 256);
            publishBytes[6] = (byte)device.SerialNum.GetValueOrDefault(0);
            return publishBytes;
        }
        /// <summary>
        /// 信标卡数据下发
        /// </summary>
        /// <param name="device">设备信息</param>
        /// <returns></returns>
        private byte[] GeBeaconCardPublishBytes(Device device)
        {
            byte[] publishBytes = new byte[7];
            publishBytes[1] = (byte)DeviceTypeEnum.Substation; 
            publishBytes[2] = (byte)device.Substation.DeviceNum;
            publishBytes[3] = (byte)device.Substation.DeviceNum;
            publishBytes[4] = (byte)(device.DeviceNum / 256);
            publishBytes[5] = (byte)(device.DeviceNum % 256);
            publishBytes[6] = (byte)device.SerialNum.GetValueOrDefault(0); 
            return publishBytes;
        }
        /// <summary>
        /// 电源数据下发
        /// </summary>
        /// <param name="device">设备信息</param>
        /// <returns></returns>
        private byte[] GePowerPublishBytes(Device device)
        {
            byte[] publishBytes = new byte[12];
            publishBytes[1] = (byte)DeviceTypeEnum.UpDataInterface; 
            publishBytes[2] = 0;
            publishBytes[3] = (byte)(device.DeviceNum / 256);
            publishBytes[4] = (byte)(device.DeviceNum % 256);
            int.TryParse(device.Extend1, out var extend1);
            publishBytes[5] = (byte)extend1;
            int.TryParse(device.Extend2, out var extend2);
            publishBytes[6] = (byte)extend2;
            int.TryParse(device.Extend3, out var extend3);
            publishBytes[7] = (byte)(extend3 / 256);
            publishBytes[8] = (byte)(extend3 % 256);
            int.TryParse(device.Extend4, out var extend4);
            publishBytes[9] = (byte)extend4;
            int.TryParse(device.Extend5, out var extend5);
            publishBytes[10] = (byte)(extend5 / 256);
            publishBytes[11] = (byte)(extend5 % 256);
            return publishBytes;
        }
    }
}