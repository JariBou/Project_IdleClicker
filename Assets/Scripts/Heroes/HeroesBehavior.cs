using System;
using System.Collections;
using System.Xml;
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
        private Rigidbody2D _rb;
        [SerializeField] private float _offset;
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
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
                case ChampionRole.HEALER:
                    _maxHealth = 150f;
                    _damage = 10f;
                    _attackRange = 8f;
                    _attackSpeed = 3;
                    _canAttack = true;
                    _healStrength = 100;
                    _armor = 75;
                    gameObject.tag = "Healer";
                    break;
                case ChampionRole.TANK:
                    _maxHealth = 500;
                    _damage = 30f;
                    _attackRange = 2f;
                    _attackSpeed = 4;
                    _canAttack = true;
                    _healStrength = 0;
                    _armor = 150;
                    gameObject.tag = "Tank";
                    break;
                case ChampionRole.ARCHER:
                    _maxHealth = 100;
                    _damage = 75f;
                    _attackRange = 8f;
                    _attackSpeed = 1;
                    _canAttack = true;
                    _healStrength = 0;
                    _armor = 50;
                    gameObject.tag = "Archer";
                    break;
                case ChampionRole.WARRIOR:
                    _maxHealth = 250;
                    _damage = 75f;
                    _attackRange = 2f;
                    _attackSpeed = 3;
                    _canAttack = true;
                    _healStrength = 0;
                    _armor = 250;
                    gameObject.tag = "Warrior";
                    break;
            }
        }


        private IEnumerator Attack()
        {
            Collider2D[] colliderAttack;
            if (gameObject.tag == "Healer" || gameObject.tag == "Archer")
            {
                colliderAttack = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + _offset, transform.position.y), _attackRange, LayerMask.GetMask("Enemy"));
            }
            else
            {
                colliderAttack = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + _offset, transform.position.y), new Vector2(2, 6), 0, LayerMask.GetMask("Enemy"));
            }
            ;
            foreach (Collider2D collider in colliderAttack)
            {
                _canAttack = false;
                Debug.Log(gameObject.tag + " attack " + collider.gameObject.tag);
                _animator.SetTrigger("Attack1");
                collider.GetComponent<EnemiesBehavior>().TakeDamage(Damage);
                yield return new WaitForSeconds(_attackSpeed);
                _canAttack = true;
            }
        }

        public enum ChampionRole
        {
            HEALER,
            TANK,
            ARCHER,
            WARRIOR
        }

        private void Heal()
        {
            _teamStats.AddHealth(_healStrength);
        }

        public ChampionRole GetRole()
        {
            return _role;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            if (gameObject.tag == "Healer" || gameObject.tag == "Archer")
            {
                Gizmos.DrawWireSphere(new Vector2(transform.position.x + _offset, transform.position.y), _attackRange);
            }
            else
            {
                Gizmos.DrawWireCube(new Vector2(transform.position.x + _offset, transform.position.y), new Vector2(2, 6));
            }
                
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
