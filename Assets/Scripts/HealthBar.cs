using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ProjectClicker.Core;
using UnityEngine.Serialization;

namespace ProjectClicker
{
    public class HealthBar : MonoBehaviour
    {
        [FormerlySerializedAs("_TextMeshPro")] [SerializeField] private TextMeshProUGUI _textMeshPro;
        [FormerlySerializedAs("_TeamStats")] [SerializeField] private TeamStats _teamStats;
        private Slider _slider;
        private GoldManager _goldManager;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
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
            _slider.maxValue = _teamStats.GetMaxTeamHealth();
            _slider.value = _teamStats.CurrentHealth; /*/ _TeamStats.GetMaxTeamHealth();*/
            _textMeshPro.text = Utils.NumberToString((decimal)_teamStats.CurrentHealth) + "/" + Utils.NumberToString((decimal)_teamStats.GetMaxTeamHealth());
        }
    }
}
