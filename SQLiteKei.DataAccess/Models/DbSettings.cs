namespace SQLiteKei.DataAccess.Models
{
    /// <summary>
    /// A model that represents a database's settings.
    /// </summary>
    public class DbSettings
    {
        public short? UserVersion { get; set; }

        public short? SchemaVersion { get; set; }

        public int? ApplicationId { get; set; }

        public int? PageSize { get; set; }

        public int? MaxPageCount { get; set; }

        public int? PageCount { get; set; }

        public int? FreeListCount { get; set; }

        public string JournalMode { get; set; }

        public int? JournalSizeLimit { get; set; }

        public int? CacheSize { get; set; }

        public int? CacheSpill { get; set; }
    }
}
