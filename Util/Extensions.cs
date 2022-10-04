using System;

namespace Util
{
    public static class Extensions
    {
        public static string GetValue(this object valor)
        {
            Type type = valor.GetType();

            if (type == typeof(DateTime) && DateTime.TryParse(valor.ToString(), out DateTime dateValue))
            {
                var month = dateValue.Month.ToString().PadLeft(2, '0');
                var day = dateValue.Day.ToString().PadLeft(2, '0');
                var hour = dateValue.Hour.ToString().PadLeft(2, '0');
                var minute = dateValue.Minute.ToString().PadLeft(2, '0');
                var second = dateValue.Second.ToString().PadLeft(2, '0');
                var milisecond = dateValue.Millisecond.ToString().PadLeft(3, '0');

                return $"{dateValue.Year}-{month}-{day} {hour}:{minute}:{second}.{milisecond}";
            }

            return valor.ToString();
        }
    }
}