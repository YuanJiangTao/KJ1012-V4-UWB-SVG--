using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KJ1012.Data.Entities.Base
{
    public partial class Member:BaseEntity
    {
        public string Name { get; set; }
        public string Sex { get; set; }
        public string MemberNum { get; set; }
        public int TerminalId { get; set; }
        public Guid? OrganizationId { get; set; }
        public string WorkAddress { get; set; }
        public bool IsLeader { get; set; }
        public bool IsSpecialOp { get; set; }
        public string BloodType { get; set; }
        public Guid? EthnicityId { get; set; }
        public Guid? WorkTypeId { get; set; }
        public Guid? PositionId { get; set; }
        public Guid? PositionalTitlesId { get; set; }
        public string CardId { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNum { get; set; }
        public Guid? PoliticalId { get; set; }
        public Guid? EducationalId { get; set; }
        public string GraduateSchool { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? FirstJobDate { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public Guid? MaritalId { get; set; }
        public Guid? StateId { get; set; }
        public int AttModel { get; set; }
        public int MinMinutes { get; set; }
        public int MaxMinutes { get; set; }
        public bool IsHide { get; set; }
        public DateTime SysStartTime { get; set; }
        public DateTime SysEndTime { get; set; }
        public static string TableName => "Base_Member";
    }
}
