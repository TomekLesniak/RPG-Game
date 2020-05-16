using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats / Create Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupDictionary = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels = lookupDictionary[characterClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            if (level == 0)
            {
                return levels[0];
            }

            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();

            float[] levels = lookupDictionary[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookupDictionary != null) return;

            lookupDictionary = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupDictionary = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupDictionary[progressionStat.stat] = progressionStat.levels;
                }

                lookupDictionary[progressionClass.characterClass] = statLookupDictionary;
            }
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        { 
            public CharacterClass characterClass;
            public ProgressionStat[] stats;

        }

        [System.Serializable]
        public class ProgressionStat
        {
            public Stat stat; 
            public float[] levels; //public variables don`t have to be serialized
        }
    }

}