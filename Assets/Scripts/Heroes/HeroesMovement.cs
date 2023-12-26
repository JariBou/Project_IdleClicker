using UnityEngine;

namespace ProjectClicker
{
   public class HeroesMovement : MonoBehaviour
    {

        [SerializeField] float Speed = 150;
        Rigidbody2D rb;
        [SerializeField] float offset;
        public bool canMove;
        [SerializeField] Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetFloat("Velocity", rb.velocity.x);
            if (gameObject.tag == "Healer" || gameObject.tag == "Archer")
            {
                Collider2D[] colliderMovement = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + offset, transform.position.y), new Vector2(4,6),0, LayerMask.GetMask("Champion"));
                if (colliderMovement.Length == 0)
                {
                    Move();
                    canMove = true;
                }
                else
                {
                    canMove = false;
                    StopMove();
                }

                
            }
            else if (gameObject.tag == "Tank" || gameObject.tag == "Warrior")
            {
                Collider2D[] colliderMovement = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + offset, transform.position.y), new Vector2(2,6),0, LayerMask.GetMask("Enemy"));
                if (colliderMovement.Length == 0)
                {
                    Move();
                    canMove = true;
                }
                else
                {
                    canMove = false;
                    StopMove();
                }
            }
            
            

        }

        private void Move()
        {
            rb.velocity = new Vector2(Speed*Time.deltaTime, rb.velocity.y);
        }
        private void StopMove()
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        private void OnDrawGizmosSelected()
        {
            if (canMove) Gizmos.color = Color.blue;
            else Gizmos.color = Color.red;
            if (gameObject.tag == "Healer" || gameObject.tag == "Archer")
            {
                Gizmos.DrawWireCube(new Vector2(transform.position.x + offset, transform.position.y), new Vector2(4, 6));
            }
            else if (gameObject.tag == "Tank" || gameObject.tag == "Warrior")
            {
                Gizmos.DrawWireCube(new Vector2(transform.position.x + offset, transform.position.y), new Vector2(2, 6));
            }


        }
    }
}
