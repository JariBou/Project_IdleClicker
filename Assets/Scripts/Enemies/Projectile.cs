using NaughtyAttributes;
using ProjectClicker.Enemies;
using ProjectClicker.Heroes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectClicker
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] private float _speed;
        [SerializeField] private bool _isEnemyProjectile;
        [SerializeField, ShowIf("_isEnemyProjectile")]
        private EnemiesBehavior _enemiesBehavior;
        [SerializeField, HideIf("_isEnemyProjectile")]
        private HeroesBehavior _heroesBehavior;
        [SerializeField, HideIf("_isEnemyProjectile")]
        private TeamStats _teamStats;
        private Animator _animator;
        private Rigidbody2D _rb;
        private CircleCollider2D _collider;

        private bool _hasHit;

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CircleCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isEnemyProjectile && !_hasHit)
            {
                _rb.velocity = Vector2.right * _speed * Time.deltaTime;
            }
            else if (!_hasHit)
            {
               _rb.velocity = Vector2.left * _speed * Time.deltaTime;
            }
        }

        public void Initialize(EnemiesBehavior enemiesBehavior)
        {
            _enemiesBehavior = enemiesBehavior;
        }

        public void Initialize(HeroesBehavior heroesBehavior)
        {
            _heroesBehavior = heroesBehavior;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isEnemyProjectile)
            {
                if (other.gameObject.transform.parent.gameObject.CompareTag("Team") && other != null)
                {
                    _teamStats = other.transform.parent.gameObject.GetComponent<TeamStats>();
                    _teamStats.TakeDamage(_enemiesBehavior.Damage);
                    _hasHit = true;
                    _rb.velocity = Vector2.zero;
                    _animator.SetTrigger("Hit");
                    Destroy(gameObject, 0.5f);
                }
            }
            else
            {
                if (other.CompareTag("Enemy"))
                {
                    _enemiesBehavior = other.GetComponent<EnemiesBehavior>();
                    _enemiesBehavior.TakeDamage(_heroesBehavior.Damage);
                    _hasHit = true;
                    _rb.velocity = Vector2.zero;
                    _animator.SetTrigger("Hit");
                    Destroy(gameObject, 0.5f);
                }
            }
        }
    }
}
