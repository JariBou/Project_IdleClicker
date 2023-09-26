using System;
using UnityEngine;

namespace Core
{
    public class GoldManager : MonoBehaviour
    {
        public static GoldManager instance { get; private set; }
        public ulong gold { get; private set; } = 0;

        private void Awake()
        {
            instance = this;
        }

        public static void AddGold(int amount)
        {
            instance.gold += (ulong)amount;
        }

        public static void AddGold(long amount)
        {
            instance.gold += (ulong)amount;
        }
        
        public static void AddGold(ulong amount)
        {
            instance.gold += amount;
        }
        
        public static void RemoveGold(int amount)
        {
            instance.gold -= (ulong)amount;
        }

        public static void RemoveGold(long amount)
        {
            instance.gold -= (ulong)amount;
        }
        
        public static void RemoveGold(ulong amount)
        {
            instance.gold -= amount;
        }

        public static String GoldString()
        {
            int pow = instance.gold.ToString().Length-1;
            String goldString = instance.gold.ToString();
            if (pow < 4)
            {
                return goldString;
            }
            return $"{goldString[0]},{goldString.Substring(1, 2)}e{pow}";
        }
    }
}
