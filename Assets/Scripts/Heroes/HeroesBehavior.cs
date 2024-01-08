using System;
using System.Collections;
using System.Xml;
using ProjectClicker.Core;
using Unity.VisualScripting;
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
        public int HeroLevel => _heroLevel;
        private LevelsManager _levelsManager;

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
        [FormerlySerializedAs("Can Attack")][SerializeField] private bool _canAttack;


        [FormerlySerializedAs("healStrength")]
        [FormerlySerializedAs("healStrenght")]
        [Header("Heal")]
        [SerializeField]
        private float _healStrength = 100;
        private bool _canHeal = true;

        [FormerlySerializedAs("armor")]
        [Header("Armor")]
        [SerializeField]
        private float _armor;

        [Header("Arrow")]
        [SerializeField] private Transform _arrowSpawnPoint;
        [SerializeField] private GameObject _arrowPrefab;
        private int _attckCount = 0;

 


        public float MaxHealth => _maxHealth + _heroLevel * _upgradeInfo.HealthPerLevel;
        public float Damage => _damage + _heroLevel * _upgradeInfo.DmgPerLevel;
        public float AttackSpeed => _attackSpeed + _heroLevel * _upgradeInfo.AtkSpeedPerLevel;
        public float PowerHeal => _healStrength + _heroLevel * _upgradeInfo.HealStrengthPerLevel;
        public float Armor => _armor + _heroLevel * _upgradeInfo.ArmorPerLevel;

        private Animator _animator;
        private Rigidbody2D _rb;

        [Header("Attack Offset")]
        [SerializeField] private float _offset;
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            /*Debug.Log(gameObject.name);*/
            _levelsManager = GameObject.FindWithTag("Managers").GetComponent<LevelsManager>();  
            _levelsManager.ChangeLevel += OnChangingLevel;
            
        }


        // Update is called once per frame
        private void Update()
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
            if (_teamStats.CurrentHealth < _teamStats.BaseMaxHealth/2 && _canHeal && _role == ChampionRole.HEALER)
            {
                _canHeal = false;
                _animator.SetTrigger("Heal");
                StartCoroutine(Heal());
            }
            if (_role == ChampionRole.HEALER)
            {
                if (_canAttack && colliderAttack.Length != 0 && _teamStats.CurrentHealth > _teamStats.BaseMaxHealth / 2)
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
                case ChampionRole.HEALER:
                    _maxHealth = 150f;
                    _damage = 10f;
                    _attackRange = 8f;
                    _attackSpeed = 3;
                    _canAttack = true;
                    _healStrength = 100;
                    _armor = 5;
                    gameObject.tag = "Healer";
                    break;
                case ChampionRole.TANK:
                    _maxHealth = 500;
                    _damage = 30f;
                    _attackRange = 2f;
                    _attackSpeed = 4;
                    _canAttack = true;
                    _healStrength = 0;
                    _armor = 75;
                    gameObject.tag = "Tank";
                    break;
                case ChampionRole.ARCHER:
                    _maxHealth = 100;
                    _damage = 75f;
                    _attackRange = 8f;
                    _attackSpeed = 2;
                    _canAttack = true;
                    _healStrength = 0;
                    _armor = 10;
                    gameObject.tag = "Archer";
                    break;
                case ChampionRole.WARRIOR:
                    _maxHealth = 250;
                    _damage = 75f;
                    _attackRange = 2f;
                    _attackSpeed = 3;
                    _canAttack = true;
                    _healStrength = 0;
                    _armor = 25;
                    gameObject.tag = "Warrior";
                    break;
                default:
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
                    if (collider?.tag != "EnemyBase")
                    {
                        if (!collider.gameObject.GetComponent<EnemiesBehavior>().IsDead)
                        {
                            if (_role == ChampionRole.ARCHER)
                            {
                                if (_attckCount == 0)
                                {
                                    _canAttack = false;

                                    _animator.SetTrigger("Attack1");
                                    yield return new WaitForSeconds(0.1f);
                                    Debug.Log(gameObject.tag + " attack " + collider.gameObject.tag);
                                    collider?.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage);
                                    yield return new WaitForSeconds(_attackSpeed + 0.5f);
                                    
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
                                    arrow.GetComponent<Arrow>()._arrowType = (ArrowType)UnityEngine.Random.Range(0, 2);
                                    yield return new WaitForSeconds(_attackSpeed + 0.5f);
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
                                    yield return new WaitForSeconds(_attackSpeed + 0.5f);
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
                                    yield return new WaitForSeconds(_attackSpeed + 0.5f);
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
                                    collider?.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage);

                                    yield return new WaitForSeconds(_attackSpeed);
                                    _canAttack = true;
                                    if (_heroLevel >= 10) _attckCount++;
                                    /*_attckCount++;*/
                                }
                                else if (_attckCount == 2)
                                {
                                    _canAttack = false;

                                    _animator.SetTrigger("Attack2");
                                    yield return new WaitForSeconds(1f);
                                    collider?.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage*2);
                                    yield return new WaitForSeconds(_attackSpeed);
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
                                    collider?.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage*3);
                                    yield return new WaitForSeconds(_attackSpeed);
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
                        collider?.GetComponent<EnemyBase>()?.TakeDamage(Damage);
                        yield return new WaitForSeconds(_attackSpeed);
                        _canAttack = true;
                    }

                }

            }
        }

        public enum ChampionRole
        {
            HEALER,
            TANK,
            ARCHER,
            WARRIOR
        }

        private IEnumerator Heal()
        {
            _teamStats.AddHealth(_healStrength);
            yield return new WaitForSeconds(_attackSpeed);
            _canHeal = true;
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
            _damage += _upgradeInfo.DmgPerLevel * _heroLevel;
            _maxHealth += _upgradeInfo.HealthPerLevel * _heroLevel;
            _armor += _upgradeInfo.ArmorPerLevel * _heroLevel;
            _healStrength += _upgradeInfo.HealStrengthPerLevel * _heroLevel;
            if (_attackSpeed > _upgradeInfo.MinAtkSpeed)  _attackSpeed -= _upgradeInfo.AtkSpeedPerLevel * _heroLevel;
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

        private void OnDestroy() // Peut �tre inutile vu qu'on ne d�truit pas les h�ros mais au cas o� on les d�truit un jour
        {
            _levelsManager.ChangeLevel -= OnChangingLevel;
        }
    }
}
