using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Health target = null;


    private void Update()
    {
        if (target == null) return;

        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void SetTarget(Health target)
    {
        this.target = target;
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
}
