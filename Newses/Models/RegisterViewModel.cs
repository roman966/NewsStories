using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newses.Models
{
    public class RegisterViewModel
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
