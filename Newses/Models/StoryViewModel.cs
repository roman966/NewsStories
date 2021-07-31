using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newses.Models
{
    public class StoryViewModel
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public DateTime date { get; set; }

        [Required]
        public string Author { get; set; }
    }
}
