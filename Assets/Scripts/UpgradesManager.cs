using System.Collections.Generic;
using UnityEngine;

namespace ProjectClicker
{
    // TODO: Shit wont work, need a SO of characters with amount contributed and have functions on said SO and here
    // TODO: we can get the amount contributed of all characters to a certain Upgrade
    public class UpgradesManager : MonoBehaviour
    {
        private Dictionary<UpgradeType, int> _upgradeLevelDic;
        
        // Start is called before the first frame update
        void Start()
        {
/*            UpgradeType[] playerUpgradesArray = (UpgradeType[])Enum.GetValues(typeof(UpgradeType));
            foreach (UpgradeType upgradeType in playerUpgradesArray)
            {
                _upgradeLevelDic.Add(upgradeType, 0);
            }*/
        }

        public int GetLevelOf(UpgradeType upgradeType)
        {
            return _upgradeLevelDic[upgradeType];
        }
    }

    public enum UpgradeType
    {
        Attack,
        Health,
        Armor,
        AttackSpeed
    }
}