using System.Collections;
using ProjectClicker.Core;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ProjectClicker.Enemies
{
    public class EnemiesBehavior : MonoBehaviour
    {
        [FormerlySerializedAs("enemyType")]
        [Header("Type")]
        [SerializeField]
        private EnemyType _enemyType;
        private EnemyType _previousEnemyType;

        [FormerlySerializedAs("health")] [Header("Health")]
        public float _health;
        [FormerlySerializedAs("maxHealth")] public float _maxHealth;
        private bool _isDead;
        public bool IsDead => _isDead;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private GameObject _healthBarGeeenBar;


        [FormerlySerializedAs("AttackRange")]
        [Header("Attack")]
        [SerializeField]
        private float _attackRange;
        [FormerlySerializedAs("Damage")] [SerializeField] private float _damage;
        [FormerlySerializedAs("AttackSpeed")] [SerializeField] private float _attackSpeed;
        [SerializeField] private float _offset = 2;
        [SerializeField] private bool _canAttack;
        private bool _isNearChampion;


        [Header("Stats & Gold")]
        private EnemyBase _enemyBase;
        [SerializeField] private float _level = 0;
        [SerializeField] private float _gold = 500f;
        [SerializeField] private GoldManager _goldManager;

        [Header("Animator")]
        private Animator _animator;
        private Rigidbody2D _rb;
        private int _atkCount;
        // Start is called before the first frame update
        private void Start()
        {
            _enemyBase = GameObject.FindWithTag("EnemyBase").GetComponent<EnemyBase>();
            _level = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelsManager>().CurrentLevel;
            SetStats(_enemyType);
            _healthBar.maxValue = _maxHealth;
            _healthBar.value = _health; 
            _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
            _offset = GetComponent<EnemiesMovement>().Offset;
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();

/*            Debug.Log(gameObject.name);*/
        }

        // Update is called once per frame
        private void Update()
        {
            _animator.SetFloat("Velocity", _rb.velocity.x);
            if (_canAttack)
            {
                Collider2D[] colliderAttack = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - _offset, transform.position.y), _attackRange, LayerMask.GetMask("Champion"));
                if (colliderAttack.Length > 0)
                {
                    _isNearChampion = true;
                    StartCoroutine(Attack());
                }
                else if (colliderAttack.Length == 0)
                {
                    _isNearChampion = false;
                }
            }

        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            _healthBar.value = _health;
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                _animator.SetTrigger("TakeDamage");
            }
            if (_health < 0 && !_isDead)
            {
                _isDead = true;
                StartCoroutine(Die());
               
            }
        }

        private IEnumerator Die()
        {
            _healthBarGeeenBar.SetActive(false);
            _animator.SetTrigger("Die");
            if (_level > 0) _goldManager.AddGold((ulong)(_gold * _level));
            else _goldManager.AddGold((ulong)(_gold));
            yield return new WaitForSeconds(1f);
            _enemyBase.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }

        private IEnumerator Attack()
        {
            _canAttack = false;
            Collider2D[] colliderAttack = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - _offset, transform.position.y), _attackRange, LayerMask.GetMask("Champion"));
            if (_atkCount == 0)
            {
                _animator.SetTrigger("Attack1");
                yield return new WaitForSeconds(0.5f);
                foreach (Collider2D collider in colliderAttack)
                {

                    collider.transform.parent.gameObject.GetComponent<TeamStats>().TakeDamage(_damage);
                    Debug.Log(gameObject.name + " attack " + colliderAttack[0].transform.parent.gameObject.name);

                    break; //chaque ennemi attaque 1 seul champion
                }
                _atkCount++;
            }
            else if (_atkCount == 1)
            {
                _animator.SetTrigger("Attack2");
                yield return new WaitForSeconds(0.5f);
                foreach (Collider2D collider in colliderAttack)
                {

                    collider.transform.parent.gameObject.GetComponent<TeamStats>().TakeDamage(Mathf.Round(_damage * 1.15f));
                    Debug.Log(gameObject.name + " attack " + colliderAttack[0].transform.parent.gameObject.name);

                    break; //chaque ennemi attaque 1 seul champion
                }
                _atkCount++;
            }
            else if (_atkCount == 2)
            {
                _animator.SetTrigger("Attack3");
                yield return new WaitForSeconds(0.5f);
                foreach (Collider2D collider in colliderAttack)
                {

                    collider.transform.parent.gameObject.GetComponent<TeamStats>().TakeDamage(Mathf.Round(_damage * 1.35f));
                    Debug.Log(gameObject.name + " attack " + colliderAttack[0].transform.parent.gameObject.name);

                    break; //chaque ennemi attaque 1 seul champion
                }
                _atkCount = 0;
            }

            yield return new WaitForSeconds(_attackSpeed);
            _canAttack = true;
        }   

        private void SetStats(EnemyType state)
        {
            if ( _level == 0) _level = 1;
            switch (state)
            {
                case EnemyType.Melee:
                    _canAttack = true;
                    _maxHealth = 500 * _level;
                    _health = _maxHealth;
                    _attackRange = 3;
                    _damage = 190 * _level;
                    _attackSpeed = 2.5f / (_level*1.05f);
                    _gold = 500 * _level;
                    break;
                case EnemyType.Ranged:
                    _canAttack = true;
                    _maxHealth = 500 * _level;
                    _health = _maxHealth;
                    _attackRange = 7.5f;
                    _damage = 150 * _level;
                    _attackSpeed = 1.5f / (_level * 1.05f);
                    _gold = 1000 * _level;
                    break;
                case EnemyType.Boss:
                    _canAttack = true;
                    _maxHealth = 5000 * _level;
                    _health = _maxHealth;
                    _attackRange = 5;
                    _damage = 450 * _level;
                    _attackSpeed = 1/(_level * 1.05f);
                    _gold = 10000 * _level;
                    break;
            }
        }
        private void OnValidate()
        {
            if (_enemyType != _previousEnemyType)
            {
                SetStats(_enemyType);
                _previousEnemyType = _enemyType;
            }
        }

        private void OnDrawGizmos()
        {
            if (_isNearChampion)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireSphere(new Vector2(transform.position.x - _offset, transform.position.y), _attackRange);
        }
    }



    public enum EnemyType
    {
        Melee,
        Ranged,
        Boss
    }
}
