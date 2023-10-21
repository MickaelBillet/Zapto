using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core
{
    public static class ZaptoMath
    {        
        public static (double?, DateTime) Median(IEnumerable<(double? temp, DateTime date)> values)
        {
            var tab = values.OrderBy((x) => x.temp).ToArray<(double? , DateTime)>();  

            int n = tab.Length;
            int mid = n / 2;

            return tab[mid];
        }
    }
}
