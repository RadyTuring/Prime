

using System.ComponentModel.DataAnnotations;

namespace Dto;
public class PubSearchDto
{
    [Display(Name = "Country")]
    public int CountryId { get; set; }
    [Required(ErrorMessage = "Choose The Code Type"), Display(Name = "Code Type")]
    
    public string SearchType { get; set; }
    [Display(Name = "Book")]
    public int? BookId { get; set; }
    [Display(Name = "Search By")]
    public string? SearchBy { get; set; }
}