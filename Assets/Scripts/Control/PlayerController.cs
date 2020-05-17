using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        //[SerializeField]
        //private Transform _target;


        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] cursorMappings = null;
        [SerializeField] private float MaxNavMeshProjectionDistance = 1.0f;
        [SerializeField] private float maxNavPathLength = 40f;

        void Update()
        {
            if (InteractWithUI())
            {
                SetCursor(CursorType.UserInterface);
                return;
            }
            if (GetComponent<Health>().IsDead()) // Can`t do anything if player is dead
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;

            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current == null) return false;

            return EventSystem.current.IsPointerOverGameObject(); // USER INTERFACE GAME OBJECT!
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false; // Nothing handled raycstable;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance; //Vector3.Distance(hits[i].transform.position, this.transform.position);
            }

            Array.Sort(distances, hits);
            return hits;
        }


        private bool InteractWithMovement()
        {
            
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                   // GetComponent<Animator>().SetTrigger("stopAttack");
                }

                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            NavMeshHit navMeshHit;
            target = new Vector3();

            if (!hasHit)
            {
                return false;
            }

            if (NavMesh.SamplePosition(hit.point, out navMeshHit, MaxNavMeshProjectionDistance, NavMesh.AllAreas))
            {
                target = navMeshHit.position;

                NavMeshPath path = new NavMeshPath();
                bool hasPath = NavMesh.CalculatePath(this.transform.position, target, NavMesh.AllAreas, path);

                if (!hasPath) return false;
                if (path.status != NavMeshPathStatus.PathComplete) return false;
                if (GetPathLength(path) > maxNavPathLength) return false;


                return true;
            }

            return false;

        }

        public void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (var mapping in cursorMappings)
            {
                if (mapping.type == type)
                    return mapping;
            }

            return cursorMappings[0];
        }

        private float GetPathLength(NavMeshPath path)
        {
            Vector3[] pathVectors = path.corners;
            float distance = 0;
            if (pathVectors.Length < 2) return distance;

            for (int i = 0; i < pathVectors.Length - 1; i++)
            {
                distance += Vector3.Distance(pathVectors[i], pathVectors[i + 1]);
            }

            print(distance);
            return distance;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}