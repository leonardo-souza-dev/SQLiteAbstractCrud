using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SQLiteAbstractCrud
{
    [ExcludeFromCodeCoverage]
    public static class Utils
    {
        public static string GetValue(this object valor)
        {
            DateTime? valorDateTime = null;
            try
            {
                valorDateTime = (DateTime)valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error no method GetValue => " + ex.ToString());
            }

            if (valorDateTime.HasValue && DateTime.TryParse(valorDateTime.Value.ToString(), out DateTime dateValue))
            {
                var month = dateValue.Month.ToString().PadLeft(2, '0');
                var dia = dateValue.Day.ToString().PadLeft(2, '0');
                var hour = dateValue.Hour.ToString().PadLeft(2, '0');
                var minute = dateValue.Minute.ToString().PadLeft(2, '0');
                var second = dateValue.Second.ToString().PadLeft(2, '0');
                var milisecond = dateValue.Millisecond.ToString().PadLeft(3, '0');

                return dateValue.Year + "-" + month + "-" + dia + " " + hour + ":" + minute + ":" + second + "." + milisecond;
            }

            return valor?.ToString();
        }
    }
}