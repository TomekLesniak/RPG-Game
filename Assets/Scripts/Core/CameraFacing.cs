using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        //<summary>
        //Script allow UI elements over characters always facing towards camera
        //</summary>

        private void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}