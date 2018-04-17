using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirStore.Models
{
    public class Air
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [EnumDataType(typeof(AirItemType))]
        [Required]
        public AirItemType Type { get; set; }

        [Required]
        public decimal Price { get; set; }

        public ICollection<Order> Orders { get; set; }
    }

    public enum AirItemType
    {
        Continental = 1,
        Maritime = 2,
        Arctic = 3,
        Tropical = 4,
        Polar = 5
    }
}
