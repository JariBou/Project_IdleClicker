using System.Globalization;
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
            _TextMeshPro.text = _TeamStats.CurrentHealth.ToString(CultureInfo.InvariantCulture) + "/" + _TeamStats.GetMaxTeamHealth().ToString();
        }
    }
}
