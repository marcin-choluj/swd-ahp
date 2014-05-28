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

        public static double[] Copy(this double[] data)
        {
            var h = new double[data.Length];
            for (var i = 0; i < data.Length; i++)
            {
                h[i] = data[i];
            }
            return h;
        }

        public static double[,] Copy(this double[,] data)
        {
            var h = new double[data.GetLength(0),data.GetLength(0)];
            for (var i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    h[i, j] = data[i, j];
                }
            }
            return h;
        }
    }
}