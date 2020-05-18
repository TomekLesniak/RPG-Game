using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab = null;


        public void Spawn(float damage)
        {
            DamageText instance = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
            instance.SetValue(damage);
        }
    }

}