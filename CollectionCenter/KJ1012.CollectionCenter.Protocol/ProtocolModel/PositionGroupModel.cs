using System;

namespace KJ1012.CollectionCenter.Protocol.ProtocolModel
{
    public class PositionGroupModel: BaseProtocolModel
    {
        public int TerminalId { get; set; }
        public short TerminalState { get; set; }
        public byte PositionWay { get; set; }
        public int Station { get; set; }
        public int Distance { get; set; }
        public DateTime PositionTime { get; set; }
    }
}
