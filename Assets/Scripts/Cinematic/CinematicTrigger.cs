using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool wasPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!wasPlayed && other.tag == "Player")
            {
                GetComponent<PlayableDirector>().Play();
                wasPlayed = true;
            }
        }
        
    }

}