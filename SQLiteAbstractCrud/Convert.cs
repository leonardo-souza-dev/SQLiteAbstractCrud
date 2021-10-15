using System;
using System.Globalization;

namespace SQLiteAbstractCrud
{
    public static class Convert
    {
        public static string SqliteDate(DateTime? data)
        {
            if (data.HasValue)
                return data.Value.ToString("s", CultureInfo.CreateSpecificCulture("pt-BR")).Replace('T', ' ') + ".000";

            return "";
        }
    }
}