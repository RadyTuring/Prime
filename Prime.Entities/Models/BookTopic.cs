using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Entities;

public class BookTopic
{
    public long BookTopicId { get; set; }

    public int BookId { get; set; }

    [StringLength(50)]

    public string Topic { get; set; }

    [StringLength(100)]

    public string? TopicDesc { get; set; }
    public string? Keywords { get; set; }
    public int? TopicTypeId { get; set; }

    public int? TopicIdParentId { get; set; }

    public int? TopicOrder { get; set; }

}
