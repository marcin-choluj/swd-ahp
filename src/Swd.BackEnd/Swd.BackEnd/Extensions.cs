using System.Data.Entity.ModelConfiguration.Configuration;
using System.Dynamic;

namespace Swd.BackEnd
{
    public static class Extensions
    {
        public static double GetIndex(this string[] data, string key)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == key)
                    return i + 1;
            }
            return -1;
        }

        public static int GetHighestIndex(this double[] data)
        {
            var h = data[0];
            var index = 0;
            for (var i = 0; i < data.Length; i++)
            {
                if (!(h < data[i])) continue;
                h = data[i];
                index = i;
            }
            return index;
        }
    }
}