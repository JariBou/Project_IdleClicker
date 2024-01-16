using System.Collections;
using System.Collections.Generic;
using ProjectClicker.Core;
using ProjectClicker.Enemies;
using UnityEngine;

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
        [SerializeField] private Transform _arrowSpawnPoint;
        [SerializeField] private GameObject _arrowPrefab;
        private int _attckCount = 0;

 


        public float MaxHealth => BaseMaxHealth + (_heroLevel + _prestigeLevel) * _upgradeInfo.HealthPerLevel;
        public float Damage => _baseDamage + (_heroLevel + _prestigeLevel) * _upgradeInfo.DmgPerLevel;
        public float AttackSpeed => _baseAttackSpeed + (_heroLevel + _prestigeLevel) * _upgradeInfo.AtkSpeedPerLevel;
        public float PowerHeal => _baseHealStrength + (_heroLevel + _prestigeLevel) * _upgradeInfo.HealStrengthPerLevel;
        public float Armor => BaseArmor + (_heroLevel + _prestigeLevel) * _upgradeInfo.ArmorPerLevel;

        public float BaseMaxHealth => _baseMaxHealth;
        public float BaseArmor => _baseArmor;


        private Animator _animator;
        private Rigidbody2D _rb;

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
            Collider2D[] colliderAttack;
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
                        new Vector2(2, 6), 0, LayerMask.GetMask("Enemy"));
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
                    StartCoroutine(Attack(colliderAttack));
                }
            }
            else
            {
                if (_canAttack && colliderAttack.Length != 0)
                {
                    StartCoroutine(Attack(colliderAttack));
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


        private IEnumerator Attack(Collider2D[] colliderAttack)
        {
            foreach (Collider2D collider in colliderAttack)
            {
                if (collider == null) continue;
                if (_rb.velocity.x < 0.01f)
                {
                    if (!collider.CompareTag("EnemyBase"))
                    {
                        if (!collider.gameObject.GetComponent<EnemiesBehavior>().IsDead)
                        {
                            if (_role == ChampionRole.Archer)
                            {
                                if (_attckCount == 0)
                                {
                                    _canAttack = false;

                                    _animator.SetTrigger("Attack1");
                                    yield return new WaitForSeconds(0.1f);
                                    Debug.Log(gameObject.tag + " attack " + collider.gameObject.tag);
                                    collider.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage);
                                    yield return new WaitForSeconds(_baseAttackSpeed + 0.5f);
                                    
                                    if (_heroLevel >= 10) _attckCount++;
                                    /*_attckCount++;*/
                                    _canAttack = true;
                                }
                                else if (_attckCount == 1)
                                {
                                    _canAttack = false;
                                    _animator.SetTrigger("Attack2");
                                    yield return new WaitForSeconds(1.358f);
                                    GameObject arrow = Instantiate(_arrowPrefab, _arrowSpawnPoint.position, Quaternion.identity);
                                    arrow.GetComponent<Arrow>()._arrowType = (ArrowType)Random.Range(0, 2);
                                    yield return new WaitForSeconds(_baseAttackSpeed + 0.5f);
                                    if (_heroLevel >= 15) _attckCount++;
                                    else _attckCount = 0;
                                    /*_attckCount++;*/
                                    _canAttack = true;
                                }
                                else if (_attckCount == 2)
                                {
                                    _canAttack = false;
                                    _animator.SetTrigger("Attack3");
                                    yield return new WaitForSeconds(1f);
                                    GameObject arrow = Instantiate(_arrowPrefab, new Vector2(collider.transform.position.x, collider.transform.position.y - 2), Quaternion.identity);
                                    arrow.GetComponent<Arrow>()._arrowType = ArrowType.Shower;
                                    yield return new WaitForSeconds(_baseAttackSpeed + 0.5f);
                                    _canAttack = true;
                                    if (_heroLevel >= 25) _attckCount++;
                                    else _attckCount = 0;
                                    /*_attckCount++;*/
                                }
                                else 
                                {
                                    _canAttack = false;
                                    _animator.SetTrigger("Attack2");
                                    yield return new WaitForSeconds(1f);
                                    GameObject arrow = Instantiate(_arrowPrefab, new Vector2(_arrowSpawnPoint.position.x + 5f, _arrowSpawnPoint.position.y), Quaternion.identity);
                                    arrow.GetComponent<Arrow>()._arrowType = ArrowType.Beam;
                                    yield return new WaitForSeconds(_baseAttackSpeed + 0.5f);
                                    _canAttack = true;
                                    _attckCount = 0;
                                }

                            }
                            else
                            {
                                if (_attckCount <= 1)
                                {
                                    _canAttack = false;

                                    _animator.SetTrigger("Attack1");
                                    yield return new WaitForSeconds(0.1f);

                                    Debug.Log(gameObject.tag + " attack " + collider.gameObject.tag);
                                    collider.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage);

                                    yield return new WaitForSeconds(_baseAttackSpeed);
                                    _canAttack = true;
                                    if (_heroLevel >= 10) _attckCount++;
                                    /*_attckCount++;*/
                                }
                                else if (_attckCount == 2)
                                {
                                    _canAttack = false;

                                    _animator.SetTrigger("Attack2");
                                    yield return new WaitForSeconds(1f);
                                    collider.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage*2);
                                    yield return new WaitForSeconds(_baseAttackSpeed);
                                    _canAttack = true;
                                    if (_heroLevel >= 25) _attckCount++;
                                    else _attckCount = 0;
                                    /*_attckCount++;*/
                                }
                                else
                                {
                                    _canAttack = false;
                                    _animator.SetTrigger("Attack3");
                                    yield return new WaitForSeconds(1f);
                                    collider.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage*3);
                                    yield return new WaitForSeconds(_baseAttackSpeed);
                                    _canAttack = true;
                                    _attckCount = 0;
                                }
                            }
                        }
                    }
                    else if (colliderAttack.Length == 1)
                    {
                        _canAttack = false;
                        _animator.SetTrigger("Attack1");
                        yield return new WaitForSeconds(0.1f);
                        Debug.Log(gameObject.tag + " attack " + collider.gameObject.tag);
                        collider.GetComponent<EnemyBase>()?.TakeDamage(Damage);
                        yield return new WaitForSeconds(_baseAttackSpeed);
                        _canAttack = true;
                    }

                }

            }
        }

        private IEnumerator Heal()
        {
            _teamStats.AddHealth(_baseHealStrength);
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
                Gizmos.DrawWireCube(new Vector2(transform.position.x + _offset, transform.position.y), new Vector2(2, 6));
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
            return (int)(_upgradeInfo.BaseCost * UpgradeCostMultiplier());
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
