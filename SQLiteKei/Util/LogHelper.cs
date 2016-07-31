using log4net;

using System.Runtime.CompilerServices;

namespace SQLiteKei.Util
{
    public static class LogHelper
    {
        public static ILog GetLogger([CallerFilePath]string fileName = "")
        {
            return LogManager.GetLogger(fileName);
        }
    }
}
