using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dto;
public  class CountryDto
{
   

    [StringLength(50)]
    public string? CountryName { get; set; }

    [StringLength(2)]
   
    public string? Code2 { get; set; }

    [StringLength(10)]
   
    public string? Code3 { get; set; }

    [StringLength(10)]
   
    public string? Code { get; set; }

}
