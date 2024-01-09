using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ProjectClicker.Core;

namespace ProjectClicker
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _TextMeshPro;
        [SerializeField] private TeamStats _TeamStats;
        private Slider Slider;
        private GoldManager _goldManager;

        void Start()
        {
            Slider = GetComponent<Slider>();
            _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
        }

        private void OnEnable()
        {
            TeamStats.TeamHealthUpdate += OnTeamHealthUpdate; 
        }

        private void OnDisable()
        {
            TeamStats.TeamHealthUpdate -= OnTeamHealthUpdate;
        }

        private void OnTeamHealthUpdate()
        {
            Slider.maxValue = _TeamStats.GetMaxTeamHealth();
            Slider.value = _TeamStats.CurrentHealth; /*/ _TeamStats.GetMaxTeamHealth();*/
            _TextMeshPro.text = _goldManager.NumberToString((decimal)_TeamStats.CurrentHealth) + "/" + _goldManager.NumberToString((decimal)_TeamStats.GetMaxTeamHealth());
        }
    }
}
