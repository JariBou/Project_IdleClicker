using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectClicker
{
    public class TeamStats : MonoBehaviour
    {
        [SerializeField] private int _baseHealth = 10;
        [SerializeField] private int _baseArmor = 3;
        [SerializeField] private int _baseAttack = 5;
        private UpgradesManager _upgradesManager;

        private float _health;
        private float _attack;

        private void Awake()
        {
            _upgradesManager = GameObject.FindWithTag("Managers").GetComponent<UpgradesManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _health = _baseHealth + _upgradesManager.GetLevelOf(UpgradeType.Health);
            _attack = _baseAttack + _upgradesManager.GetLevelOf(UpgradeType.Attack);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetHealth(float health) => _health = health;

        public void AddHealth(float heal) => _health += heal;
    }
}
