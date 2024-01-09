using UnityEngine;

namespace ProjectClicker
{
    public class Arrow : MonoBehaviour
    {
        [Header("Circle Collider")]
        [SerializeField] private float CircleRadius = 0.5f;
        [SerializeField] private GameObject CircleOffSet;

        [Header("Box Collider")]
        [SerializeField] private float BoxSizeX = 0.5f;
        [SerializeField] private float BoxSizeY = 0.5f;
        [SerializeField] private GameObject BoxOffSet;

        [Header("Beam Collider")]
        [SerializeField] private float BeamSizeX = 0.5f;
        [SerializeField] private float BeamSizeY = 0.5f;
        [SerializeField] private GameObject BeamOffSet;

        [Header("Rigibody")]
        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Header("Arrow Stats")]
        public float Speed = 10f;
        public float Damage = 10f;

        public ArrowType _arrowType;

        private Animator _animator;

        private bool _hasHit;

        // Start is called before the first frame update
        void Start()
        {
            _hasHit = false;
            Damage = GameObject.FindWithTag("Archer").GetComponent<HeroesBehavior>().Damage;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            if (_arrowType == ArrowType.Normal || _arrowType == ArrowType.Entangle || _arrowType == ArrowType.Poison)
            {
                _rigidbody2D.velocity = transform.right * Speed;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!_hasHit)
            {
                if (_arrowType == ArrowType.Normal || _arrowType == ArrowType.Entangle || _arrowType == ArrowType.Poison)
                {
                    Collider2D[] colliderHit = Physics2D.OverlapCircleAll(CircleOffSet.transform.position, CircleRadius, LayerMask.GetMask("Enemy"));
                    foreach (var collider in colliderHit)
                    {
                        collider.gameObject.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage);
                        collider.gameObject.GetComponent<EnemyBase>()?.TakeDamage(Damage);
                        _hasHit = true;
                        Debug.Log("Hit with " + Damage + " damage");
                        if (_arrowType == ArrowType.Normal)
                        {
                            _rigidbody2D.velocity *= 0;
                            _rigidbody2D.velocity = collider.gameObject.GetComponent<Rigidbody2D>().velocity;
                            _animator.SetTrigger("NormalHit");
                            Destroy(gameObject, 2f);
                        }
                        else if (_arrowType == ArrowType.Entangle)
                        {
                            _rigidbody2D.velocity *= 0;
                            _rigidbody2D.velocity = collider.gameObject.GetComponent<Rigidbody2D>().velocity;
                            _animator.SetTrigger("EntangleHit");
                            Destroy(gameObject, 2f);
                        }
                        else if (_arrowType == ArrowType.Poison)
                        {
                            _rigidbody2D.velocity *= 0;
                            _rigidbody2D.velocity = collider.gameObject.GetComponent<Rigidbody2D>().velocity;
                            _animator.SetTrigger("PoisonHit");
                            Destroy(gameObject, 2f);
                        }
                        break;
                    }
                    Destroy(gameObject, 8f);
                }
                else if (_arrowType == ArrowType.Shower)
                {
                    Collider2D[] colliderHit = Physics2D.OverlapBoxAll(BoxOffSet.transform.position, new Vector2(BoxSizeX, BoxSizeY),0, LayerMask.GetMask("Enemy"));
                    _animator.SetTrigger("Shower");
                    Debug.Log("Hit with " + Damage + " damage");
                    foreach (var collider in colliderHit)
                    {
                        collider?.gameObject.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage * 3f); //C'est une pluie de fl�ches donc on augmente les d�gats
                        collider?.gameObject.GetComponent<EnemyBase>()?.TakeDamage(Damage * 3f);
                        
                    }
                    _hasHit = true;
                    Destroy(gameObject, 2.5f);

                }
                else
                {
                    Collider2D[] colliderHit = Physics2D.OverlapBoxAll(BeamOffSet.transform.position, new Vector2(BeamSizeX, BeamSizeY), 0,LayerMask.GetMask("Enemy"));
                    _animator.SetTrigger("Beam");
                    Debug.Log("Hit with " + Damage + " damage");
                    foreach (var collider in colliderHit)
                    {
                        collider?.gameObject.GetComponent<EnemiesBehavior>()?.TakeDamage(Damage * 3f); //M�me logigue que pour la pluie de fl�ches
                        collider?.gameObject.GetComponent<EnemyBase>()?.TakeDamage(Damage * 3f); 
                    }
                    _hasHit = true;
                    Destroy(gameObject, 2f);
                }
            }
            
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(CircleOffSet.transform.position, CircleRadius);
            Gizmos.DrawWireCube(BoxOffSet.transform.position, new Vector2(BoxSizeX, BoxSizeY));
            Gizmos.DrawWireCube(BeamOffSet.transform.position, new Vector2(BeamSizeX, BeamSizeY));
        }


    }


    public enum ArrowType
    {
        Normal = 0,
        Entangle = 1,
        Poison = 2,
        Shower = 3,
        Beam = 4
    }
}
