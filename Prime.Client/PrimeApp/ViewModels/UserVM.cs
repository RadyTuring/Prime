using System.ComponentModel.DataAnnotations;
namespace ViewModels;
public class UserVM
{
   [Required(ErrorMessage = "enter User Name"), StringLength(60),  Display(Name = "User Name")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "enter Your Full Name"), StringLength(60), Display(Name = "Full Name")]
    public string FullName { get; set; }
    [Display(Name = "Profile Picture")]
    public IFormFile? ImageFile { get; set; }
    [Required(ErrorMessage = "Choose the country"),  Display(Name = "Country")]
    public int CountryId { get; set; }
    [Required(ErrorMessage = "Choose the time zone"),  Display(Name = "Time Zone")]
    public int TimeZoneId { get; set; }
    [Required(ErrorMessage = "enter Password."), DataType(DataType.Password)]
    public string? Password { get; set; }
    [Required(ErrorMessage = "enter Confirm Password."), DataType(DataType.Password),Compare("Password",ErrorMessage ="unmatched password"),Display(Name = "Confirm Password")]
    public string? ConfirmPassword { get; set; }
}