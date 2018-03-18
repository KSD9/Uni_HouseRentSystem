using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HouseRent.Common.PasswordHash
{
    public class PasswordHasher
    {
        public string HashPassword(string password)
        {
            //Making a byte array
            byte[] salt;
            //Generating salt
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            //Hashing the given password using the PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

            //Placing the string in a byte array
            byte[] hash = pbkdf2.GetBytes(20);

            //Making new byte array for storing the hashed password+salt
            byte[] hashBytes = new byte[36];

            //Placing the hash and the salt in their places
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            //Convertint the byte array to a string
            string passHash = Convert.ToBase64String(hashBytes);

            return passHash;
        }

        public bool CheckPassword(string enteredPssword, string storedHashedPassword)
        {
            //Extracting the bytes
            byte[] bytes = Convert.FromBase64String(storedHashedPassword);

            //Getting the salt
            byte[] salt = new byte[16];
            Array.Copy(bytes, 0, salt, 0, 16);

            //Hashing the user inputted password with the salt
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPssword, salt, 10000);

            //putting the has input in a byte array
            byte[] hash = pbkdf2.GetBytes(20);

            //Checking if the passwords are the same
            for (int i = 0; i < 20; i++)
            {
                if (bytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }

    }
}
