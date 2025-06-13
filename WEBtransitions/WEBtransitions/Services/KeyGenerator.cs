using System.Text;
using WEBtransitions.Services.Interfaces;

namespace WEBtransitions.Services
{
    /// <summary>
    /// <see cref="https://www.geeksforgeeks.org/c-sharp/c-sharp-randomly-generating-strings/"/>
    /// </summary>
    public class KeyGenerator : IKeyGenerator
    {
        private string Characters = string.Empty;

        public string GenerateKey(int keyLength, byte flags)
        {
            Characters = GenerateTemplate(flags);
            var builder = new StringBuilder();
            Random rand = new Random((new DateTime()).Millisecond);
            while (builder.Length < keyLength)
            {
                int i = rand.Next(1, Characters.Length) - 1;
                builder.Append(Characters[i]);
            }
            return builder.ToString();
        }

        private string GenerateTemplate(byte flags)
        {
            var builder = new StringBuilder();
            if ((flags & 0x1) != 0)
            {
                builder.Append("QWERTYUIOPASDFGHJKLZXCVBNM");
            }
            if ((flags & 0x2) != 0)
            {
                builder.Append("qwertyuiopasdfghjklzxcvbnm");
            }
            if ((flags & 0x4) != 0)
            {
                builder.Append("0123456789");
            }
            return builder.ToString();
        }
    }
}
