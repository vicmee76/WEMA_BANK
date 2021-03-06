using System;
using System.Text;

namespace WEMA_BANK.Helpers
{
    public class HelperClass
    {
        public string Encrypt(string clearText)
        {
            try
            {
                byte[] b = ASCIIEncoding.ASCII.GetBytes(clearText);
                string crypt = Convert.ToBase64String(b);
                byte[] c = ASCIIEncoding.ASCII.GetBytes(crypt);
                string encrypt = Convert.ToBase64String(c);

                return encrypt;
            }
            catch (Exception)
            {
                return "Error";
            }
        }



        public string Decrypt(string cipherText)
        {
            try
            {
                byte[] b;
                byte[] c;
                string decrypt;
                b = Convert.FromBase64String(cipherText);
                string crypt = ASCIIEncoding.ASCII.GetString(b);
                c = Convert.FromBase64String(crypt);
                decrypt = ASCIIEncoding.ASCII.GetString(c);

                return decrypt;
            }
            catch (Exception)
            {
                return "Error";
            }

        }


        public string OTP()
        {
            Random random = new Random();
           var str = random.Next(0, 100000000);
            return str.ToString();
        }
    }
}
