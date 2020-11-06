using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthMicroservice.DAL.Infrastructure.PasswordHasher
{
    public static class PasswordHasher
    {
        public static PasswordSalt Hash(string password)
        {
            var salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            var result = new PasswordSalt
            {
                Hash = hashed,
                Salt = Convert.ToBase64String(salt)
            };

            return result;
        }

        public static bool Check(string salt, string userHash, string password)
        {
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                 password: password,
                 salt: Convert.FromBase64String(salt),
                 prf: KeyDerivationPrf.HMACSHA1,
                 iterationCount: 10000,
                 numBytesRequested: 256 / 8));

            var isHashCorrect = hash.Equals(userHash);

            return isHashCorrect;
        }

        public static string ActivationHash()
        {
            var salt = new byte[128 / 8];
            byte[] hash;

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (SHA512 shaM = new SHA512Managed())
            {
                hash = shaM.ComputeHash(salt);
            }

            var result = Convert.ToBase64String(hash);

            return result;
        }
    }
}
