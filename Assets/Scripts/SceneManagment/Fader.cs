using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private float time = 1f;
        
        private CanvasGroup canvasGroup;
        private float deltaAlpha;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;

            StartCoroutine(FadeOutIn());
        }

        IEnumerator FadeOutIn()
        {
            yield return FadeOut(3f);
            yield return FadeIn(3f);
        }

        public IEnumerator FadeOut(float time)
        {
            deltaAlpha = Time.deltaTime / time;
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += deltaAlpha;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            deltaAlpha = Time.deltaTime / time;
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= deltaAlpha;
                yield return null;
            }
        }
    }

}
