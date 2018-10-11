using System;
using System.Security.Cryptography;

namespace BAC.Crypto
{
    public class CryptoAes
    {
        private AesCryptoServiceProvider AESDes = new AesCryptoServiceProvider();

        public CryptoAes(string key)
        {
            // Initialize the crypto provider.
            AESDes.Key = TruncateHash(key, AESDes.KeySize / 8);
            AESDes.IV = TruncateHash("", AESDes.BlockSize / 8);
        }

        private byte[] TruncateHash(string key, int length)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            // Hash the key.
            byte[] keyBytes = System.Text.Encoding.Unicode.GetBytes(key);
            byte[] hash = sha1.ComputeHash(keyBytes);

            // Truncate or pad the hash.
            Array.Resize(ref hash, length);
            return hash;
        }

        public string Decrypt(string encryptedtext)
        {
            ICryptoTransform decryptor = AESDes.CreateDecryptor(AESDes.Key, AESDes.IV);
            string plaintext = null;
            byte[] encryptedtextBytes = Convert.FromBase64String(encryptedtext);

            // Create the streams used for decryption. 
            using (System.IO.MemoryStream msDecrypt = new System.IO.MemoryStream(encryptedtextBytes))
            {
                using (CryptoStream csDecrypt =
                        new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (System.IO.StreamReader srDecrypt = new System.IO.StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream 
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }

            return plaintext;
        }

        public string Encrypt(string plaintext)
        {
            ICryptoTransform encryptor = AESDes.CreateEncryptor(AESDes.Key, AESDes.IV);
            byte[] encrypted;

            // Create the streams used for encryption. 
            using (System.IO.MemoryStream msEncrypt = new System.IO.MemoryStream())
            {
                using (CryptoStream csEncrypt =
                        new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (System.IO.StreamWriter swEncrypt = new System.IO.StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plaintext);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            return Convert.ToBase64String(encrypted);
        }
    }
}