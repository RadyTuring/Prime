using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dto;

public  class BookServiceDto
{
    [StringLength(50)]
   
    public string ServiceName { get; set; } 
}
