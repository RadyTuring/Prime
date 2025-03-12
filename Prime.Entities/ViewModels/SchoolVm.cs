using Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  ViewModels
{
    public class SchoolVm
    {
       
        public long Recid { get; set; }

        [Display(Name ="School Code")]
        public string? SchoolCode { get; set; }

        [Display(Name = "School")]
        public string? SchoolName { get; set; }
       
        public  Country Country { get; set; }
    }
}
