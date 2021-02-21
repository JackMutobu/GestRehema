using System;

namespace GestRehema.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string Role { get; set; } = null!;

        public string? AccessLevel { get; set; }

        public Employee? Employee { get; set; }
    }
}
