using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class BookPrintDto
{
    [Required(ErrorMessage = "Choose The Book"), Display(Name = "Book")]
    public int BookId { get; set; }
    [Required(ErrorMessage = "Choose The Book services"), Display(Name = "Book Services")]
    public List<string>? BookServices { get; set; }
    [ Display(Name = "Include Ekit")]
    public bool IncludeEkit { get; set; }
    [Required(ErrorMessage = "Enter the Patch Name"), Display(Name = "Patch Name")]
    public string? PatchDesc { get; set; }
    public string? PatchType { get; set; }
    [Required(ErrorMessage = "Enter the number of Codes"), Display(Name = "Number Of Codes"),Range(1, 100000, ErrorMessage = "The Number Between 1 and 100000")]
    public int NumberOfCodes { get; set; }
}
