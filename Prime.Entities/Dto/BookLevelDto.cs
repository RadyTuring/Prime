using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dto;

public  class BookLevelDto
{

    [StringLength(50)]
   
    public string? LevelName { get; set; }
}
