using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;
public class TeacherCodeAdminDto
{
   
    [Required(ErrorMessage = "Enter The Teacher Code"), Display(Name = "Teacher Code"), StringLength(10,ErrorMessage ="The Teacher Code length should be 10 Characters Length "), MinLength(10, ErrorMessage = "The Teacher Code length should be 10 Characters Length ")]
    public string TeacherCode { get; set; }
}
