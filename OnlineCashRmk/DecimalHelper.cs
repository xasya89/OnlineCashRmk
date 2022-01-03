using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk
{
    public static class DecimalHelper
    {
        public static decimal ToDecimal( this string str)
        {
            decimal dec = -1;
            if (!decimal.TryParse(str.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out dec))
                throw new NotFiniteNumberException("Ошибка преобразования строки в decimal");
            return dec;
        }

        public static double ToDouble(this string str)
        {
            double dec = -1;
            if (!double.TryParse(str.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out dec))
                throw new NotFiniteNumberException("Ошибка преобразования строки в decimal");
            return dec;
        }
    }
}
