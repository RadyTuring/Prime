using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;
public class LogV
{
    [Display(Name = "Id")]
    public int? LogvId { get; set; }
    [Display(Name = "User Id")]
    public int? UserId { get; set; }
 
    [Display(Name = "User Name")]
    public string UserName { get; set; }
    [Display(Name = "Full Name")]
    public string? FullName { get; set; }
    [Display(Name = "Action")]
    public string? LogAction { get; set; }
    [Display(Name = "Date")]
    public DateTime? TranDate { get; set; }
    [Display(Name = "Admin Id")]
    public int? AdminId { get; set; }
}
 