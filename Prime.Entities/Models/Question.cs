using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class Question
{
    [Key]
    public long QuestionId { get; set; }
    public int  TeacherId { get; set; }
    public int PracticeCode { get; set; }
    public int? BookId { get; set; }
    public int QuestionTypeId { get; set; }
    [StringLength(300)]
    public string? ParentQuestionTitle { get; set; }
    [StringLength(300)]
    public string QuestionTitle { get; set; }
    public string? QuestionDesc { get; set; }
    [StringLength(400)]
    public string QuestionChoices { get; set; }
    [StringLength(300)]
    public string ModelAnswer { get; set; }
    [StringLength(300)]
    public string? QuestionMedia { get; set; }
    [StringLength(300)]
    public string? AnswerMedia { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public double Score { get; set; }
   
    public int OrderNo { get; set; }
   
    public bool IsShared { get; set; }
    public bool IsManyChoicesAnswer { get; set; }
    public int DurationByMin { get; set; }

    public int? Dificulty { get; set; }
    public int? TopicId { get; set; }
    public string? Keywords { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? TranDt { get; set; }=DateTime.Now;
}
