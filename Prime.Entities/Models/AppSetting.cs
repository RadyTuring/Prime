using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class AppSetting
    {
        public int Id { get; set; }
        [StringLength(3)]
        public string Separator1 { get; set; }
        [StringLength(3)]
        public string Separator2 { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public double  TimeFactor { get; set; }
        [StringLength(3)]
        public string? AdminPrefix { get; set; }
        [StringLength(3)]
        public string? TeacherPrefix { get; set; }
        public int? AppVersion { get; set; }
    }
}
