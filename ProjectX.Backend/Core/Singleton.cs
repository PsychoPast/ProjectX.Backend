namespace ProjectX.Backend.Core
{
    /// <summary>
    /// The Singleton instance of the application.
    /// </summary>
    public sealed class Singleton
    {
        /// <summary>
        /// The Database instance.
        /// </summary>
        public DatabaseManager Database = new("projectx", "lfOnBgIdsIdCjp8p");

        private static Singleton _instance;

        public static Singleton Instance => _instance ??= new Singleton();
    }
}