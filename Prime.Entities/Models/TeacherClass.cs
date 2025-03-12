using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;
public class TeacherClass
{
    public int Id { get; set; }
    public string ClassCode { get; set; }
    public string ClassName { get; set; }
    public int TeacherId { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
}

