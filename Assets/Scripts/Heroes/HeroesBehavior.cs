using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectClicker
{
    public class HeroesBehavior : MonoBehaviour
    {
        [SerializeField] championRole Role;
        private championRole PreviousRole;

        [Header("Team Manager")]
        [SerializeField] TeamStats teamStats;

        [Header("Health")]
        public float maxHealth;

        [Header("Attack")]
        [SerializeField] float Damage;
        [SerializeField] float attackRange;
        [SerializeField] float attackSpeed;
        [SerializeField] LayerMask layerToHit;
        bool canAttack;


        [Header("Heal")]
        [SerializeField] float healRate;
        [SerializeField] float healStrenght;

        



        // Start is called before the first frame update
        void Start()
        {
            Debug.Log((int)layerToHit);
            
        }

        // Update is called once per frame
        void Update()
        {
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
        }

        public void SetStat(championRole role)
        {
            switch (role)
            {
                case championRole.HEALER:
                    maxHealth = 150f;
                    Damage = 10f;
                    attackRange = 8f;
                    attackSpeed = 3;
                    canAttack = true;
                    healRate = 5; // Temporaire
                    healStrenght = 100;
                    
                    break;
                case championRole.TANK:
                    maxHealth = 500;
                    Damage = 30f;
                    attackRange = 2f;
                    attackSpeed = 4;
                    canAttack = true;
                    healRate = 0; // Temporaire
                    healStrenght = 0;
                    break;
                case championRole.ARCHER:
                    maxHealth = 100;
                    Damage = 50f;
                    attackRange = 10f;
                    attackSpeed = 1;
                    canAttack = true;
                    healRate = 0; // Temporaire
                    healStrenght = 0;
                    break;
                case championRole.REGULAR:
                    maxHealth = 250;
                    Damage = 40f;
                    attackRange = 5f;
                    attackSpeed = 2;
                    canAttack = true;
                    healRate = 0; // Temporaire
                    healStrenght = 0;
                    break;
            }
        }


        IEnumerator Attack()
        {
            Collider2D[] colliderAttack = Physics2D.OverlapCircleAll(transform.position, attackRange, layerToHit);
            foreach (Collider2D collider in colliderAttack)
            {
                canAttack = false;
                collider.GetComponent<EnemiesBehavior>().TakeDamage(Damage);
                yield return new WaitForSeconds(attackSpeed);
                canAttack = true;
            }
        }

        void Heal()
        {

            teamStats.AddHealth(healStrenght);


        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        private void OnValidate()
        {
            if (PreviousRole != Role)
            {
                PreviousRole = Role;
                SetStat(Role);
            }
        }
    }

    public enum championRole
    {
        TANK = 1,
        HEALER = 2,
        ARCHER = 3,
        REGULAR = 4,

    }

    
}
