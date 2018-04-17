using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirStore.Models
{
    public class Comment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Text { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
