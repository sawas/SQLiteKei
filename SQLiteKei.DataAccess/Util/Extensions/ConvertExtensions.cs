using System.ComponentModel;

namespace SQLiteKei.DataAccess.Util.Extensions
{
    /// <summary>
    /// Extensions for generic type conversion.
    /// </summary>
    internal static class ConvertExtensions
    {
        public static TTargetType ConvertTo<TTargetType>(this object source)
        {
            var converter = TypeDescriptor.GetConverter(typeof(TTargetType));
            if (converter != null)
            {
                try
                {
                    return (TTargetType)converter.ConvertFromString(source.ToString());
                }
                catch
                {
                    return default(TTargetType);
                }
            }
            return default(TTargetType);
        }
    }
}
