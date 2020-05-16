using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Resources;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 70)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null; // Stats that will point to (core)
        [SerializeField] private GameObject levelUpEffect = null;
        [SerializeField] private bool shouldUseModifiers = false;

        public event Action OnLevelUp;

        private LazyValue<int> currentLevel;
        private Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.value = CalculateLevel();
            currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
            
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpEffect, transform);
        }

        public float GetStat(Stat statToGet)
        {
            return (GetBaseStat(statToGet) + GetAdditiveModifier(statToGet)) * (1 + GetPercentageModifier(statToGet)/100);
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        private float GetBaseStat(Stat statToGet)
        {
            return progression.GetStat(statToGet, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat statToGet)
        {
            if (!shouldUseModifiers) return 0;
            if (GetComponents<IModifierProvider>() == null) return 0;
            float sumOfModifiers = 0;

            foreach (var modifierComponent in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in modifierComponent.GetAdditiveModifiers(statToGet))
                {
                    sumOfModifiers += modifier;
                }
            }

            return sumOfModifiers;
        }

        private float GetPercentageModifier(Stat statToGet)
        {
            if (!shouldUseModifiers) return 0;
            if (GetComponents<IModifierProvider>() == null) return 0;
            float totalPercentageBonus = 0;

            foreach (var modifierComponent in GetComponents<IModifierProvider>())
            {
                foreach (var percentageModifier in modifierComponent.GetPercentageModifiers(statToGet))
                {
                    totalPercentageBonus += percentageModifier;
                }
            }

            return totalPercentageBonus;
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentXP = GetComponent<Experience>().GetExperiencePoints();
            int maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= maxLevel; level++)
            {
                float XPToLevelUP = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUP > currentXP)
                {
                    return level;
                }
            }

            return maxLevel + 1;
        }
    }

}