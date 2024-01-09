using UnityEngine;

namespace ProjectClicker
{
    public class EnemiesMovement : MonoBehaviour
    {
        [SerializeField] float Speed = 500;
        Rigidbody2D rb;
        [SerializeField] float _offset;
        bool _canMove;

        public float Offset => _offset;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            Collider2D[] colliderMovement = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - _offset, transform.position.y), new Vector2(2, 6), 0, LayerMask.GetMask("Champion"));
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
            rb.velocity *= 0;
        }

        private void Move()
        {
            rb.velocity = new Vector2(-Speed * Time.deltaTime, rb.velocity.y);
        }

        private void OnDrawGizmos()
        {
            if (_canMove)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireCube(new Vector2(transform.position.x - _offset, transform.position.y), new Vector2(2, 6));
        }
        
    }
}
