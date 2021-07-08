using BlogApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class EditHistory
    {   
        [Key]
        public int Id { get; set; }

        public DateTime UpdatedOn { get; set; }
        public ApplicationUser Author { get; set; }

        public ApplicationUser user { get; set; }
    }
}
