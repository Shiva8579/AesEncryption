using System.Security.Cryptography;
namespace AesEncryption
{
    public static class InfoSec
    {
        //Code to generate the AES key
        public static string GenerateKey()
        {
            string keyBase64 = "";
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();

                keyBase64 = Convert.ToBase64String(aes.Key);

            }
            return keyBase64;
        }

        public static string Encrypt(string plainText, string key, out string IVKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Padding = PaddingMode.Zeros;
                aes.Key = Convert.FromBase64String(key);
                aes.GenerateIV();

                IVKey = Convert.ToBase64String(aes.IV);

                ICryptoTransform encryption = aes.CreateEncryptor();

                byte[] encryteData;
                using (MemoryStream ms = new MemoryStream())
                {
                    using(CryptoStream cs = new CryptoStream(ms,encryption,CryptoStreamMode.Write))
                    {
                        using(StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encryteData = ms.ToArray();
                    }
                }
                return Convert.ToBase64String(encryteData);
            }
        }
        public static string Decrypt(string CipherText, string key, string IVKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Padding = PaddingMode.Zeros;
                aes.Key = Convert.FromBase64String(key);
                aes.IV = Convert.FromBase64String(IVKey);


                ICryptoTransform encryption = aes.CreateDecryptor();

                string PlainText = "";
                byte[] ciper = Convert.FromBase64String(CipherText);

                using (MemoryStream ms = new MemoryStream(ciper))
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryption, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            PlainText = sr.ReadToEnd();
                        }
                    }
                }
                return PlainText.ToString().TrimEnd(new char[] {'\0'});
            }
        }
    }
}
