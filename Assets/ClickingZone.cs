using System;
using System.Collections;
using System.Collections.Generic;
using ProjectClicker.Core;
using UnityEngine;

namespace ProjectClicker
{
    public class ClickingZone : MonoBehaviour
    {
        private GoldManager _goldManager;

        private void Awake()
        {
            _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
        }

        public void OnClick()
        {
            _goldManager.AddGold(1);
        }
    }
}
