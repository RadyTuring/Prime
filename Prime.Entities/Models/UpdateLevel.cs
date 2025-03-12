using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class UpdateLevel
{
    [Key]
    public int UpdateLevelId { get; set; }

    [StringLength(50)]
   
    public string? UpdateLevelName { get; set; }

  
}
