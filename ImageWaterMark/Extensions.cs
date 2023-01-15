using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWaterMark
{
    public static class Extensions
    {
        public static IEnumerable<int> GetEnumVals<T>() where T : Enum
        {
            List<int> result = new List<int>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                result.Add((int)item);
            }

            return result;
        }
    }
}
