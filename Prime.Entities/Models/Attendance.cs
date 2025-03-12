using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;
public class Attendance
{
    public int AttendanceId { get; set; }
    public int? AdminId { get; set; }
    public int StudentId { get; set; }
    public int TeacherId { get; set; }
    public int Book1 { get; set; }
    public string? ClassCode { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? AttendDate { get; set; } 
    public bool? IsAttend { get; set; }
    public string ? Note { get; set; }
}
