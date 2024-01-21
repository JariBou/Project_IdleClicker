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

        public static ulong Power(decimal n, decimal p)
        {
            ulong total = (ulong)n;
            for (int i = 0; i < p; i++)
            {
                total *= (ulong)n;
            }

            return total;
        }

        public static bool IsMouseOverGameObject(Vector2 MousePosition, GameObject Object)
        {
            if (Object.GetComponent<CapsuleCollider2D>() != null)
            {
                   if (MousePosition.x >= Object.transform.position.x - Object.GetComponent<CapsuleCollider2D>().bounds.size.x / 2 &&
                                       MousePosition.x <= Object.transform.position.x + Object.GetComponent<CapsuleCollider2D>().bounds.size.x / 2 &&
                                                          MousePosition.y >= Object.transform.position.y - Object.GetComponent<CapsuleCollider2D>().bounds.size.y / 2 &&
                                                                             MousePosition.y <= Object.transform.position.y + Object.GetComponent<CapsuleCollider2D>().bounds.size.y / 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (Object.GetComponent<CircleCollider2D>() != null)
            {
                if (MousePosition.x >= Object.transform.position.x - Object.GetComponent<CircleCollider2D>().bounds.size.x / 2 &&
                                       MousePosition.x <= Object.transform.position.x + Object.GetComponent<CircleCollider2D>().bounds.size.x / 2 &&
                                                          MousePosition.y >= Object.transform.position.y - Object.GetComponent<CircleCollider2D>().bounds.size.y / 2 &&
                                                                             MousePosition.y <= Object.transform.position.y + Object.GetComponent<CircleCollider2D>().bounds.size.y / 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}