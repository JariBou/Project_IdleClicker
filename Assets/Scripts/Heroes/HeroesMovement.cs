using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectClicker.Heroes
{
   public class HeroesMovement : MonoBehaviour
    {

        [FormerlySerializedAs("Speed")] [SerializeField] private float _speed;
        private Rigidbody2D _rb;
        [FormerlySerializedAs("offset")] [SerializeField] private float _offset;
        public bool _canMove;
        [FormerlySerializedAs("animator")] [SerializeField] private Animator _animator;

        // Start is called before the first frame update
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            /*Debug.Log("Tag " + gameObject.tag);*/
        }

        // Update is called once per frame
        private void Update()
        {
            _animator.SetFloat("Velocity", _rb.velocity.x);
            if (gameObject.CompareTag("Healer") || gameObject.CompareTag("Archer"))
            {
                Collider2D[] colliderMovement = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + _offset, transform.position.y), new Vector2(4,6),0, LayerMask.GetMask("Champion"));
                if (colliderMovement.Length == 0)
                {
                    Move();
                    _canMove = true;
                }
                else
                {
                    _canMove = false;
                    StopMove();
                }

                
            }
            else if (gameObject.CompareTag("Tank") || gameObject.CompareTag("Warrior"))
            {
                Collider2D[] colliderMovement = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + _offset, transform.position.y), new Vector2(2,10),0, LayerMask.GetMask("Enemy"));
                if (colliderMovement.Length == 0)
                {
                    Move();
                    _canMove = true;
                }
                else
                {
                    _canMove = false;
                    StopMove();
                }
            }
        }

        private void Move()
        {
            _rb.velocity = new Vector2(_speed*Time.deltaTime, _rb.velocity.y);
        }
        private void StopMove()
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }

        private void OnDrawGizmos()
        {
            if (_canMove) Gizmos.color = Color.blue;
            else Gizmos.color = Color.red;
            if (gameObject.CompareTag("Healer") || gameObject.CompareTag("Archer"))
            {
                Gizmos.DrawWireCube(new Vector2(transform.position.x + _offset, transform.position.y), new Vector2(4, 6));
            }
            else if (gameObject.CompareTag("Tank") || gameObject.CompareTag("Warrior"))
            {
                Gizmos.DrawWireCube(new Vector2(transform.position.x + _offset, transform.position.y), new Vector2(2, 10));
            }


        }
    }
}
