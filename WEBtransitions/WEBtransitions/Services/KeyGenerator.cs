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

        /// <summary>
        /// Generates random string
        /// </summary>
        /// <param name="keyLength">Length of the string</param>
        /// <param name="flags">One byte containing 3 bits: <code>0000 0xyz</code></param>
        /// <type="bullet">
        ///     <item>
        ///         <term><code>x == 1></code></term>
        ///         <description>The string contains numbers</description>
        ///     </item>
        ///     <item>
        ///         <term><code>y == 1></code></term>
        ///         <description>The string contains lower-case characters</description>
        ///     </item>
        ///     <item>
        ///         <term><code>z == 1></code></term>
        ///         <description>The string contains upper-case characters</description>
        ///     </item>
        /// </type>
        /// <returns></returns>
        public string GenerateKey(int keyLength, byte flags)
        {
            Characters = GenerateTemplate(flags);
            var builder = new StringBuilder();

            DateTime currentDate = DateTime.Now;
            DateTime centuryBegin = new(2001, 1, 1);
            long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;
            int seed = elapsedTicks.GetHashCode();

            Random rand = new(seed);
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
