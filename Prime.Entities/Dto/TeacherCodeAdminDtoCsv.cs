using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;
public class TeacherCodeAdminDtoCsv
{
    [Required(ErrorMessage ="Please choose the csv file")]
    public IFormFile TeacherCodeFile { get; set; }
}
