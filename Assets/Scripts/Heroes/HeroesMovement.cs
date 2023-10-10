using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectClicker
{
   public class HeroesMovement : MonoBehaviour
    {

        [SerializeField] float Speed = 150;
        Rigidbody2D rb;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void Move()
        {
            rb.velocity = new Vector2(Speed*Time.deltaTime, rb.velocity.y);
        }
    }
}
