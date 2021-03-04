using System.Collections.Generic;

namespace GestRehema.Views
{
    public class BillRecuMotives
    {
        public const string BillPayement = "en règlement de la facture";

        public const string Deposit = "pour dépot d'argent au compte";

        public static List<string> Motives
            => new()
            {
                BillPayement,
                Deposit
            };
    }
}
