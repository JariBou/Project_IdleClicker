using System.Collections;
using System.Collections.Generic;
using ProjectClicker.Core;
using UnityEngine;

namespace ProjectClicker
{
    public class PrestigeButtonScript : MonoBehaviour
    {
        [SerializeField] private GameObject _button;
        private bool _enabled = false;
        // Start is called before the first frame update
        void Awake()
        {
            _button.SetActive(false);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_enabled) return;
            if (PrestigeManager.Instance.CalculateScore() > 10)
            {
                _button.SetActive(true);
                _enabled = true;
            }
        }

        public void DoPrestige()
        {
            LevelsManager.Instance.PrestigeGame();
            _button.SetActive(false);
            _enabled = false;
        }
    }
}
