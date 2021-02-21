namespace GestRehema.Entities
{
    public class Person
    {
        public string Nom { get; set; } = null!;

        public string Postnom { get; set; } = null!;

        public string Prenom { get; set; } = null!;

        public string Fullname => $"{Nom} {Postnom} {Prenom}";

        public string? Adresse { get; set; }

        public string NumTelephone { get; set; } = null!;

        public string? Email { get; set; } 

        public string? ProfileUrl { get; set; }
    }
}
