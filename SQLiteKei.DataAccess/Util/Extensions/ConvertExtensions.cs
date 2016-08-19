using System.ComponentModel;

namespace SQLiteKei.DataAccess.Util.Extensions
{
    /// <summary>
    /// Extensions for generic type conversion.
    /// </summary>
    internal static class ConvertExtensions
    {
        /// <summary>
        /// Tries to convert the source object to the specified target type.
        /// </summary>
        /// <typeparam name="TTargetType">The type of the target type.</typeparam>
        /// <param name="source">The source object.</param>
        /// <returns>Returns the converted object or default(T) if the conversion fails.</returns>
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
