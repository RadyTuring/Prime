using System.ComponentModel.DataAnnotations;

namespace JqDtDisplay;

public class AdminLogColsDt
{
    
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
    
}
