using System;

namespace GestRehema.Entities
{
    public class Store : IBaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
