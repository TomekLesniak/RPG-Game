using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private float _timeBetweenAttacks = 1.0f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private WeaponConfig _defaultWeapon = null;

        private float _timeSinceLastAttack = 0;
        private Health _target;
        WeaponConfig currentWeaponConfig;
        private LazyValue<Weapon> currentWeapon;

        private void Awake()
        {
            currentWeaponConfig = _defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(_defaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
            //AttachWeapon(currentWeaponConfig);
            // Else saving system will equip a weaponConfig
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

        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            currentWeapon.value = AttachWeapon(weaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig weaponConfig)
        {
            Animator animator = GetComponent<Animator>();
            return weaponConfig.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return _target;
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

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }


            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, _target, this.gameObject, damage);
            }
            else
            { 
                _target.TakeDamage(this.gameObject, damage);
            }
        }

        // Animation event
        private void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) <= currentWeaponConfig.GetWeaponRange();
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            WeaponConfig weaponConfig = UnityEngine.Resources.Load<WeaponConfig>((string) state);
            EquipWeapon(weaponConfig);
        }

        
    }
}

