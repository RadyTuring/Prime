using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   Entities;
[Keyless]
public class AdminTeachersV
{
    [Display(Name = "Admin Id")]
    public int? AdminId { get; set; }
    [Display(Name = "Admin Code")]
    public string? AdminCode { get; set; }
    [Display(Name = "Admin User Name")]
    public string? AdminUserName { get; set; }
    [Display(Name = "Admin Full Name")]
    public string? AdminFullName { get; set; }
    [Key]
    public int? TeacherId { get; set; }
    [Display(Name = "T User Name")]
    public string? TeacherUserName { get; set; }
    [Display(Name = "T Full Name")]
    public string? TeacherFullName { get; set; }
    [Display(Name = "Country")]
    public string? CountryName { get; set; }
    public string? Status { get; set; }
}
