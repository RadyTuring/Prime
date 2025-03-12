
using System.ComponentModel.DataAnnotations;

namespace Entities;

public class UsersV  
{
    [Key]
    [Display(Name ="User Id")]
    public int? UserId { get; set; }
    [Display(Name = "User Name")]
    public string? UserName { get; set; }
    [Display(Name = "Full Name")]
    public string? FullName { get; set; }
    [Display(Name = "Role")]
    public string? RoleName { get; set; }
    [Display(Name = "Status")]
   
    public string? Status { get; set; }
    [Display(Name = "Country")]
    public string? CountryName { get; set; }
    [Display(Name = "Admin Code")]
    public string? AdminCode { get; set; }
    
    [Display(Name = "Admin Id")]
    public int? AdminId { get; set; }
    [Display(Name = "Admin")]
    public string? AdminUserName { get; set; }
    [Display(Name = "Admin Name")]
    public string? AdminFullName { get; set; }
    public int? PatchNumber { get; set; }
    

}

 
