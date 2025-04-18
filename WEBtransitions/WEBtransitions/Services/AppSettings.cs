namespace WEBtransitions.Services
{
    /// <summary>
    /// Section "AppSettings" in appsettings.json file
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Value of "ActiveCulture" key ("lt-LT" in my example)
        /// </summary>
        public string? ActiveCulture { get; set; }
        public string? DefaultCulture { get; set; }
    }
}
