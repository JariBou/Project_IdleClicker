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
        [SerializeField] private Slider _healthBar;


        [Header("Attack")]
        [SerializeField] float AttackRange;
        [SerializeField] float Damage;
        [SerializeField] float AttackSpeed;
        [SerializeField] private float _offset = 2;
        [SerializeField] private bool _canAttack;
        bool isNearChampion;


        [Header("Stats & Gold")]
        [SerializeField] private float _level = 1;
        [SerializeField] private float _gold = 500f;
        [SerializeField] private GoldManager _goldManager;
        // Start is called before the first frame update
        void Start()
        {
            _healthBar.maxValue = maxHealth;
            _healthBar.value = health;
            _goldManager = GameObject.FindWithTag("Managers").GetComponent<GoldManager>();
            _offset = GetComponent<EnemiesMovement>().Offset;
/*            Debug.Log(gameObject.name);*/
        }

        // Update is called once per frame
        void Update()
        {
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
            if (health < 0 && !isDead)
            {
                isDead = true;
                _goldManager.AddGold((ulong)(_gold * _level));
                Destroy(gameObject);
            }
        }

        private IEnumerator Attack()
        {
            _canAttack = false;
            Collider2D[] colliderAttack = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - _offset, transform.position.y), AttackRange, LayerMask.GetMask("Champion"));
            foreach (Collider2D collider in colliderAttack)
            {
                collider.transform.parent.gameObject.GetComponent<TeamStats>().TakeDamage(Damage);
                Debug.Log(gameObject.name + " attack " + colliderAttack[0].transform.parent.gameObject.name);
                break;
            }
            yield return new WaitForSeconds(AttackSpeed);
            _canAttack = true;
        }   

        private void SetStats(EnemyType state)
        {
            switch (state)
            {
                case EnemyType.Melee:
                    _canAttack = true;
                    maxHealth = 1000 * _level;
                    health = maxHealth;
                    AttackRange = 3;
                    Damage = 300 * _level;
                    AttackSpeed = 2.5f / (_level*1.05f);
                    _gold = 500 * _level;
                    break;
                case EnemyType.Ranged:
                    _canAttack = true;
                    maxHealth = 500 * _level;
                    health = maxHealth;
                    AttackRange = 7.5f;
                    Damage = 200 * _level;
                    AttackSpeed = 1.5f / (_level * 1.05f);
                    _gold = 1000 * _level;
                    break;
                case EnemyType.Boss:
                    _canAttack = true;
                    maxHealth = 5000 * _level;
                    health = maxHealth;
                    AttackRange = 5;
                    Damage = 500 * _level;
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
