using System.Text;
using System.Security.Cryptography;

public class EncDec
{
    private static string CipherKey = "verySecretKey";

    public static void Main()
    {
        string text = "HEY supp";

        Console.WriteLine("ENCRYPTION:");
        Console.Write(text + "==>");
        string encryptedText = Encrypt(text);
        Console.WriteLine(encryptedText);

        Console.WriteLine("\n\nDECRYPTION:");
        Console.Write(encryptedText + "==>");
        Console.WriteLine(Decrypt(encryptedText));

    }
    public static string Encrypt(string plainText)
    {
        string encryptedText = Encrypt(plainText, CipherKey);
        return Uri.EscapeDataString(encryptedText);
    }

    public static string Decrypt(string plainText)
    {
        if (!string.IsNullOrEmpty(plainText))
        {
            plainText = Uri.UnescapeDataString(plainText);

            string dt = Decrypt(plainText, CipherKey);
            return dt;
        }
        else
        {
            return plainText;
        }
    }

    public static string Encrypt(string clearText, string cipherKey)
    {
        string encryptionKey = cipherKey;
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
     }
    public static string Decrypt(string cipherText, string cipherKey)
    {
        string encryptionKey = cipherKey;
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                return Encoding.Unicode.GetString(ms.ToArray());
            }
        }
    }

}
