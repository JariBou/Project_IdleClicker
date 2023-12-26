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
        [SerializeField] private float maxHealth;

        [Header("Attack")]
        [SerializeField] float damage;
        [SerializeField] float attackRange;
        [SerializeField] float attackSpeed;
        [SerializeField] LayerMask layerToHit;
        bool canAttack;


        [Header("Heal")]
        [SerializeField] float healStrenght;

        [Header("Armor")]
        [SerializeField] float armor;

        public float MaxHealth => maxHealth;
        public float Damage => damage;
        public float AttackSpeed => attackSpeed;
        public float PowerHeal => healStrenght;
        public float Armor => armor;
        



        // Start is called before the first frame update
        void Start()
        {
            
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
                    damage = 10f;
                    attackRange = 8f;
                    attackSpeed = 3;
                    canAttack = true;
                    healStrenght = 100;
                    armor = 75;
                    gameObject.tag = "Healer";
                    
                    break;
                case championRole.TANK:
                    maxHealth = 500;
                    damage = 30f;
                    attackRange = 2f;
                    attackSpeed = 4;
                    canAttack = true;
                    healStrenght = 0;
                    armor = 150;
                    gameObject.tag = "Tank";
                    break;
                case championRole.ARCHER:
                    maxHealth = 100;
                    damage = 75f;
                    attackRange = 8f;
                    attackSpeed = 1;
                    canAttack = true;
                    healStrenght = 0;
                    armor = 50;
                    gameObject.tag = "Archer";
                    break;
                case championRole.WARRIOR:
                    maxHealth = 250;
                    damage = 75f;
                    attackRange = 2f;
                    attackSpeed = 3;
                    canAttack = true;
                    healStrenght = 0;
                    armor = 250;
                    gameObject.tag = "Warrior";
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

        public championRole GetRole()
        {
            return Role;
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
        WARRIOR = 4,

    }

    
}
