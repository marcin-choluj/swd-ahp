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
                    return i+1;
            }
            return -1;
        }
    }
}