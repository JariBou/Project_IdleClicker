using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ProjectClicker
{
    public class ChampionUpgradeUI : MonoBehaviour
    {

        [SerializeField] private GameObject _upgradePanelPrefab;
        [SerializeField] private GameObject _team;

        // Start is called before the first frame update
        void Start()
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
            var panel = Instantiate(_upgradePanelPrefab, Vector3.zero, Quaternion.identity);
            panel.transform.SetParent(transform);
            panel.GetComponent<HeroUpgradeDisplay>().Initialize(index, championRole);
        }

    }
}
