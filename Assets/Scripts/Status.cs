using UnityEngine;
namespace Descent{
public class Status : MonoBehaviour
    {
        // Fuel settings
        [Header("Fuel Settings")]
        [SerializeField] private float maxFuel = 100f;      // Maximum fuel capacity
        [SerializeField] private float fuelDepletionRate = 5f; // Fuel consumption rate per second

        // Visual fuel indicator
        [Header("Fuel Indicator")]
        [SerializeField] private GameObject fuelBarPrefab;  // Prefab for a single fuel bar
        [SerializeField] private Transform fuelPanel;       // Panel for displaying the fuel bars
        [SerializeField] private float spacing = 5f;        // Spacing between bars
        [SerializeField] private Vector2 barOffset = new Vector2(0, 0); // Position adjustment for bars

        // Death settings
        [Header("Death")]
        [SerializeField] private GameObject deathEffect;    // Effect to play on death

        private float currentFuel;           // Current fuel level
        private GameObject[] fuelBars;       // Array to hold the fuel bars
        private int lastActiveBarIndex = 9;  // Index of the last active bar (starts at 9 as it's array indexing)

        void Start()
        {
            currentFuel = maxFuel; // Start with maximum fuel
            CreateFuelBars();      // Create initial fuel bars
        }

        void Update()
        {
            if (currentFuel > 0)
            {
                UpdateFuel(); // Update fuel every frame
            }
            else
            {
                Die(); // Trigger death if fuel runs out
            }
        }

        // Create initial fuel bars
        void CreateFuelBars()
        {
            fuelBars = new GameObject[10]; // Create 10 bars

            float barWidth = fuelBarPrefab.GetComponent<RectTransform>().rect.width;

            for (int i = 0; i < 10; i++)
            {
                // Create a new bar and add it to the panel
                fuelBars[i] = Instantiate(fuelBarPrefab, fuelPanel);
            
                // Calculate its position based on its order
                float xPosition = i * (barWidth + spacing) + barOffset.x;
                fuelBars[i].transform.localPosition = new Vector3(xPosition, barOffset.y, 0);
            }
        }

        // Update fuel over time
        void UpdateFuel()
        {
            float previousFuel = currentFuel;
            currentFuel -= fuelDepletionRate * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);

            // Check if we've crossed a 10% threshold
            int previousBars = Mathf.FloorToInt(previousFuel / 10);
            int currentBars = Mathf.FloorToInt(currentFuel / 10);

            if (currentBars < previousBars)
            {
                RemoveBar(previousBars - 1); // Remove the rightmost bar
            }
        }

        // Remove a specific bar
        void RemoveBar(int barIndex)
        {
            if (barIndex >= 0 && barIndex < fuelBars.Length)
            {
                if (fuelBars[barIndex] != null)
                {
                    Destroy(fuelBars[barIndex]); // Destroy the bar
                    lastActiveBarIndex--;
                }
            }
        }

        // Add fuel (when collecting a pick-up)
        public void AddFuel(float amount)
        {
            currentFuel = Mathf.Clamp(currentFuel + amount, 0, maxFuel);
        
            int neededBars = Mathf.FloorToInt(currentFuel / 10);

            // Recreate any missing bars
            for (int i = 0; i <= neededBars; i++)
            {
                if (fuelBars[i] == null)
                {
                    fuelBars[i] = Instantiate(fuelBarPrefab, fuelPanel);
                    float xPosition = i * (fuelBarPrefab.GetComponent<RectTransform>().rect.width + spacing) + barOffset.x;
                    fuelBars[i].transform.localPosition = new Vector3(xPosition, barOffset.y, 0);
                    lastActiveBarIndex = i;
                }
            }
        }

        // Reduce fuel (when hitting a spider web)
        public void ReduceFuel(float percent)
        {
            float reduction = maxFuel * (percent / 100f);
            currentFuel = Mathf.Clamp(currentFuel - reduction, 0, maxFuel);
        
            int currentBars = Mathf.FloorToInt(currentFuel / 10);
        
            // Remove excess bars
            while (lastActiveBarIndex > currentBars)
            {
                RemoveBar(lastActiveBarIndex);
            }
        }

        // Handle death
        void Die()
        {
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject); // Destroy the player object
            Debug.Log("Player died due to fuel depletion!");
        }
    }
}