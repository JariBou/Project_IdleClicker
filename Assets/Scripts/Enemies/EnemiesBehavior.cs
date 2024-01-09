using ProjectClicker.Core;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


namespace ProjectClicker
{
    public class EnemiesBehavior : MonoBehaviour
    {
        [Header("Type")]
        [SerializeField] EnemyType enemyType;
        private EnemyType previousEnemyType;

        [Header("Health")]
        public float health;
        public float maxHealth;
        bool isDead;
        public bool IsDead => isDead;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private GameObject _healthBarGeeenBar;


        [Header("Attack")]
        [SerializeField] float AttackRange;
        [SerializeField] float Damage;
        [SerializeField] float AttackSpeed;
        [SerializeField] private float _offset = 2;
        [SerializeField] private bool _canAttack;
        bool isNearChampion;


        [Header("Stats & Gold")]
        private EnemyBase _enemyBase;
        [SerializeField] private float _level = 0;
        [SerializeField] private float _gold = 500f;
        [SerializeField] private GoldManager _goldManager;

        [Header("Animator")]
        private Animator _animator;
        private Rigidbody2D _rb;
        private int _atkCount = 0;
        // Start is called before the first frame update
        void Start()
        {
            _enemyBase = GameObject.FindWithTag("EnemyBase").GetComponent<EnemyBase>();
            _level = GameObject.FindGameObjectWithTag("Managers").GetComponent<LevelsManager>().CurrentLevel;
            SetStats(enemyType);
            _healthBar.maxValue = maxHealth;
            _healthBar.value = health; 
            _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
            _offset = GetComponent<EnemiesMovement>().Offset;
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();

/*            Debug.Log(gameObject.name);*/
        }

        // Update is called once per frame
        void Update()
        {
            _animator.SetFloat("Velocity", _rb.velocity.x);
            if (_canAttack)
            {
                Collider2D[] colliderAttack = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - _offset, transform.position.y), AttackRange, LayerMask.GetMask("Champion"));
                if (colliderAttack.Length > 0)
                {
                    isNearChampion = true;
                    StartCoroutine(Attack());
                }
                else if (colliderAttack.Length == 0)
                {
                    isNearChampion = false;
                }
            }

        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            _healthBar.value = health;
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                _animator.SetTrigger("TakeDamage");
            }
            if (health < 0 && !isDead)
            {
                isDead = true;
                StartCoroutine(Die());
               
            }
        }

        IEnumerator Die()
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
            Collider2D[] colliderAttack = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - _offset, transform.position.y), AttackRange, LayerMask.GetMask("Champion"));
            if (_atkCount == 0)
            {
                _animator.SetTrigger("Attack1");
                yield return new WaitForSeconds(0.5f);
                foreach (Collider2D collider in colliderAttack)
                {

                    collider.transform.parent.gameObject.GetComponent<TeamStats>().TakeDamage(Damage);
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

                    collider.transform.parent.gameObject.GetComponent<TeamStats>().TakeDamage(Mathf.Round(Damage * 1.15f));
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

                    collider.transform.parent.gameObject.GetComponent<TeamStats>().TakeDamage(Mathf.Round(Damage * 1.35f));
                    Debug.Log(gameObject.name + " attack " + colliderAttack[0].transform.parent.gameObject.name);

                    break; //chaque ennemi attaque 1 seul champion
                }
                _atkCount = 0;
            }

            yield return new WaitForSeconds(AttackSpeed);
            _canAttack = true;
        }   

        private void SetStats(EnemyType state)
        {
            if ( _level == 0) _level = 1;
            switch (state)
            {
                case EnemyType.Melee:
                    _canAttack = true;
                    maxHealth = 500 * _level;
                    health = maxHealth;
                    AttackRange = 3;
                    Damage = 190 * _level;
                    AttackSpeed = 2.5f / (_level*1.05f);
                    _gold = 500 * _level;
                    break;
                case EnemyType.Ranged:
                    _canAttack = true;
                    maxHealth = 500 * _level;
                    health = maxHealth;
                    AttackRange = 7.5f;
                    Damage = 150 * _level;
                    AttackSpeed = 1.5f / (_level * 1.05f);
                    _gold = 1000 * _level;
                    break;
                case EnemyType.Boss:
                    _canAttack = true;
                    maxHealth = 5000 * _level;
                    health = maxHealth;
                    AttackRange = 5;
                    Damage = 450 * _level;
                    AttackSpeed = 1/(_level * 1.05f);
                    _gold = 10000 * _level;
                    break;
            }
        }
        private void OnValidate()
        {
            if (enemyType != previousEnemyType)
            {
                SetStats(enemyType);
                previousEnemyType = enemyType;
            }
        }

        private void OnDrawGizmos()
        {
            if (isNearChampion)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireSphere(new Vector2(transform.position.x - _offset, transform.position.y), AttackRange);
        }
    }



    public enum EnemyType
    {
        Melee,
        Ranged,
        Boss
    }
}
