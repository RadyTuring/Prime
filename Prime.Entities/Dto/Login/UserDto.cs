using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Entities;

namespace Dto;
public class UserDto
{
    [StringLength(100)]
    [Required(ErrorMessage = "Enter The User Name"), Display(Name = "User Name")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "enter Your Full Name") ,StringLength(60), Display(Name = "Full Name")]
    public string FullName { get; set; }
    [Display(Name = "Profile Picture")]
    public IFormFile? ImageFile { get; set; }
    public int CountryId { get; set; }
    [Required(ErrorMessage = "enter Your Password."), DataType(DataType.Password)]
    public string? Password { get; set; }
}
public class UserUpdateDto
{
    [Required(ErrorMessage = "Enter The User Name"), Display(Name = "User Name")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "enter Your Full Name"), StringLength(60), Display(Name = "Full Name")]
    public string FullName { get; set; }
    [Display(Name = "Profile Picture")]
    public IFormFile? ImageFile { get; set; }
    [Display(Name = "Country")]
    public int CountryId { get; set; }
   
}
