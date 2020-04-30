﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float waypointGizmoRadius = 0.3f;


        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }

        }

        public int GetNextIndex(int index)
        {
            if (index + 1 == transform.childCount)
                return 0;
            return index + 1;
        }
        public Vector3 GetWaypoint(int index)
        {
            return transform.GetChild(index).position;
        }
    }

}