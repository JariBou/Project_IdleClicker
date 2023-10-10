using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectClicker
{
    public class TeamStats : MonoBehaviour
    {
        [Header("Team Stats")]
        [SerializeField] private float _baseHealth;
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _baseArmor;
        [SerializeField] private float _baseDamage;
        [SerializeField] private float _baseHeal;

        public static Action OnTeamDamage;
        private bool isDead;

        [SerializeField] private float _healRate;
        private UpgradesManager _upgradesManager;

        
        private float _attack;

        public float BaseHealth => _baseHealth;
        public float CurrentHealth => _currentHealth;

        private void Awake()
        {
            GetStats();
            _upgradesManager = GameObject.FindWithTag("Managers").GetComponent<UpgradesManager>();
            
        }

        // Start is called before the first frame update
        void Start()
        {
            _currentHealth = _baseHealth;
            _attack = _baseDamage;
            OnTeamDamage.Invoke();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void GetStats()
        {
            foreach(Transform t in transform)
            {
                _baseHealth += t.GetComponent<HeroesBehavior>().MaxHealth;
                _baseArmor += t.GetComponent<HeroesBehavior>().Armor;
                _baseDamage += t.GetComponent<HeroesBehavior>().Damage;
                _baseHeal += t.GetComponent<HeroesBehavior>().PowerHeal;
            }
        }
        
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            OnTeamDamage?.Invoke();
            if (_currentHealth < 0 && !isDead)
            {
                isDead = true;
                Debug.Log("Dead");
            }
        }

        public void AddHealth(float heal) => _currentHealth += heal;

    }
}
