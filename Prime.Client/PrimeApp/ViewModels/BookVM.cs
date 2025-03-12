using CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace ViewModels;

public class BookVM
{
    [Required(ErrorMessage = "Please Enter the Book Name"), StringLength(60), Display(Name = "Book Name")]
    public string? BookName { get; set; }
    [Display(Name = "PPK File")]
    public string? PPKFile { get; set; }
    [Display(Name = "Book Image")]
    public string? DefaultImage { get; set; }
    [Required(ErrorMessage = "Please enter the version"), Display(Name = "PPK Version")]
    public int? PPkVersion { get; set; }
    [RequiredIfNotZero(ErrorMessage = "Please Choose the Update Level"), Display(Name = "Update Level")]
    public int UpdateLevelId { get; set; }
    [Display(Name = "Games File")]
    public string? GamesFileName { get; set; }
    [Display(Name = "Games Image")]
    public string? GamesImage { get; set; }
    [Required(ErrorMessage = "Please Enter the Book Prefix"), StringLength(3,ErrorMessage ="The Prefix should be 3 Characters"), MinLength(3, ErrorMessage = "The Prefix should be 3 Characters"),Display(Name = "Book Prefix")]
    public string Prefix { get; set; }
    [StringLength(50), DataType(DataType.Url, ErrorMessage = "Invalid Url")]
    public string? LinkUrl { get; set; }
    [StringLength(100)]
    public string? Notes { get; set; }
}
