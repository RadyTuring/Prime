using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class PracticeQuestionDto
{
    [StringLength(20)]
    public string? SchoolCode { get; set; }
    [StringLength(20)]
    public string? TeacherCode { get; set; }
    public long? PracticeId { get; set; }
    [StringLength(60)]
    public string? QuestionMasterHeader { get; set; }
    [StringLength(50)]
    public string? NumberSection { get; set; }
    public long? QuestionId { get; set; }
    public int? DurationByMin { get; set; }
    public int? QuestionOrder { get; set; }
    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Score { get; set; }
}
