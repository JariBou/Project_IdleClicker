using System;
using System.Collections;
using ProjectClicker.Core;

namespace ProjectClicker.Saves
{
    [Serializable]
    public class ChampionLevelItem
    {
        public ChampionRole championRole;
        public int championLevel;
    }
    
    
    [Serializable]
    public class ChampionLevelDictionary : IEnumerable
    {
        public ChampionLevelItem[] items;
        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}