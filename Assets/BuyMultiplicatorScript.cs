using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectClicker
{
    public class BuyMultiplicatorScript : MonoBehaviour
    {
        
        public static BuyMultiplicatorScript Instance { get; private set; }
        [SerializeField] private TMP_Text _text;
        private List<int> _multiplicators = new(){ 1, 2, 5, 10 };
        private int _multIndex = 0;

        public static event Action<int> UpdatePrice;

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public static int GetMultiplicator() => Instance._multiplicators[Instance._multIndex];

        public void NextMultiplicator()
        {
            _multIndex = (_multIndex + 1) % _multiplicators.Count;
            _text.text = $"x{_multiplicators[_multIndex]}";
            UpdatePrice?.Invoke(_multiplicators[_multIndex]);
        }

        public static void TabChanged()
        {
            UpdatePrice?.Invoke(GetMultiplicator());
        }
    }
}
