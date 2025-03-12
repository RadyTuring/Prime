using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Entities;

namespace Dto;

public class QuestionDto
{
    public int QuestionTypeId { get; set; }
    public string? ParentQuestionTitle { get; set; }
    public string QuestionTitle { get; set; }
    public string QuestionChoices { get; set; }
    public string ModelAnswer { get; set; }
    public double Score { get; set; }
    //public int? Dificulty { get; set; }
    public bool IsManyChoicesAnswer { get; set; }
    public int OrderNo { get; set; }
    public int DurationByMin { get; set; }
    public List<IFormFile>? QuestionFiles { get; set; }
    public List<IFormFile>? AnswerFiles { get; set; }
}
