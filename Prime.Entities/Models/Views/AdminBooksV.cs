using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   Entities;
 public class AdminBooksV
{
    public long Id { get; set; }
    public int? AdminId { get; set; }
    public int? BookId { get; set; }
    public string? BookName { get; set; } = string.Empty;
    public long? TeacherCodesCount { get; set; }
    public long? AssignedTeachersCount { get; set; }
}
