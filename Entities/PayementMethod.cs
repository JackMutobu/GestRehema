using System.Collections.Generic;

namespace GestRehema.Entities
{
    public class PayementMethod
    {
        public const string Cash = "Cash";

        public const string Bank = "Banque";

        public const string Cheque = "Cheque";

        public const string Wallet = "Compte Virtuel";

        public const string MobileMoney = nameof(MobileMoney);

        public static List<string> Methods
            => new List<string>
            {
                Cash,
                Bank,
                Wallet,
                Cheque,
                MobileMoney
            };
    }
}
