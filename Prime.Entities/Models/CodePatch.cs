using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class CodePatch
{
    public int Id { get; set; }
    public int? BookId { get; set; }
    [ Display(Name = "Patch Description")]
    public string PatchDesc { get; set; }
    [Display(Name = "Date")]
    public DateTime PatchDate { get; set; }=DateTime.Now;
    [Display(Name = "Type")]
    public string PatchType { get; set; }
    [Display(Name = "Created By")]
    public string UserName { get; set; }
    [Display(Name = "Number of Codes")]
    public int NumberOfCodes { get; set; }
    public Book? Book { get; set; }
    public bool? IsOneAssinged { get; set; }
    [Display(Name = "Print")]
    public bool IsPrinted { get; set; }=false;
    [Display(Name = "Inv-Location")]
    public string? InventoryLocation { get; set; }
    public string? Notes { get; set; }
}

