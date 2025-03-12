using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class Country
{
    [Key]
    public int CountryId { get; set; }

    [StringLength(300)]
    public string? CountryNameUtc { get; set; }

    [StringLength(50)]
    public string? CountryName { get; set; }

    [StringLength(50)]
    public string? Code2 { get; set; }

    [StringLength(50)]
    public string? Code3 { get; set; }

    [StringLength(50)]
    public string? Code { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public Double TimeDif { get; set; }
    public int? TbTimeZoneId { get; set; }

    public TbTimeZone? TbTimeZone { get; set; }
}
