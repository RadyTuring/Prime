
using System.ComponentModel.DataAnnotations;

namespace Dto;
public class CodeSearchDto
{
    [Required(ErrorMessage = "Choose The Code Type"), Display(Name = "Code Type")]
    public string PatchType { get; set; }
    [Display(Name = "Book")]
    public int? BookId { get; set; }
    [Display(Name = "Date From")]
    public DateTime? DateFrom { get; set; }
    [Display(Name = "Date To")]
    public DateTime? DateTo { get; set; }
    public string? PatchDesc { get; set; }
    [Display(Name = "Created By")]
    public string? UserName { get; set; }
}