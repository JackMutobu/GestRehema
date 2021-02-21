using GestRehema.Helpers;
using System;
using System.Text;

namespace GestRehema.Extensions
{
    public static class PasswordExtensions
    {
        public static string Hash(this string password)
        {
            var passwordHasher = new PasswordHasher(new HashingOptions());
            return passwordHasher.Hash(password);
        }

        public static bool Verify(this string hash, string password)
        {
            var passwordHasher = new PasswordHasher(new HashingOptions());
            return passwordHasher.Check(hash,password).Verified;
        }
        
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }


    }
}
