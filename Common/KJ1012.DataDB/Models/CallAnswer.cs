using System;

namespace KJ1012.DataDB.Models
{
    public partial class CallAnswer
    {
        public Guid Id { get; set; }
        public short CallNum { get; set; }
        public int AnswerType { get; set; }
        public int TerminalId { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual CallUpCall CallNumNavigation { get; set; }
    }
}
