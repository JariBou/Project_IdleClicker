using System;
using System.Collections.Generic;
using NaughtyAttributes;
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
        
        [Foldout("Upgrades"), SerializeField] private Transform _upgradesParent;
        [Foldout("Upgrades"), SerializeField] private GameObject _upgradePrefab;

        public static event Action TeamHealthUpdate;
        private bool isDead;

        [SerializeField] private List<HeroesBehavior> _heroes;


        private GoldManager _goldManager;

        public float CurrentHealth => _currentHealth;

        private void Awake()
        {
            _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _currentHealth = _baseMaxHealth;
            for (int i = 0; i < _heroes.Count; i++)
            {
                _upgradesParent.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    _upgradePrefab.GetComponent<RectTransform>().sizeDelta.x,
                        (_upgradePrefab.GetComponent<RectTransform>().sizeDelta.y));
                Instantiate(_upgradePrefab, _upgradesParent).GetComponent<HeroUpgradeDisplay>().Initialize(i, this);
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
