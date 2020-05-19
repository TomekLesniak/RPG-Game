using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private TakeDamageEvent takeDamage;
        [SerializeField] private UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        LazyValue<float> health;
        private bool isDead = false;

        private BaseStats baseStats;

        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            health.ForceInit();
            baseStats = GetComponent<BaseStats>();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += SetHealthToFull;
            //baseStats.OnLevelUp += SetHealthToFull;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= SetHealthToFull;
            //baseStats.OnLevelUp -= SetHealthToFull;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float amountOfDamage)
        {
            print(gameObject.name + " took damage: " + amountOfDamage);
            takeDamage?.Invoke(amountOfDamage); // DamageText UI

            if (health.value - amountOfDamage <= 0)
            {
                health.value = 0;
                Die();
                onDie?.Invoke();
                GiveExperience(instigator);
            }
            else
                health.value -= amountOfDamage;
        }


        public float GetHealthPoints()
        {
            return health.value;
        }

        public float GetMaxHealthPoints()
        {
            return baseStats.GetStat(Stat.Health);
        }

        public float GetHealthPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return health.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void GiveExperience(GameObject instigator)
        {
            if (instigator.GetComponent<Experience>() == null) return;

            var experienceReward = this.GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);
            instigator.GetComponent<Experience>().GainExperience(experienceReward);
        }

        private void Die()
        {
            if (isDead) return;

            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void SetHealthToFull()
        {
            health.value = baseStats.GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return health.value;
        }

        public void RestoreState(object state)
        {
            float healthPoints = (float) state;
            health.value = healthPoints;
            if (health.value <= 0)
            {
                health.value = 0;
                Die();
            }
        }
    }

}
