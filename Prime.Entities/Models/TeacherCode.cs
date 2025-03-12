using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Entities;
public class TeacherCode
{
    public int Id { get; set; }
    public string TCode { get; set; }
    public int? UserId { get; set; }
    public int? AdminId { get; set; }
    public DateTime? AssignDate { get; set; }
    public int BookId { get; set; }
    public int? PatchNumber { get; set; }
    public DateTime? ValidUpToDate { get; set; }
}
