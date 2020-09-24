using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KJ1012.CollectionCenter.Protocol.Protocol;
using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using KJ1012.CollectionCenter.SocketSend;
using KJ1012.Data.Entities.Base;
using KJ1012.Domain.Enums;
using KJ1012.Services.IServices.Base;

namespace KJ1012.CollectionCenter.Protocol.BusinessModule
{
    public class BaseStationListModule : IGroupSubscribe<BaseStationListGroupModel>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISocketSendServer _socketSendServer;
        private readonly ILogger<BaseStationListModule> _logger;

        public BaseStationListModule(IServiceProvider serviceProvider, ISocketSendServer socketSendServer,
            ILogger<BaseStationListModule> logger)
        {
            _serviceProvider = serviceProvider;
            _socketSendServer = socketSendServer;
            _logger = logger;
        }

        public async Task ExecReceive(BaseStationListGroupModel protocolModel)
        {
            try
            {
                if (protocolModel != null)
                {
                    //依赖注入是以作用域管理对象，net core 默认作用域为每一个请求
                    //在这里没有请求，所有通过IServiceProvider构建一个作用域对象，保证后面的数据操作引用异步始终都是独立的对象
                    using (var serviceScope = _serviceProvider.CreateScope())
                    {
                        IDeviceService service =
                            serviceScope.ServiceProvider.GetService<IDeviceService>();
                        var devices = await service.GetDevicesByTypeAndSubNum(DeviceTypeEnum.BaseStation, protocolModel.RequestSubstation).ToListAsync();
                        var bytes = SendBytes(protocolModel, devices);
                        _socketSendServer.SendMessage((int)protocolModel.RequestDeviceType, protocolModel.RequestNum, bytes);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.Message ?? e.Message);
            }
        }

        private byte[] SendBytes(BaseStationListGroupModel protocolModel, IList<Device> devices)
        {
            List<byte> bytes = new List<byte>
            {
                20,
                (byte) protocolModel.RequestDeviceType,
                (byte) protocolModel.RequestNum,
                (byte) protocolModel.RequestSubstation,
                (byte) devices.Count
            };
            foreach (var device in devices)
            {
                bytes.Add((byte)device.SerialNum.GetValueOrDefault(0));
                bytes.Add((byte)(device.DeviceNum / 256));
                bytes.Add((byte)(device.DeviceNum % 256));
            }
            return bytes.ToArray();
        }
    }
}
