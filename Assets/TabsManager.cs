using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectClicker
{
    public class TabsManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _tabs;
        [SerializeField] private List<GameObject> _counters;

        private void Awake()
        {
            SetActiveTab(0);
        }

        public void SetActiveTab(int index)
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].SetActive(false);
                _counters[i].SetActive(false);
            }
            _tabs[index].SetActive(true);
            _counters[index].SetActive(true);
            try
            {
                BuyMultiplicatorScript.TabChanged();
            }
            catch (Exception e)
            {
                // Means it's on awake
            }
        }
    }
}
