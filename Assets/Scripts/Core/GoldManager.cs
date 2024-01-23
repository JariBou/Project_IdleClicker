using System;
using System.Globalization;
using NaughtyAttributes;
using UnityEngine;

namespace ProjectClicker.Core
{
    public class GoldManager : MonoBehaviour
    {
        public ulong Gold { get; private set; } = 0;
        
        public static GoldManager Instance { get; private set; }
        
        #if UNITY_EDITOR
        [SerializeField] private ulong _goldToAdd;
        [Button]
        public void AddGold()
        {
            AddGold(_goldToAdd);
        }
        #endif

        private void Awake()
        {
            Instance = this;
        }


        public void AddGold(int amount)
        {
            Gold += (ulong)amount;
        }

        public void AddGold(long amount)
        {
            Gold += (ulong)amount;
        }
        
        public void AddGold(ulong amount)
        {
            Gold += amount;
        }
        
        public void RemoveGold(int amount)
        {
            Gold -= (ulong)amount;
        }

        public void RemoveGold(long amount)
        {
            Gold -= (ulong)amount;
        }
        
        public void RemoveGold(ulong amount)
        {
            Gold -= amount;
        }

        public string GoldString()
        {
            if (Gold < 1000) return Gold.ToString();
            decimal goldMoney = Gold;
            string[] suffixes = { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s"}; 

            int suffixIndex = 0;
            while (goldMoney >= 1000 && suffixIndex < suffixes.Length - 1)
            {
                goldMoney /= 1000;
                suffixIndex++;
            }
            return goldMoney.ToString("F2") + suffixes[suffixIndex];
        }
       

        public void SetGold(ulong saveDataGold)
        {
            Gold = saveDataGold;
        }
    }
}
