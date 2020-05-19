using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig _weaponConfig = null;
        [SerializeField] private float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }

        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(_weaponConfig);
            StartCoroutine(RespawnPickup(respawnTime));
        }

        private IEnumerator RespawnPickup(float seconds)
        {
            HidePickup();
            yield return new WaitForSeconds(seconds);
            ShowPickup();
        }

        private void ShowPickup()
        {
            GetComponent<SphereCollider>().enabled = true;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        private void HidePickup()
        {
            GetComponent<SphereCollider>().enabled = false;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.GetComponent<Fighter>());
            }

            return true;
        }
    }

}
