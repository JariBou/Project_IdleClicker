using ProjectClicker.Core;
using UnityEngine;

namespace ProjectClicker.UI
{
    public class ChampionUpgradeUI : MonoBehaviour
    {

        [SerializeField] private GameObject _upgradePanelPrefab;
        [SerializeField] private GameObject _upgradePanelPrefabHealer;
        [SerializeField] private GameObject _team;
        [SerializeField] private UpgradeResource _upgradeResource;

        // Start is called before the first frame update
        private void Start()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < _team.transform.childCount; i++)
            {
                if (_team.transform.GetChild(i).gameObject.activeSelf)
                {
                    AddChampionPanel(i);
                }
            }
        }

        public void AddChampionPanel(int index)
        {
            string championRole = _team.transform.GetChild(index).gameObject.tag;
            GameObject panel;
            if (championRole == "Healer") panel = Instantiate(_upgradePanelPrefabHealer, Vector3.zero, Quaternion.identity);         
            else panel = Instantiate(_upgradePanelPrefab, Vector3.zero, Quaternion.identity);
            panel.transform.SetParent(transform);
            panel.GetComponent<HeroUpgradeDisplay>().Initialize(index, championRole, _upgradeResource);
        }

    }
}
