using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private bool isHoming = false;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float projectileLifeTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit = null;
        [SerializeField] private float lifeTimeAfterHit = 2f;

        private Health target = null;
        private GameObject instigator = null;
        private float damage = 0; // Initialized from bow

        private void Start()
        {
            if (!isHoming)
            {
                transform.LookAt(GetAimLocation());
            }
        }

        private void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(this.gameObject, projectileLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsuleCollider = target.GetComponent<CapsuleCollider>();
            if (targetCapsuleCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            //IF HIT SOMETHING OTHER
            if (other.GetComponent<Health>() != target)
            {
                Destroy(this.gameObject);
                return;
            }

            if (target.IsDead())
            {
                target.GetComponent<CapsuleCollider>().enabled = false;
            }

            Instantiate(hitEffect, GetAimLocation(), Quaternion.identity); // TODO: might change to transform.position
            target.TakeDamage(instigator, damage);
            speed = 0;

            foreach (var toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(this.gameObject, lifeTimeAfterHit);
        }
    }

}
