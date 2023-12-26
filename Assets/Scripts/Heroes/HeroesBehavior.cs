using System;
using System.Collections;
using ProjectClicker.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectClicker
{
    public class HeroesBehavior : MonoBehaviour
    {
        [FormerlySerializedAs("Role")] [SerializeField] private ChampionRole _role;
        private ChampionRole _previousRole;
        [SerializeField] private UpgradeInfo _upgradeInfo; 
        [SerializeField] private int _heroLevel; 

        [FormerlySerializedAs("teamStats")]
        [Header("Team Manager")]
        [SerializeField]
        private TeamStats _teamStats;

        [FormerlySerializedAs("maxHealth")]
        [Header("Health")]
        [SerializeField] private float _maxHealth;

        [FormerlySerializedAs("damage")]
        [Header("Attack")]
        [SerializeField]
        private float _damage;
        [FormerlySerializedAs("attackRange")] [SerializeField] private float _attackRange;
        [FormerlySerializedAs("attackSpeed")] [SerializeField] private float _attackSpeed;
        [FormerlySerializedAs("layerToHit")] [SerializeField] private LayerMask _layerToHit;
        private bool _canAttack;


        [FormerlySerializedAs("healStrength")]
        [FormerlySerializedAs("healStrenght")]
        [Header("Heal")]
        [SerializeField]
        private float _healStrength = 100;

        [FormerlySerializedAs("armor")]
        [Header("Armor")]
        [SerializeField]
        private float _armor;


        public float MaxHealth => _maxHealth + _heroLevel * _upgradeInfo.HealthPerLevel;
        public float Damage => _damage + _heroLevel * _upgradeInfo.DmgPerLevel;
        public float AttackSpeed => _attackSpeed + _heroLevel * _upgradeInfo.AtkSpeedPerLevel;
        public float PowerHeal => _healStrength + _heroLevel * _upgradeInfo.HealStrengthPerLevel;
        public float Armor => _armor + _heroLevel * _upgradeInfo.ArmorPerLevel;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }


        // Update is called once per frame
        private void Update()
        {
            if (_canAttack)
            {
                StartCoroutine(Attack());
            }
        }

        private void SetState(ChampionRole role)
        {
            switch (role)
            {
                    _damage = 10f;
                case championRole.HEALER:
                    maxHealth = 150f;
                    damage = 10f;
                    attackRange = 8f;
                    attackSpeed = 3;
                    canAttack = true;
                    healStrenght = 100;
                    armor = 75;
                    gameObject.tag = "Healer";
                    
                    break;
                case championRole.TANK:
                    maxHealth = 500;
                    damage = 30f;
                    attackRange = 2f;
                    attackSpeed = 4;
                    canAttack = true;
                    healStrenght = 0;
                    armor = 150;
                    gameObject.tag = "Tank";
                    break;
                case championRole.ARCHER:
                    maxHealth = 100;
                    damage = 75f;
                    attackRange = 8f;
                    attackSpeed = 1;
                    canAttack = true;
                    healStrenght = 0;
                    armor = 50;
                    gameObject.tag = "Archer";
                    break;
                case championRole.WARRIOR:
                    maxHealth = 250;
                    damage = 75f;
                    attackRange = 2f;
                    attackSpeed = 3;
                    canAttack = true;
                    healStrenght = 0;
                    armor = 250;
                    gameObject.tag = "Warrior";
                    break;
            }
        }


        private IEnumerator Attack()
        {
            Collider2D[] colliderAttack = Physics2D.OverlapCircleAll(transform.position, _attackRange, _layerToHit);
            foreach (Collider2D collider in colliderAttack)
            {
                _canAttack = false;
                _animator.SetTrigger("Attack1");
                collider.GetComponent<EnemiesBehavior>().TakeDamage(Damage);
                yield return new WaitForSeconds(_attackSpeed);
                _canAttack = true;
            }
        }

        private void Heal()
        {
            _teamStats.AddHealth(_healStrength);
        }

        public championRole GetRole()
        {
            return Role;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }

        private void OnValidate()
        {
            if (_previousRole != _role)
            {
                _previousRole = _role;
                SetState(_role);
            }
        }

        /// <summary>
        /// Returns the health gained to heal
        /// </summary>
        /// <returns></returns>
        public float Upgrade()
        {
            _heroLevel++;
            return _upgradeInfo.HealthPerLevel;
        }

        public int GetUpgradeCost()
        {
            return (int)(_upgradeInfo.BaseCost * UpgradeCostMultiplier());
        }

        private float UpgradeCostMultiplier()
        {
            return 1 / 175f * _heroLevel * _heroLevel + 1;
        }
    }
}
