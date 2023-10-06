using System.Collections.Generic;
using UnityEngine;

namespace ProjectClicker
{
    public class TabsManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _tabs;

        private void Awake()
        {
            SetActiveTab(0);
        }

        public void SetActiveTab(int index)
        {
            foreach (GameObject tab in _tabs)
            {
                tab.SetActive(false);
            }
            _tabs[index].SetActive(true);
        }
    }
}
