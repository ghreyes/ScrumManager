using System;
using System.ComponentModel.DataAnnotations;

namespace ScrumManager.Models
{
    public class DailyExportForm
    {
        [Required]
        public string GroupId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime ExportDate { get; set; }
    }
}
