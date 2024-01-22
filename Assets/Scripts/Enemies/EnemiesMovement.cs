using UnityEngine;

namespace ProjectClicker.Enemies
{
    public class EnemiesMovement : MonoBehaviour
    {
        [SerializeField] private float Speed = 500;
        private Rigidbody2D _rb;
        [SerializeField] private float _offset;
        [SerializeField] private float _width;
        [SerializeField] private float _height;
        private bool _canMove;
        private EnemiesBehavior _enemiesBehavior;

        public float Offset => _offset;
        // Start is called before the first frame update
        private void Start()
        {
            _enemiesBehavior = GetComponent<EnemiesBehavior>();
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (_enemiesBehavior.IsDead)
            {
                StopMove();
                return;
            }
            Collider2D[] colliderMovement = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - _offset, transform.position.y), new Vector2(_width, _height), 0, LayerMask.GetMask("Champion"));
            if (colliderMovement.Length == 0)
            {
                Move();
                _canMove = true;
            }
            else
            {
                StopMove();
                _canMove = false;
            }
        }

        private void StopMove()
        {
            _rb.velocity *= 0;
        }

        private void Move()
        {
            _rb.velocity = new Vector2(-Speed * Time.deltaTime, _rb.velocity.y);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _canMove ? Color.green : Color.red;
            Gizmos.DrawWireCube(new Vector2(transform.position.x - _offset, transform.position.y), new Vector2(_width, _height));
        }
        
    }
}
