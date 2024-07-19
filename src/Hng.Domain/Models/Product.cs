using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hng.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}