using System.Globalization;
using UnityEngine;

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

        public static Vector2 ConvertPosCamToCam(Vector2 pos, Vector2 displayPos, Camera from, Camera to)
        {
            Vector2 mainCameraMousePosition = from.ScreenToWorldPoint(pos);
            Vector2 difference = mainCameraMousePosition - displayPos;

            float ratioX = (to.orthographicSize * 2f * to.aspect)/(from.orthographicSize * 2f * from.aspect);
            float ratioY = to.orthographicSize/from.orthographicSize *2f;
            difference = new Vector2(difference.x * ratioX, difference.y * ratioY);
            Vector2 newMousePosition = new Vector2(to.transform.position.x, to.transform.position.y) + difference;
            return newMousePosition;
        }
    }
}