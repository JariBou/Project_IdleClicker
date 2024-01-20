using ProjectClicker.Core;
using ProjectClicker.Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectClicker.Heroes
{
    public class Arrow : MonoBehaviour
    {
        [FormerlySerializedAs("CircleRadius")]
        [Header("Circle Collider")]
        [SerializeField] private float _circleRadius = 0.5f;
        [FormerlySerializedAs("CircleOffSet")] [SerializeField] private GameObject _circleOffSet;

        [FormerlySerializedAs("BoxSizeX")]
        [Header("Box Collider")]
        [SerializeField] private float _boxSizeX = 0.5f;
        [FormerlySerializedAs("BoxSizeY")] [SerializeField] private float _boxSizeY = 0.5f;
        [FormerlySerializedAs("BoxOffSet")] [SerializeField] private GameObject _boxOffSet;

        [FormerlySerializedAs("BeamSizeX")]
        [Header("Beam Collider")]
        [SerializeField] private float _beamSizeX = 0.5f;
        [FormerlySerializedAs("BeamSizeY")] [SerializeField] private float _beamSizeY = 0.5f;
        [FormerlySerializedAs("BeamOffSet")] [SerializeField] private GameObject _beamOffSet;

        [Header("Rigibody")]
        [SerializeField] private Rigidbody2D _rigidbody2D;

        [FormerlySerializedAs("Speed")] [Header("Arrow Stats")]
        public float _speed = 10f;
        [FormerlySerializedAs("Damage")] public float _damage = 10f;

        public ArrowType _arrowType;

        private Animator _animator;

        private bool _hasHit;

        // Start is called before the first frame update
        private void Start()
        {
            _hasHit = false;
            _damage = GameObject.FindWithTag("Archer").GetComponent<HeroesBehavior>().Damage;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            if (_arrowType == ArrowType.Normal || _arrowType == ArrowType.Entangle || _arrowType == ArrowType.Poison)
            {
                _rigidbody2D.velocity = transform.right * _speed;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_hasHit)
            {
                if (_arrowType == ArrowType.Normal || _arrowType == ArrowType.Entangle || _arrowType == ArrowType.Poison)
                {
                    Collider2D[] colliderHit = Physics2D.OverlapCircleAll(_circleOffSet.transform.position, _circleRadius, LayerMask.GetMask("Enemy"));
                    foreach (var col in colliderHit)
                    {
                        col.gameObject.GetComponent<EnemiesBehavior>()?.TakeDamage(_damage);
                        col.gameObject.GetComponent<EnemyBase>()?.TakeDamage(_damage);
                        _hasHit = true;
                        Debug.Log("Hit with " + _damage + " damage");
                        _rigidbody2D.velocity *= 0;
                        if (col != null && col.gameObject.GetComponent<EnemiesBehavior>()) _rigidbody2D.velocity = col.gameObject.GetComponent<Rigidbody2D>().velocity;
                        if (_arrowType == ArrowType.Normal)
                        {
                            _animator.SetTrigger("NormalHit");
                        }
                        else if (_arrowType == ArrowType.Entangle)
                        {
                            _animator.SetTrigger("EntangleHit");
                        }
                        else if (_arrowType == ArrowType.Poison)
                        {
                            _animator.SetTrigger("PoisonHit");
                        }
                        Destroy(gameObject, 2f);
                        break;
                    }
                    Destroy(gameObject, 8f);
                }
                else if (_arrowType == ArrowType.Shower)
                {
                    Collider2D[] colliderHit = Physics2D.OverlapBoxAll(_boxOffSet.transform.position, new Vector2(_boxSizeX, _boxSizeY),0, LayerMask.GetMask("Enemy"));
                    _animator.SetTrigger("Shower");
                    Debug.Log("Hit with " + _damage + " damage");
                    foreach (var col in colliderHit)
                    {
                        col.gameObject.GetComponent<EnemiesBehavior>()?.TakeDamage(_damage * 3f); //C'est une pluie de fl�ches donc on augmente les d�gats
                        col.gameObject.GetComponent<EnemyBase>()?.TakeDamage(_damage * 3f);
                        
                    }
                    _hasHit = true;
                    Destroy(gameObject, 2.5f);

                }
                else
                {
                    Collider2D[] colliderHit = Physics2D.OverlapBoxAll(_beamOffSet.transform.position, new Vector2(_beamSizeX, _beamSizeY), 0,LayerMask.GetMask("Enemy"));
                    _animator.SetTrigger("Beam");
                    Debug.Log("Hit with " + _damage + " damage");
                    foreach (var col in colliderHit)
                    {
                        col.gameObject.GetComponent<EnemiesBehavior>()?.TakeDamage(_damage * 3f); //M�me logigue que pour la pluie de fl�ches
                        col.gameObject.GetComponent<EnemyBase>()?.TakeDamage(_damage * 3f); 
                    }
                    _hasHit = true;
                    Destroy(gameObject, 2f);
                }
            }
            
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_circleOffSet.transform.position, _circleRadius);
            Gizmos.DrawWireCube(_boxOffSet.transform.position, new Vector2(_boxSizeX, _boxSizeY));
            Gizmos.DrawWireCube(_beamOffSet.transform.position, new Vector2(_beamSizeX, _beamSizeY));
        }


    }


}
