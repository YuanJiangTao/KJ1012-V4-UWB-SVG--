namespace KJ1012.CollectionCenter.Protocol.ProtocolModel
{
    public class PositionContinueGroupModel: BaseProtocolModel
    {
        public int TerminalId { get; set; }
        public short TerminalState { get; set; }
        public byte PositionWay { get; set; }
        public int Station { get; set; }
        public short Distance { get; set; }
        public int Timestamp { get; set; }
    }
}
