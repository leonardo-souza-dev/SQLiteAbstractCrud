using System.Globalization;
using System;

namespace SQLiteAbstractCrud.Util
{
    public static class Convert
    {
        public static string SqliteDate(DateTime? data)
        {
            if (data.HasValue)
            {
                var milliseconds = data.Value.Millisecond.ToString().PadLeft(3, '0');
                return data.Value.ToString("s", CultureInfo.CreateSpecificCulture("pt-BR")).Replace('T', ' ') + $".{milliseconds}";
            }

            return "";
        }
    }
}