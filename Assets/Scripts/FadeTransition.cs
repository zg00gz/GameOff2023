using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    [RequireComponent(typeof(CanvasGroup))]
    public class FadeTransition : MonoBehaviour
    {
        public float Duration = 1.5f;
        public float IntroDelay = 1.0f;
        public Coroutine CurrentRoutine { private set; get; } = null;

        private CanvasGroup m_CanvasGroup = null;
        private float m_Alpha = 0.0f;

        private void Awake()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }

        public void IntroStartFadeOut()
        {
            StopAllCoroutines();
            CurrentRoutine = StartCoroutine(IntroFadeOut(Duration));
        }

        public void StartFadeIn()
        {
            StopAllCoroutines();
            CurrentRoutine = StartCoroutine(FadeIn(Duration));
        }

        public void StartFadeOut()
        {
            StopAllCoroutines();
            CurrentRoutine = StartCoroutine(FadeOut(Duration));
        }

        private IEnumerator FadeIn(float duration)
        {
            float elapsedTime = 0.0f;
            SetAlpha(0.0f);

            while (m_Alpha <= 1.0f)
            {
                SetAlpha(elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator FadeOut(float duration)
        {
            float elapsedTime = 0.0f;
            SetAlpha(1.0f);

            while (m_Alpha >= 0.0f)
            {
                SetAlpha(1 - (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator IntroFadeOut(float duration)
        {
            float elapsedTime = 0.0f;
            SetAlpha(1.0f);

            yield return new WaitForSeconds(IntroDelay);

            while (m_Alpha >= 0.0f)
            {
                SetAlpha(1 - (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private void SetAlpha(float value)
        {
            m_Alpha = value;
            m_CanvasGroup.alpha = m_Alpha;
        }
    }

}


