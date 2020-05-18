using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text damageText = null;

        public void DestroyAfter()
        {
            Destroy(this.gameObject);
        }

        public void SetValue(float damage)
        {
            damageText.text = String.Format("{0:0}", damage);
        }
    }
}
