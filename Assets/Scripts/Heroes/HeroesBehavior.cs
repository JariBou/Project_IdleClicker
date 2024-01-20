using System.Collections;
using System.Linq;
using NaughtyAttributes;
using System.Collections.Generic;
using ProjectClicker.Core;
using ProjectClicker.Enemies;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectClicker.Heroes
{
    public class HeroesBehavior : MonoBehaviour
    {
        [SerializeField] private ChampionRole _role;
        private ChampionRole _previousRole;
        [SerializeField] private UpgradeInfo _upgradeInfo; 
        [SerializeField] private int _heroLevel; 
        public int HeroLevel => _heroLevel;
        [SerializeField] private int _prestigeLevel = 0; 
        public int PrestigeLevel => _prestigeLevel;
        private LevelsManager _levelsManager;

        public List<HeroUpgradeDisplay> LinkedDisplays { get; set; }
        
        [Header("Team Manager")]
        [SerializeField]
        private TeamStats _teamStats;
        
        [Header("Health")]
        [SerializeField] private float _baseMaxHealth;
        
        [Header("Attack")]
        [SerializeField]
        private float _baseDamage;
        [SerializeField] private float _attackRange;
        [SerializeField] private float _baseAttackSpeed;
        [SerializeField] private LayerMask _layerToHit;
        [SerializeField] private bool _canAttack;


        [Header("Heal")]
        [SerializeField]
        private float _baseHealStrength = 100;
        private bool _canHeal = true;

        [Header("Armor")]
        [SerializeField]
        private float _baseArmor;

        [Header("Arrow")]
        [ShowIf("_role", ChampionRole.Archer)]
        [SerializeField] private Transform _arrowSpawnPoint;
        [ShowIf("_role", ChampionRole.Archer)]
        [SerializeField] private GameObject _arrowPrefab;
        [FormerlySerializedAs("_attckCount")] [SerializeField] private int _attackCount = 0;
        

        public float MaxHealth => BaseMaxHealth + (_heroLevel + _prestigeLevel) * Info.HealthPerLevel;
        public float Damage => BaseDamage + (_heroLevel + _prestigeLevel) * Info.DmgPerLevel;
        public float AttackSpeed => _baseAttackSpeed + (_heroLevel + _prestigeLevel) * Info.AtkSpeedPerLevel;
        public float PowerHeal => BaseHealStrength + (_heroLevel + _prestigeLevel) * Info.HealStrengthPerLevel;
        public float Armor => BaseArmor + (_heroLevel + _prestigeLevel) * Info.ArmorPerLevel;

        public float BaseMaxHealth => _baseMaxHealth;
        public float BaseArmor => _baseArmor;

        public UpgradeInfo Info => _upgradeInfo;

        public float BaseDamage => _baseDamage;

        public float BaseHealStrength => _baseHealStrength;


        private Animator _animator;
        private Rigidbody2D _rb;
        private Collider2D[] colliderAttack;

        [Header("Attack Offset")]
        [SerializeField] private float _offset;
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            LinkedDisplays = new List<HeroUpgradeDisplay>();
            /*Debug.Log(gameObject.name);*/
            _levelsManager = GameObject.FindWithTag("Managers").GetComponent<LevelsManager>();  
            LevelsManager.OnChangeLevel += OnChangingLevel;
        }


        // Update is called once per frame
        private void Update()
        {
            
            if (gameObject.CompareTag("Healer") || gameObject.CompareTag("Archer"))
            {
                colliderAttack = Physics2D.OverlapCircleAll(
                    new Vector2(transform.position.x + _offset, transform.position.y), _attackRange,
                    LayerMask.GetMask("Enemy"));
            }
            else
            {
                colliderAttack =
                    Physics2D.OverlapBoxAll(new Vector2(transform.position.x + _offset, transform.position.y),
                        new Vector2(2, 10), 0, LayerMask.GetMask("Enemy"));
            }



            if (_teamStats.CurrentHealth < _teamStats.MaxHealth/2 && _canHeal && _role == ChampionRole.Healer)
            {
                _canHeal = false;
                _animator.SetTrigger("Heal");
                StartCoroutine(Heal());
            }
            if (_role == ChampionRole.Healer)
            {
                if (_canAttack && colliderAttack.Length != 0 && _teamStats.CurrentHealth > _teamStats.MaxHealth / 2)
                {
                    Attack(colliderAttack);
                }
            }
            else
            {
                if (_canAttack && colliderAttack.Length != 0)
                {
                    Attack(colliderAttack);
                }
            }
            
        }

        private void SetState(ChampionRole role)
        {
            switch (role)
            {
                case ChampionRole.Healer:
                    _baseMaxHealth = 150f;
                    _baseDamage = 10f;
                    _attackRange = 8f;
                    _baseAttackSpeed = 3;
                    _canAttack = true;
                    _baseHealStrength = 100;
                    _baseArmor = 5;
                    gameObject.tag = "Healer";
                    break;
                case ChampionRole.Tank:
                    _baseMaxHealth = 500;
                    _baseDamage = 30f;
                    _attackRange = 2f;
                    _baseAttackSpeed = 4;
                    _canAttack = true;
                    _baseHealStrength = 0;
                    _baseArmor = 75;
                    gameObject.tag = "Tank";
                    break;
                case ChampionRole.Archer:
                    _baseMaxHealth = 100;
                    _baseDamage = 75f;
                    _attackRange = 8f;
                    _baseAttackSpeed = 2;
                    _canAttack = true;
                    _baseHealStrength = 0;
                    _baseArmor = 10;
                    gameObject.tag = "Archer";
                    break;
                case ChampionRole.Warrior:
                    _baseMaxHealth = 250;
                    _baseDamage = 75f;
                    _attackRange = 2f;
                    _baseAttackSpeed = 3;
                    _canAttack = true;
                    _baseHealStrength = 0;
                    _baseArmor = 25;
                    gameObject.tag = "Warrior";
                    break;
            }
        }


        private void Attack(Collider2D[] colliderAttack)
        {
            Debug.Log("J'attaque");
            _canAttack = false;
            foreach (Collider2D collider in colliderAttack.Where(i => i != null))
            {
                /*                if (collider == null) continue;*/
/*                _animator.ResetTrigger("Attack1");
                _animator.ResetTrigger("Attack2");
                _animator.ResetTrigger("Attack3");*/
                if (_rb.velocity.x < 0.01f && !collider.CompareTag("EnemyBase"))
                {
                    if (_attackCount == 0) _animator.SetTrigger("Attack1");
                    else if (_attackCount == 1 || _attackCount == 3) _animator.SetTrigger("Attack2");
                    else if (_attackCount == 2) _animator.SetTrigger("Attack3");
                }
                else
                {
                    _animator.SetTrigger("Attack1");

                }
                break;
            }
        }
        public void AttackMelee()
        {
            _canAttack = false;
            foreach (Collider2D collider in colliderAttack.Where(i => i != null))
            {
                Debug.Log(gameObject.tag + " attack " + collider.gameObject.tag);
                if (collider.CompareTag("EnemyBase"))
                {
                    if (_attackCount > 0) collider.GetComponent<EnemyBase>()?.TakeDamage(Damage * _attackCount);
                    else collider.GetComponent<EnemyBase>()?.TakeDamage(Damage);
                    _attackCount = 0;
                    return;
                }
                else
                {
                    collider.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage);
                    break;
                }
            }
            /*if ((_attckCount == 0 && _heroLevel >= 12) || (_attckCount == 1 && _heroLevel >= 25))*/ _attackCount++;
            if ((_attackCount > 0 && _heroLevel < 12) || (_attackCount > 1 && /*_heroLevel >= 12 &&*/ _heroLevel < 25) || _attackCount > 2) _attackCount = 0;
/*            _animator.ResetTrigger("Attack1");
            _animator.ResetTrigger("Attack2");
            _animator.ResetTrigger("Attack3");*/
        }

        public void AttackRange() // je dois l'appeller dans l'animator
        {
            _canAttack = false;
            Arrow projectile;
            if (_attackCount == 1)
            {
                projectile = Instantiate(_arrowPrefab, _arrowSpawnPoint.transform.position, Quaternion.identity).GetComponent<Arrow>();
                projectile._arrowType = (ArrowType)Random.Range(0, 2);
            }
            else if (_attackCount == 2)
            {
                if (colliderAttack.Length == 0) return;
                projectile = Instantiate(_arrowPrefab, new Vector2(colliderAttack[0].transform.position.x, colliderAttack[0].transform.position.y -1), Quaternion.identity).GetComponent<Arrow>();
                projectile._arrowType = (ArrowType)3;
            }
            else if (_attackCount == 3)
            {
                projectile = Instantiate(_arrowPrefab, new Vector2(_arrowSpawnPoint.transform.position.x + 4.5f, _arrowPrefab.transform.position.y - 2.2f), Quaternion.identity).GetComponent<Arrow>();
                projectile._arrowType = (ArrowType)4;
            }
            /*if ((_attckCount == 1 && _heroLevel >= 12 && _heroLevel < 18) || (_attckCount == 2 && _heroLevel >= 18  && _heroLevel > 25) || (_attckCount == 3 && _heroLevel >= 25))*/ _attackCount++;
            if ((_attackCount > 0 && _heroLevel < 12) || (_attackCount > 1 && _heroLevel < 18) || (_attackCount > 2 && _heroLevel < 25) || _attackCount > 3 ) _attackCount = 0;
/*            _animator.ResetTrigger("Attack1");
            _animator.ResetTrigger("Attack2");
            _animator.ResetTrigger("Attack3");*/
        }
        Coroutine _coroutine;
        public void Coroutine()
        {

            if (_canAttack || _coroutine != null) return;
            _coroutine = StartCoroutine(AttackCooldown());
        }
        private IEnumerator AttackCooldown()
        {
/*            if (_canAttack) yield break;*/
            yield return new WaitForSeconds(_baseAttackSpeed);
            _canAttack = true;
            _coroutine = null;
        }

        private IEnumerator Heal()
        {
            _teamStats.AddHealth(BaseHealStrength);
            yield return new WaitForSeconds(_baseAttackSpeed);
            _canHeal = true;
        }

        public ChampionRole GetRole()
        {
            return _role;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            if (gameObject.CompareTag("Healer") || gameObject.CompareTag("Archer"))
            {
                Gizmos.DrawWireSphere(new Vector2(transform.position.x + _offset, transform.position.y), _attackRange);
            }
            else
            {
                Gizmos.DrawWireCube(new Vector2(transform.position.x + _offset, transform.position.y), new Vector2(2, 10));
            }
                
        }

        public void OnChangingLevel()
        {
            _animator.SetTrigger("ChangingLevel");
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
        public void Upgrade()
        {
            _heroLevel++;
/*            _damage += _upgradeInfo.DmgPerLevel * _heroLevel;
            _maxHealth += _upgradeInfo.HealthPerLevel * _heroLevel;
            _armor += _upgradeInfo.ArmorPerLevel * _heroLevel;
            _healStrength += _upgradeInfo.HealStrengthPerLevel * _heroLevel;
            if (_attackSpeed > _upgradeInfo.MinAtkSpeed)  _attackSpeed -= _upgradeInfo.AtkSpeedPerLevel * _heroLevel;*/
            _teamStats.UpdateStats();
        }

        

        public int GetUpgradeCost()
        {
            return (int)(Info.BaseCost * UpgradeCostMultiplier());
        }

        private float UpgradeCostMultiplier()
        {
            return 1 / 175f * _heroLevel * _heroLevel + 1;
        }

        private void OnDestroy() // Peut être inutile vu qu'on ne détruit pas les héros mais au cas où on les détruit un jour
        {
            LevelsManager.OnChangeLevel -= OnChangingLevel;
        }

        public void ResetLevel()
        {
            _heroLevel = 0;
        }

        public void SetLevel(int championLevel)
        {
            _heroLevel = championLevel;
        }

        public void PrestigeUpgrade()
        {
            _prestigeLevel++;
        }
    }
}
