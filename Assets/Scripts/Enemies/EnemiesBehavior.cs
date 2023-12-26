using UnityEngine;
using UnityEngine.UI;


namespace ProjectClicker
{
    public class EnemiesBehavior : MonoBehaviour
    {

        [Header("Health")]
        public float health;
        public float maxHealth;
        bool isDead;
        [SerializeField] private Slider _healthBar;


        [Header("Attack")]
        [SerializeField] float AttackRange;
        [SerializeField] float Damage;
        // Start is called before the first frame update
        void Start()
        {
            _healthBar.maxValue = maxHealth;
            _healthBar.value = health;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            _healthBar.value = health;
            if (health < 0 && !isDead)
            {
                isDead = true;
                Destroy(gameObject);
            }
        }
    }
}
