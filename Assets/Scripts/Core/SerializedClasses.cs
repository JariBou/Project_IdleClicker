using System;
using UnityEngine;

namespace ProjectClicker.Core
{
    [Serializable]
    public class UpgradeInfo
    {
        [SerializeField] private float _baseCost = 10;
        [SerializeField] private float _dmgPerLevel = 1;
        [SerializeField] private float _atkSpeedPerLevel = 0.1f;
        [SerializeField] private float _armorPerLevel = 1;
        [SerializeField] private float _healthPerLevel = 10;
        [SerializeField] private float _healStrengthPerLevel = 2;

        public float HealthPerLevel => _healthPerLevel;

        public float DmgPerLevel => _dmgPerLevel;

        public float AtkSpeedPerLevel => _atkSpeedPerLevel;

        public float ArmorPerLevel => _armorPerLevel;

        public float HealStrengthPerLevel => _healStrengthPerLevel;

        public float BaseCost => _baseCost;
    }
}
