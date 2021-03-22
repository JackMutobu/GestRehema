using System;

namespace GestRehema.Entities
{
    public class SupplyExpense: IBaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Description { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Category { get; set; } = null!;

        public decimal Amount { get; set; }

        public string Owner { get; set; } = null!;

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int PayementId { get; set; }
        public Payement? Payement { get; set; }

        public int SupplyId { get; set; }
        public Supply? Supply { get; set; }
    }
}
