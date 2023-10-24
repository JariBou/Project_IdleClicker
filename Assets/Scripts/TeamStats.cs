using System;
using System.Collections.Generic;
using ProjectClicker.Core;
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

        public static event Action TeamHealthUpdate;
        private bool isDead;

        private List<HeroesBehavior> _heroes;


        private GoldManager _goldManager;

        public float CurrentHealth => _currentHealth;

        private void Awake()
        {
            _goldManager = GameObject.FindWithTag("GameController").GetComponent<GoldManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _currentHealth = _baseMaxHealth;
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
