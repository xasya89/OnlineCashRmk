using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Extenisions;

public static class DigitValidationExtension
{
    public static bool IsDigitsOnly(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty) return false; // или true, в зависимости от логики

        foreach (char c in input)
        {
            if (c < '0' || c > '9')
                return false;
        }
        return true;
    }
}
