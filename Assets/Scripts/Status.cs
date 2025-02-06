using UnityEngine;
namespace Descent{
public class Status : MonoBehaviour
    {
        // Fuel settings
        [Header("Fuel Settings")]
        [SerializeField] private float maxFuel = 100f;
        [SerializeField] private float fuelDepletionRate = 5f;

        // Visual fuel indicator
        [Header("Fuel Indicator")]
        [SerializeField] private GameObject fuelBarPrefab;
        [SerializeField] private Transform fuelPanel;
        [SerializeField] private float spacing = 5f;
        [SerializeField] private Vector2 barOffset = new Vector2(0, 0);

        // Death settings
        [Header("Death")]
        [SerializeField] private GameObject deathEffect;

        private float currentFuel;
        private GameObject[] fuelBars;

        void Start()
        {
            currentFuel = maxFuel;
            CreateFuelBars();
            UpdateFuelBars();
        }

        void Update()
        {
            if (currentFuel > 0)
            {
                UpdateFuel();
            }
            else
            {
                Die();
            }
        }

        void CreateFuelBars()
        {
            fuelBars = new GameObject[10];
            float barWidth = fuelBarPrefab.GetComponent<RectTransform>().rect.width;

            for (int i = 0; i < 10; i++)
            {
                fuelBars[i] = Instantiate(fuelBarPrefab, fuelPanel);
                float xPosition = i * (barWidth + spacing) + barOffset.x;
                fuelBars[i].transform.localPosition = new Vector3(xPosition, barOffset.y, 0);
            }
        }

        void UpdateFuel()
        {
            currentFuel -= fuelDepletionRate * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
            UpdateFuelBars();
        }

        void UpdateFuelBars()
        {
            int activeBars = Mathf.CeilToInt(currentFuel / 10); // Change from Floor to Ceil to fix the off-by-one issue
            for (int i = 0; i < 10; i++)
            {
                if (fuelBars[i] != null)
                {
                    fuelBars[i].SetActive(i < activeBars);
                }
            }
        }

        public void AddFuel(float amount)
        {
            currentFuel = Mathf.Clamp(currentFuel + amount, 0, maxFuel);
            UpdateFuelBars();
        }

        public void ReduceFuel(float percent)
        {
            float reduction = maxFuel * (percent / 100f);
            currentFuel = Mathf.Clamp(currentFuel - reduction, 0, maxFuel);
            UpdateFuelBars();
        }

        void Die()
        {
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
            Debug.Log("Player died due to fuel depletion!");
        }
    }
}
