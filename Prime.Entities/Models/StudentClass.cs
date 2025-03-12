using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;
public class StudentClass
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int TeacherId { get; set; }
    public string ClassCode { get; set; }
    public int BookId { get; set; }
    public DateTime? AssignDate { get; set; }
}
