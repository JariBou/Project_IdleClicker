using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectClicker
{
    public class EnemiesBehavior : MonoBehaviour
    {

        [Header("Health")]
        public float health;
        public float maxHealth;
        bool isDead;

        [Header("Attack")]
        [SerializeField] float AttackRange;
        [SerializeField] float Damage;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health < 0 && !isDead)
            {
                isDead = true;
                Destroy(gameObject);
            }
        }
    }
}
