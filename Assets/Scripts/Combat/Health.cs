using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;
        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float amountOfDamage)
        {
            if (health - amountOfDamage <= 0)
            {
                health = 0;
                Die();
            }
            else
                health -= amountOfDamage;
            
            print(health); //debug line
        }

        private void Die()
        {
            if (isDead) return;

            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
        }
    }

}
