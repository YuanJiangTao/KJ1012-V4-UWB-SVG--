using System;
using System.Collections.Generic;

namespace KJ1012.DataDB.Models
{
    public partial class CallUpCall
    {
        public CallUpCall()
        {
            CallAnswer = new HashSet<CallAnswer>();
        }

        public Guid Id { get; set; }
        public int CallType { get; set; }
        public string Target { get; set; }
        public string TargetExtend { get; set; }
        public int DataType { get; set; }
        public string SubstationId { get; set; }
        public int? BasestationId { get; set; }
        public short CallNum { get; set; }
        public string Data { get; set; }
        public DateTime? CallTime { get; set; }
        public Guid? CreateAccount { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<CallAnswer> CallAnswer { get; set; }
    }
}
