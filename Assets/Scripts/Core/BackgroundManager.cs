using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace VisualNovel.Core
{
    /// <summary>
    /// Manages background images and transitions
    /// </summary>
    public class BackgroundManager : MonoBehaviour
    {
        [System.Serializable]
        public class BackgroundData
        {
            public string backgroundName;
            public Sprite backgroundSprite;
        }

        [Header("Background Settings")]
        public Image backgroundImage;
        public BackgroundData[] backgrounds;
        public float transitionDuration = 1f;

        private Sprite currentBackground;
        private Coroutine transitionCoroutine;

        /// <summary>
        /// Changes the background with a fade transition
        /// </summary>
        public void ChangeBackground(string backgroundName, bool immediate = false)
        {
            BackgroundData bgData = System.Array.Find(backgrounds, bg => bg.backgroundName == backgroundName);
            
            if (bgData == null)
            {
                Debug.LogWarning($"Background not found: {backgroundName}");
                return;
            }

            if (immediate)
            {
                backgroundImage.sprite = bgData.backgroundSprite;
                currentBackground = bgData.backgroundSprite;
            }
            else
            {
                if (transitionCoroutine != null)
                {
                    StopCoroutine(transitionCoroutine);
                }
                transitionCoroutine = StartCoroutine(TransitionBackground(bgData.backgroundSprite));
            }
        }

        /// <summary>
        /// Smoothly transitions between backgrounds
        /// </summary>
        private IEnumerator TransitionBackground(Sprite newBackground)
        {
            // Create temporary image for cross-fade
            GameObject tempObj = new GameObject("TempBackground");
            tempObj.transform.SetParent(backgroundImage.transform.parent);
            tempObj.transform.SetSiblingIndex(backgroundImage.transform.GetSiblingIndex());
            
            Image tempImage = tempObj.AddComponent<Image>();
            tempImage.sprite = newBackground;
            tempImage.rectTransform.anchorMin = Vector2.zero;
            tempImage.rectTransform.anchorMax = Vector2.one;
            tempImage.rectTransform.sizeDelta = Vector2.zero;

            CanvasGroup tempCG = tempObj.AddComponent<CanvasGroup>();
            tempCG.alpha = 0f;

            // Fade in new background
            float elapsed = 0f;
            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                tempCG.alpha = elapsed / transitionDuration;
                yield return null;
            }

            // Replace main background
            backgroundImage.sprite = newBackground;
            currentBackground = newBackground;
            
            Destroy(tempObj);
        }

        /// <summary>
        /// Fades the background to black
        /// </summary>
        public void FadeToBlack(float duration = 1f)
        {
            StartCoroutine(FadeBackgroundCoroutine(0f, duration));
        }

        /// <summary>
        /// Fades the background in from black
        /// </summary>
        public void FadeFromBlack(float duration = 1f)
        {
            StartCoroutine(FadeBackgroundCoroutine(1f, duration));
        }

        private IEnumerator FadeBackgroundCoroutine(float targetAlpha, float duration)
        {
            CanvasGroup cg = backgroundImage.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = backgroundImage.gameObject.AddComponent<CanvasGroup>();
            }

            float startAlpha = cg.alpha;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
                yield return null;
            }

            cg.alpha = targetAlpha;
        }
    }
}
