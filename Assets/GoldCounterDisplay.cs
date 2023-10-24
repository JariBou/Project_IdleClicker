using ProjectClicker.Core;
using TMPro;
using UnityEngine;

namespace ProjectClicker
{
    public class GoldCounterDisplay : MonoBehaviour
    {
        private GoldManager _goldManager;
        private TMP_Text _display;

        private void Awake()
        {
            _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
            _display = GetComponent<TMP_Text>();
        }

        private void FixedUpdate()
        {
            /*_display.text = $"{_goldManager.GoldString()} [GOLD]";*/
            _display.text = _goldManager.GoldString();
        }
    }
}
