using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class AttendRecordDto
{
    public DateTime? AttendDate { get; set; }
    public int Book1 { get; set; }
    public string? ClassCode { get; set; }
    public List<AttendRecordDet> StudentsAttend { get; set; }

}
public class AttendRecordDet
{
    public int StudentId { get; set; }
    public bool? IsAttend { get; set; }
    public string? Note { get; set; }
}


