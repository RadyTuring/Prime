using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dto;

public class BookTopicDto
{

    public int BookId { get; set; }

    [StringLength(50)]
   
    public string Topic { get; set; } 

    [StringLength(100)]
   
    public string? TopicDesc { get; set; }

    public int TopicTypeId { get; set; }

    public int TopicIdParent { get; set; }

    public int? TopicOrder { get; set; }


   
}
