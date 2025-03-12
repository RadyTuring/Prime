using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;
public class User
{
    [Key]
    public int UserId { get; set; }
    [StringLength(100)]
    public string UserName { get; set; }
    [StringLength(100)]
    public string? AdminCode { get; set; }
    [StringLength(100)]
    public string? FullName { get; set; }
   
    public int? RoleId { get; set; }
    public Role? Role { get; set; }
    [StringLength(150)]
    public string? ImageFile { get; set; }
    public bool IsActiveUser { get; set; }
    [StringLength(60)]
    public string? PasswordHash { get; set; }
    public bool IsOnline { get; set; } = false;
    public bool IsNewUser { get; set; } = true;
    [StringLength(100)]
    //---------------For admin user only-----
    public string? SchoolName { get; set; }
    public int CountryId { get; set; }
    public Country? Country { get; set; }
    public string? Notes { get; set; }
    //---------------------------------------
    public int? PatchNumber { get; set; }
    public int? AdminId { get; set; }
    public DateTime? TranDt { get; set; }

    public int? ThemeId { get; set; } = 1;
    //---------------------------------------
    public string? Books { get; set; }
    public string? Teachers { get; set; }
    public string? Clasess { get; set; }
    public int? BlockLevel { get; set; }
    public int? LogOfflineTimes { get; set; } = 20;
    //---------------------------------------
    public bool? IsRemovedFromSchool { get; set; }

}