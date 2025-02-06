using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

namespace Descent
{
    public class WaveCardSwapper : MonoBehaviour
    {
        [Header("Card References")]
        public RectTransform currentCard, nextCard;
        public TextMeshProUGUI currentCardText, nextCardText;
        public AudioSource waveUpSound;
        public Image currentCardPanel, nextCardPanel;

        [Header("Animation Settings")]
        public float animationDuration = 0.5f;
        public Vector2 moveOffset = new Vector2(0, 100);
        public AnimationCurve positionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);
        [Range(0.1f, 1f)] public float cardScaleFactor = 0.9f;

        [Header("Flicker Settings")]
        public Color flickerColor = Color.red;
        public Color textFlickerColor = Color.white;
        public float flickerSpeed = 15f;

        private int currentWave = 0;
        private Vector2 originalPosition;
        private bool isAnimating = false;
        private Canvas currentCardCanvas, nextCardCanvas;
        private Coroutine flickerCoroutine;
        private Color originalPanelColor, originalTextColor;

        void Start()
        {
            originalPosition = currentCard.anchoredPosition;
            originalPanelColor = currentCardPanel.color;
            originalTextColor = currentCardText.color;

            currentCardText.text = currentWave.ToString("D2");
            nextCardText.text = (currentWave + 1).ToString("D2");

            currentCardCanvas = InitCanvas(currentCard, 1);
            nextCardCanvas = InitCanvas(nextCard, 0);

            ResetCardState();
        }

        Canvas InitCanvas(RectTransform card, int sortingOrder)
        {
            Canvas canvas = card.gameObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = sortingOrder;
            return canvas;
        }

        void ResetCardState()
        {
            // Reset current card
            currentCard.anchoredPosition = originalPosition;
            currentCard.localScale = Vector3.one;
            currentCardCanvas.sortingOrder = 1;
            currentCardPanel.color = originalPanelColor;
            currentCardText.color = originalTextColor;
            currentCard.gameObject.SetActive(true);

            // Reset next card
            nextCard.anchoredPosition = originalPosition - moveOffset;
            nextCard.localScale = Vector3.one * cardScaleFactor;
            nextCardCanvas.sortingOrder = 0;
            nextCardPanel.color = originalPanelColor;
            nextCardText.color = originalTextColor;
            nextCard.gameObject.SetActive(false);
        }

        public void SwapWave()
        {
            if (!isAnimating) StartCoroutine(AnimateCardSwap());
        }

        private IEnumerator AnimateCardSwap()
        {
            if (!UIManager.Instance.IsMenuActive)
            {
                isAnimating = true;
                int nextWave = currentWave + 1;

                // Activate and prepare NEW CARD
                nextCard.gameObject.SetActive(true);
                nextCardCanvas.sortingOrder = 2;
                nextCardText.text = nextWave.ToString("D2");

                if (waveUpSound) waveUpSound.Play();

                // Start flicker ONLY on new card
                if (flickerCoroutine != null) StopCoroutine(flickerCoroutine);
                flickerCoroutine = StartCoroutine(FlickerCard(nextCardPanel, nextCardText));

                float elapsed = 0;
                Vector2 nextStartPos = originalPosition - moveOffset;

                while (elapsed < animationDuration)
                {
                    float progress = positionCurve.Evaluate(elapsed / animationDuration);
                    float scaleProgress = scaleCurve.Evaluate(elapsed / animationDuration);

                    // Animate current card out (no visual changes)
                    currentCard.anchoredPosition = Vector2.Lerp(originalPosition, originalPosition + moveOffset, progress);
                    currentCard.localScale = Vector3.Lerp(Vector3.one, Vector3.one * cardScaleFactor, progress);

                    // Animate new card in (with flicker)
                    nextCard.anchoredPosition = Vector2.Lerp(nextStartPos, originalPosition, progress);
                    nextCard.localScale = Vector3.Lerp(Vector3.one * cardScaleFactor, Vector3.one, progress);

                    elapsed += Time.deltaTime;
                    yield return null;
                }

                // Stop flicker and reset new card's appearance
                if (flickerCoroutine != null)
                {
                    StopCoroutine(flickerCoroutine);
                    nextCardPanel.color = originalPanelColor;
                    nextCardText.color = originalTextColor;
                }

                // Swap references
                (currentCard, nextCard) = (nextCard, currentCard);
                (currentCardText, nextCardText) = (nextCardText, currentCardText);
                (currentCardCanvas, nextCardCanvas) = (nextCardCanvas, currentCardCanvas);
                (currentCardPanel, nextCardPanel) = (nextCardPanel, currentCardPanel);

                currentWave = nextWave;
                ResetCardState();
                isAnimating = false;
            }
        }

        private IEnumerator FlickerCard(Image targetPanel, TextMeshProUGUI targetText)
        {
            while (true)
            {
                float lerp = Mathf.PingPong(Time.time * flickerSpeed, 1);
                targetPanel.color = Color.Lerp(originalPanelColor, flickerColor, lerp);
                targetText.color = Color.Lerp(originalTextColor, textFlickerColor, lerp);
                yield return null;
            }
        }
    }
}