using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace ProjectClicker
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _TextMeshPro;
        [SerializeField] private TeamStats _TeamStats;
        private Slider Slider;

        void Start()
        {
            Slider = GetComponent<Slider>();
            Slider.maxValue = _TeamStats.BaseHealth;
            Slider.minValue = 0;
        }

        private void OnEnable()
        {
            TeamStats.OnTeamDamage += UpdateTeamHealth;
        }
        private void OnDisable()
        {
            TeamStats.OnTeamDamage -= UpdateTeamHealth;
        }

        void UpdateTeamHealth()
        {
            Slider.value = _TeamStats.CurrentHealth;
            _TextMeshPro.text = _TeamStats.CurrentHealth.ToString() + "/" + _TeamStats.BaseHealth.ToString();
        }

    }
}
