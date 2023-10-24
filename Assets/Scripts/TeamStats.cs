using System;
using System.Collections;
using System.Collections.Generic;
using ProjectClicker.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectClicker
{
    public class TeamStats : MonoBehaviour
    {
        [FormerlySerializedAs("_maxHealth")]
        [FormerlySerializedAs("_baseHealth")]
        [Header("Team Stats")]
        [SerializeField] private float _baseMaxHealth;
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _baseArmor;
        [SerializeField] private float _baseDamage;
        [SerializeField] private float _baseHeal;

        public static event Action TeamHealthUpdate;
        private bool isDead;

        [SerializeField] private float _healRate;

        private List<HeroesBehavior> _heroes;

        
        private float _attack;
        private GoldManager _goldManager;

        public float MaxHealth => _baseMaxHealth;
        public float CurrentHealth => _currentHealth;

        private void Awake()
        {
            _goldManager = GameObject.FindWithTag("GameController").GetComponent<GoldManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _currentHealth = _baseMaxHealth;
            _attack = _baseDamage;
            foreach(Transform t in transform)
            {
                _heroes.Add(t.GetComponent<HeroesBehavior>());
            }
            TeamHealthUpdate?.Invoke();
        }

        public void UpgradeHeroAtIndex(int index)
        {
            HeroesBehavior hero = _heroes[index];
            if (_goldManager.gold <= (ulong)hero.GetUpgradeCost())
            {
                return;
            }
            
            _goldManager.RemoveGold(hero.GetUpgradeCost());
            AddHealth(hero.Upgrade());
            TeamHealthUpdate?.Invoke();
        }

        public float GetMaxTeamHealth()
        {
            float tempMaxHealth = _baseMaxHealth;
            foreach (HeroesBehavior hero in _heroes)
            {
                tempMaxHealth += hero.MaxHealth;
            }

            return tempMaxHealth;
        }
        
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            TeamHealthUpdate?.Invoke();
            if (_currentHealth < 0 && !isDead)
            {
                isDead = true;
                Debug.Log("Dead");
            }
        }

        public void AddHealth(float heal) => _currentHealth += heal;

    }
}
