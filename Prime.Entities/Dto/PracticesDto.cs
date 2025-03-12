using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class PracticesDto
{
    public PracticeDto Practice { get; set; }
    public QuestionDto[]  Questions { get; set; }
}
public class PracticesWithGlobalDto
{
    public PracticeWithGloablDto Practice { get; set; }
    public QuestionDto[] Questions { get; set; }
}
