using System;
using NaughtyAttributes;
using UnityEngine;

namespace ProjectClicker.Core
{
    public class GoldManager : MonoBehaviour
    {
        public ulong gold { get; private set; } = 0;
        
        #if UNITY_EDITOR
        [SerializeField] private ulong _goldToAdd;
        [Button]
        public void AddGold()
        {
            AddGold(_goldToAdd);
        }
        #endif


        public void AddGold(int amount)
        {
            gold += (ulong)amount;
        }

        public void AddGold(long amount)
        {
            gold += (ulong)amount;
        }
        
        public void AddGold(ulong amount)
        {
            gold += amount;
        }
        
        public void RemoveGold(int amount)
        {
            gold -= (ulong)amount;
        }

        public void RemoveGold(long amount)
        {
            gold -= (ulong)amount;
        }
        
        public void RemoveGold(ulong amount)
        {
            gold -= amount;
        }

        public String GoldString()
        {
            int pow = gold.ToString().Length-1;
            String goldString = gold.ToString();
            if (pow < 4)
            {
                return goldString;
            }
            return $"{goldString[0]},{goldString.Substring(1, 2)}e{pow}";
        }
    }
}
