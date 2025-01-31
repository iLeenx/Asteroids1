using UnityEngine;
namespace Descent
{
    public class Status : MonoBehaviour
    {
        [Header("Fuel Settings")]
        [SerializeField] private float maxFuel = 100f;
        [SerializeField] private float fuelDepletionRate = 5f;

        [Header("Fuel Indicator")]
        [SerializeField] private GameObject fuelBarPrefab;
        [SerializeField] private Transform fuelPanel;
        [SerializeField] private float spacing = 5f;
        [SerializeField] private Vector2 barOffset = new Vector2(0, 0);

        [Header("Death")]
        [SerializeField] private GameObject deathEffect;

        private float currentFuel;
        private GameObject[] fuelBars;
        private int lastActiveBarIndex;

        void Start()
        {
            currentFuel = maxFuel;
            InitializeFuelBars();
        }

        void InitializeFuelBars()
        {
            fuelBars = new GameObject[10];
            float barWidth = fuelBarPrefab.GetComponent<RectTransform>().rect.width;

            for (int i = 0; i < 10; i++)
            {
                fuelBars[i] = Instantiate(fuelBarPrefab, fuelPanel);
                float xPos = i * (barWidth + spacing) + barOffset.x;
                fuelBars[i].transform.localPosition = new Vector3(xPos, barOffset.y, 0);
            }

            lastActiveBarIndex = 9; // Start with all bars active
        }

        void Update()
        {
            if (currentFuel > 0)
            {
                float previousFuel = currentFuel;
                currentFuel -= fuelDepletionRate * Time.deltaTime;
                currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);

                // Calculate active bars using CEILING to detect exact thresholds
                int previousBars = Mathf.CeilToInt(previousFuel / 10);
                int currentBars = Mathf.CeilToInt(currentFuel / 10);

                // Only remove bars when crossing exact 10% thresholds downward
                if (currentBars < previousBars)
                {
                    RemoveFuelBar(previousBars - 1); // Remove the appropriate bar
                }
            }
            else
            {
                HandleDeath();
            }
        }

        void RemoveFuelBar(int barIndex)
        {
            if (barIndex >= 0 && barIndex < fuelBars.Length && fuelBars[barIndex] != null)
            {
                Destroy(fuelBars[barIndex]);
                lastActiveBarIndex--;
            }
        }

        public void AddFuel(float amount)
        {
            currentFuel = Mathf.Clamp(currentFuel + amount, 0, maxFuel);
            int activeBars = Mathf.CeilToInt(currentFuel / 10);

            // Restore missing bars from left to right
            for (int i = 0; i < activeBars; i++)
            {
                if (fuelBars[i] == null)
                {
                    fuelBars[i] = Instantiate(fuelBarPrefab, fuelPanel);
                    float xPos = i * (fuelBarPrefab.GetComponent<RectTransform>().rect.width + spacing) + barOffset.x;
                    fuelBars[i].transform.localPosition = new Vector3(xPos, barOffset.y, 0);
                    lastActiveBarIndex = i;
                }
            }
        }

        public void ReduceFuel(float percent)
        {
            currentFuel = Mathf.Clamp(currentFuel - (maxFuel * (percent / 100)), 0, maxFuel);
            int activeBars = Mathf.CeilToInt(currentFuel / 10);

            // Remove bars if necessary
            while (lastActiveBarIndex >= activeBars)
            {
                RemoveFuelBar(lastActiveBarIndex);
            }
        }

        void HandleDeath()
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}