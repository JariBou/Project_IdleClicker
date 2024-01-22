using System;
using System.Collections;
using System.Collections.Generic;
using ProjectClicker.Core;
using TMPro;
using UnityEngine;

namespace ProjectClicker
{
    public class TimePassedGUI : MonoBehaviour
    {
        public static TimePassedGUI Instance { get; private set; }
        
        [SerializeField] private TMP_Text _goldAmount;
        [SerializeField] private TMP_Text _timeAway;

        private void Awake()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ButtonClick()
        {
            gameObject.SetActive(false);
        }

        public void Config(long goldValue, double timeSpanTotalMinutes)
        {
            _goldAmount.text = $"+{Utils.NumberToString(goldValue)}";
            _timeAway.text = $"{Utils.NumberToString((decimal)Math.Round(timeSpanTotalMinutes, 2))}minutes";
        }
    }
}
