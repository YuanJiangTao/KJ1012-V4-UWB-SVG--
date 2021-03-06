﻿using System;

namespace KJ1012.DataDB.Models
{
    public partial class LocAttendance
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TerminalId { get; set; }
        public int? InMachine { get; set; }
        public int InType { get; set; }
        public DateTime InTime { get; set; }
        public int? OutMachine { get; set; }
        public int? OutType { get; set; }
        public DateTime? OutTime { get; set; }
        public int? TotalMinutes { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
