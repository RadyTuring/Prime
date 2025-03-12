using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;


public class PracticeQuestionsDto
{
    public Practice Practice { get; set; }
    public IEnumerable<Question> ? Questions { get; set; }
}

