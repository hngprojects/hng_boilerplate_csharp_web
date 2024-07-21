using System.ComponentModel.DataAnnotations;

namespace Hng.Domain.Entities
{
    public class SubscriptionPlan : EntityBase
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public ICollection<Feature> Features { get; set; }












        //  public List<Feature> Features { get; set; }
        //public ICollection<User> Users { get; set; } = new List<User>();
    }

    //public class Feature : EntityBase
    //{
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //}

}

