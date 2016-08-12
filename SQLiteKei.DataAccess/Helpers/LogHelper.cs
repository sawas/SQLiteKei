using log4net;

using System.Runtime.CompilerServices;

namespace SQLiteKei.DataAccess.Helpers
{
    /// <summary>
    /// Class that assists with logging.
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// Gets the logger for the calling class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static ILog GetLogger([CallerFilePath]string fileName = "")
        {
            return LogManager.GetLogger(fileName);
        }
    }
}
