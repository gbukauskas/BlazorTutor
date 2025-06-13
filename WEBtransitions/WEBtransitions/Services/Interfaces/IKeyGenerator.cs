namespace WEBtransitions.Services.Interfaces
{
    public interface IKeyGenerator
    {
        /// <summary>
        /// Generates random string.
        /// </summary>
        /// <param name="keyLength">Numger of characters in the key</param>
        /// <param name="flags">Collection of bits that defines content of the key: 
        ///     bit 0 == 1 - include capital letters,
        ///     bit 1 == 1 - include lower case letters,
        ///     bit 2 == 1 - include numbers.
        /// <returns></returns>
        string GenerateKey(int keyLength, byte flags);
    }
}
