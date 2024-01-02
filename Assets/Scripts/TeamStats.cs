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
        [SerializeField] private float _baseArmor;
        
/*        [Foldout("Upgrades"), SerializeField] private Transform _upgradesParent;
        [Foldout("Upgrades"), SerializeField] private GameObject _upgradePrefab;*/

        public static event Action TeamHealthUpdate;
        private bool isDead;

        [SerializeField] private List<HeroesBehavior> _heroes;


        private GoldManager _goldManager;
        private LevelsManager _managers;

        public float CurrentHealth => _currentHealth;

        private void Awake()
        {
            _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
            _managers = GameObject.FindWithTag("Managers").GetComponent<LevelsManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _baseMaxHealth = GetMaxTeamHealth();
            _currentHealth = _baseMaxHealth;
            _baseArmor = GetTeamArmor();
            _managers.ResetTeamHealth += ResetHealth;

/*          for (int i = 0; i < _heroes.Count; i++)
            {
                _upgradesParent.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    _upgradePrefab.GetComponent<RectTransform>().sizeDelta.x,
                        (_upgradePrefab.GetComponent<RectTransform>().sizeDelta.y));
                Instantiate(_upgradePrefab, _upgradesParent).GetComponent<HeroUpgradeDisplay>().Initialize(i, this);
            }*/

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
            float tempMaxHealth = 0;
            foreach (HeroesBehavior hero in _heroes)
            {
                tempMaxHealth += hero.MaxHealth;
            }

            return tempMaxHealth;
        }

        public void ResetHealth()
        { 
            _currentHealth = _baseMaxHealth;
            TeamHealthUpdate?.Invoke();
            
        }
            

        public float GetTeamArmor()
        {
            float tempArmor = 0;
            foreach (HeroesBehavior hero in _heroes)
            {
                tempArmor += hero.Armor;
            }

            return tempArmor;
        }
        
        public void TakeDamage(float damage)
        {
            float Damage = damage - _baseArmor;
            if (Damage > 0)
            {
                _currentHealth -= Damage;
            }
            TeamHealthUpdate?.Invoke();
            if (_currentHealth < 0 && !isDead)
            {
                isDead = true;
                Debug.Log("Dead");
            }
        }


        public void AddHealth(float heal)
        {
            _currentHealth += heal;
            if (_currentHealth > _baseMaxHealth)
            {
                _currentHealth = _baseMaxHealth;
            }
            TeamHealthUpdate?.Invoke();
        }

       

    }
}
