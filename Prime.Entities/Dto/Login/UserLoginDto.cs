using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class UserLoginDto
{
    [StringLength(100)]
    [Required(ErrorMessage = "Enter The User Name"), Display(Name = "User Name")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "enter Your Password."), DataType(DataType.Password)]
    public string Password { get; set; }

}
