using System;
using System.ComponentModel.DataAnnotations;

namespace ScrumManager.Models
{
    public class CustomExportForm
    {
        [Required]
        public string GroupId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime ExportStartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime ExportEndDate { get; set; }

        public bool ShowWeekends { get; set; }

        [Required(ErrorMessage = "Grouping type is required")]
        public ExportGroupType GroupingType { get; set; }
    }

    public enum ExportGroupType
    {
        Date,
        User
    }
}

