using System;
using ProjectClicker.Core;
using TMPro;
using UnityEngine;

namespace ProjectClicker
{
    public class CounterDisplay : MonoBehaviour
    {
        [SerializeField] private UpgradeResource _upgradeResource;
        private PrestigeManager _prestigeManager;
        private GoldManager _goldManager;
        private TMP_Text _display;

        private void Awake()
        {
            _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
            _prestigeManager = GameObject.FindWithTag("Managers").GetComponent<PrestigeManager>();
            _display = GetComponent<TMP_Text>();
        }

        private void FixedUpdate()
        {
            /*_display.text = $"{_goldManager.GoldString()} [GOLD]";*/
            _display.text = _upgradeResource switch
            {
                UpgradeResource.GoldUpgrade => _goldManager.GoldString(),
                UpgradeResource.PrestigeUpgrade => Utils.NumberToString(_prestigeManager.Medals),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
