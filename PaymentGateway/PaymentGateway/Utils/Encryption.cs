using System;
namespace PaymentGateway.Controllers
{
    public class Encryption
    {
        private static String encryptionKey = "gateway2020";

        public Encryption()
        {
        }

        //Encryption using TripleDES method
        public static string EncryptTripleDES(string Plaintext)
        {

            System.Security.Cryptography.TripleDESCryptoServiceProvider DES = new System.Security.Cryptography.TripleDESCryptoServiceProvider();

            System.Security.Cryptography.MD5CryptoServiceProvider hashMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            DES.Key = hashMD5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(encryptionKey));

            DES.Mode = System.Security.Cryptography.CipherMode.ECB;

            System.Security.Cryptography.ICryptoTransform DESEncrypt = DES.CreateEncryptor();

            byte[] Buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(Plaintext);

            string TripleDES = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));

            return TripleDES;

        }

        //Decryption using TripleDES method
        public static string DecryptTripleDES(string base64Text)
        {

            System.Security.Cryptography.TripleDESCryptoServiceProvider DES = new System.Security.Cryptography.TripleDESCryptoServiceProvider();

            System.Security.Cryptography.MD5CryptoServiceProvider hashMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            DES.Key = hashMD5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(encryptionKey));

            DES.Mode = System.Security.Cryptography.CipherMode.ECB;

            System.Security.Cryptography.ICryptoTransform DESDecrypt = DES.CreateDecryptor();

            byte[] Buffer = Convert.FromBase64String(base64Text);

            string DecTripleDES = System.Text.ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));

            return DecTripleDES;

        }
    }
}
