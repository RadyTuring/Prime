using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class PracticeDto
{
    public int PracticeTypeId { get; set; }
    public int BookId { get; set; }
    public bool? IsShared { get; set; }
    [StringLength(100)]
    public string? PracticeTitle { get; set; }
    public int? DurationByMin { get; set; }
}
public class PracticeWithGloablDto
{
    public int PracticeTypeId { get; set; }
    public int BookId { get; set; }
    public bool? IsShared { get; set; }
    public bool IsGlobal { get; set; }
    [StringLength(100)]
    public string? PracticeTitle { get; set; }
    public int? DurationByMin { get; set; }
}
