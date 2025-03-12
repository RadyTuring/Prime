using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    [Keyless]
    public class PagesV
    {
        public int RoleId { get; set; }
        public int Id { get; set; }
        [StringLength(50)]
        public string  PgTitle { get; set; }
        [StringLength(50)]
        public string? PgImage { get; set; }
        [StringLength(50)]
        public string? PgHref { get; set; }
        [StringLength(50)]
        public int? PgParentId { get; set; }
        public int? PgORder { get; set; }
    }
}
