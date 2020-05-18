using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }
        void Update()
        {
            if (fighter.GetTarget() != null)
            {
                GetComponent<Text>().text = String.Format("{0:0}/{1:0}", fighter.GetTarget().GetHealthPoints(), fighter.GetTarget().GetMaxHealthPoints());
            }
            else
            {
                GetComponent<Text>().text = "NULL";
            }
            
        }
    }

}