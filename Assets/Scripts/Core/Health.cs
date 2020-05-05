using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
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
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            float healthPoints = (float) state;
            health = healthPoints;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }
    }

}
