using AutoMapper;
using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using KJ1012.Core.Helper;
using KJ1012.Core.Mapper;
using KJ1012.Data.Entities.Position;
using KJ1012.Domain.Enums;
using System;
using KJ1012.Data.Entities.Warn;

namespace KJ1012.CollectionCenter.Protocol.BusinessModule.Mapper
{
    public class ModuleMapperConfiguration : Profile, IMapperProfile
    {
        public ModuleMapperConfiguration()
        {
            CreateMap<PositionGroupModel, Position>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiveFrom, opt => opt.UseValue(1))
                .ForMember(desc => desc.CreateDate, opt => opt.Ignore());

            CreateMap<PositionGroupModel, TerminalWarn>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TerminalState, opt => opt.MapFrom(r => r.TerminalState & 0x0f))
                .ForMember(desc => desc.CreateDate, opt => opt.Ignore());

            CreateMap<Position, DownMember>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataFrom, opt => opt.MapFrom(r => r.ReceiveFrom))
                .ForMember(dest => dest.TerminalState, opt => opt.MapFrom(r => r.TerminalState & 0x0f))
                .ForMember(dest => dest.SinsState, opt => opt.MapFrom(r => (r.TerminalState & 0xf0) >> 4))
                .ForMember(desc => desc.CreateDate, opt => opt.Ignore());

            CreateMap<PositionGroupModel, MemberPositionGroupModel>();

            CreateMap<PositionGroupModel, Position>()
                .ForMember(dest => dest.ReceiveFrom, opt => opt.UseValue(1));

            CreateMap<PositionContinueGroupModel, Position>()
                .ForMember(dest => dest.ReceiveFrom, opt => opt.UseValue(6))
                .ForMember(dest => dest.PositionTime,
                    opt => opt.MapFrom(r => CommonHelper.ConvertToUtcDateTime(r.Timestamp)));

            CreateMap<PositionGroupModel, TerminalWarn>()
                .ForMember(dest => dest.TerminalState, opt => opt.MapFrom(r => r.TerminalState & 0x0f));
        }


        public int Order => 1;
    }
}
