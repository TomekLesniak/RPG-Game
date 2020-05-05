using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float _timeBetweenAttacks = 1.0f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;

        private float _timeSinceLastAttack = 0;
        private Health _target;
        private Weapon currentWeapon = null;

        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }

        
        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target == null) return;
            if (_target.IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }

        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);

        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
                return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack >= _timeBetweenAttacks)
            {
                //This will trigger Hit() event(below)
                TriggerAttack();
                _timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation event
        private void Hit()
        {
            if (_target == null) { return; }

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, _target);
            }
            else
            {
                _target.TakeDamage(currentWeapon.GetWeaponDamage());
            }
        }

        // Animation event
        private void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) <= currentWeapon.GetWeaponRange();
        }

        public void Attack(GameObject target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = target.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttackTrigger();
            _target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttackTrigger()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
        }
    }
}

