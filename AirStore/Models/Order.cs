using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirStore.Models
{
    public class Order
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public int AirId { get; set; }
        public Air Air { get; set; }

        public bool Shipped { get; set; }
        public bool Delivered { get; set; }
    }
}
