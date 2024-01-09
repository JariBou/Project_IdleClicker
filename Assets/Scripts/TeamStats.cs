using System;
using System.Collections.Generic;
using ProjectClicker.Core;
using ProjectClicker.Heroes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectClicker
{
    public class TeamStats : MonoBehaviour
    {
        [FormerlySerializedAs("_baseMaxHealth")]
        [Header("Team Stats")]
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _currentHealth;
        [FormerlySerializedAs("_baseArmor")] [SerializeField] private float _armor;
        
/*        [Foldout("Upgrades"), SerializeField] private Transform _upgradesParent;
        [Foldout("Upgrades"), SerializeField] private GameObject _upgradePrefab;*/

        public static event Action TeamHealthUpdate;
        private bool _isDead;

        [SerializeField] private List<HeroesBehavior> _heroes;


        private GoldManager _goldManager;
        private LevelsManager _managers;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;


        private void Awake()
        {
            _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
            _managers = GameObject.FindWithTag("Managers").GetComponent<LevelsManager>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _maxHealth = GetMaxTeamHealth();
            _currentHealth = _maxHealth;
            _armor = GetTeamArmor();
            _managers.ResetTeamHealth += ResetHealth;
            LevelsManager.OnPrestige += Prestige;

/*          for (int i = 0; i < _heroes.Count; i++)
            {
                _upgradesParent.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    _upgradePrefab.GetComponent<RectTransform>().sizeDelta.x,
                        (_upgradePrefab.GetComponent<RectTransform>().sizeDelta.y));
                Instantiate(_upgradePrefab, _upgradesParent).GetComponent<HeroUpgradeDisplay>().Initialize(i, this);
            }*/

            TeamHealthUpdate?.Invoke();
        }

        private void OnDisable()
        {
            _managers.ResetTeamHealth -= ResetHealth;
            LevelsManager.OnPrestige -= Prestige;
        }

        private void Prestige()
        {
            float tempMaxHealth = 0;
            float tempArmor = 0;
            foreach (HeroesBehavior hero in _heroes)
            {
                tempMaxHealth += hero.BaseMaxHealth;
                tempArmor += hero.BaseArmor;
                hero.ResetLevel();
                hero.LinkedDisplay.UpdateUpgradePanel();
            }

            _maxHealth = tempMaxHealth;
            _armor = tempArmor;
            
            TeamHealthUpdate?.Invoke();
        }

        /*        public void UpgradeHeroAtIndex(int index)
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
*/
        public float GetMaxTeamHealth()
        {
            float tempMaxHealth = 0;
            foreach (HeroesBehavior hero in _heroes)
            {
                tempMaxHealth += hero.MaxHealth;
            }

            return tempMaxHealth;
        }

        public void UpdateStats()
        {
            _maxHealth = GetMaxTeamHealth();
            _armor = GetTeamArmor();
            TeamHealthUpdate?.Invoke();
        }

        public void ResetHealth()  // fonction appel� uniquement par le LevelManager au changement de niveau
        { 
            _currentHealth = MaxHealth;
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
            Debug.Log("Damage taken: " + damage + ", Base Armor: " + _armor);
            float dmg = damage - _armor;
            if (dmg > 0)
            {
                _currentHealth -= dmg;
            }
            TeamHealthUpdate?.Invoke();
            if (_currentHealth < 0 && !_isDead)
            {
                _isDead = true;
                Debug.Log("Dead");
                _managers.PreviousLevel();
                ResetHealth();
            }
        }


        public void AddHealth(float heal)
        {
            _currentHealth += heal;
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            TeamHealthUpdate?.Invoke();
        }

       

    }
}
