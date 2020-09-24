using System;
using System.ComponentModel.DataAnnotations.Schema;
using KJ1012.Data.Entities;
using KJ1012.Domain.Enums;

namespace KJ1012.Data.Views
{
    public class ViewTerminal : BaseView
    {
        public Guid Id { get; set; }
        public int TerminalId { get; set; }
        public TerminalTypeEnum Type { get; set; }
        [NotMapped]
        public static string TableName => "View_Terminal";

    }
}
