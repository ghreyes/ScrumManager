using System;

namespace ScrumManager.Models
{
    public class CustomExportForm
    {
        public string GroupId { get; set; }
        public DateTime ExportStartDate { get; set; }
        public DateTime ExportEndDate { get; set; }
        public bool ShowWeekends { get; set; }
        public ExportGroupType GroupingType { get; set; }
    }

    public enum ExportGroupType
    {
        User,
        Date
    }
}

