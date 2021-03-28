using System.Collections.Generic;

namespace GestRehema.Entities
{
    public class CustomerType
    {
        public const string Ordinary = "Journalier";

        public const string Organization = "Organization(ONG)";

        public const string OtherStore = "Autre magasin";
    }

    public class SupplierType
    {
        public const string Ordinary = "Ordinaire";
        public const string OtherStore = "Autre magasin";

        public static List<string> GetList()
           => new()
           {
                Ordinary,
                OtherStore
           };
    }
}
