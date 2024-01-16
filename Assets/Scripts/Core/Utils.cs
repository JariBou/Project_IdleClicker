using System.Globalization;

namespace ProjectClicker.Core
{
    public static class Utils
    {
        public static string NumberToString(decimal number)
        {
            if (number <= 1000) return number.ToString(CultureInfo.InvariantCulture);
            string[] suffixes = { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s" };

            int suffixIndex = 0;
            while (number >= 1000 && suffixIndex < suffixes.Length - 1)
            {
                number /= 1000;
                suffixIndex++;
            }
            string formattedMoney = number.ToString("F2");

            return formattedMoney + suffixes[suffixIndex];
        }
    }
}