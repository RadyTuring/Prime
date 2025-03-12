using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class AdminUserDto
{
    [Required(ErrorMessage = "Enter the Patch Name"), Display(Name = "Patch Name")]
    public string? PatchDesc { get; set; }
    public string? PatchType { get; set; }
    [Required(ErrorMessage = "Enter the number of Codes"), Display(Name = "Number Of Codes"), Range(1, 1000, ErrorMessage = "The Number Between 1 and 1000")]
    public int NumberOfCodes { get; set; }
}