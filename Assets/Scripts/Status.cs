using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;

// Make fuel sounds the more it deplates

namespace Descent{

    public class Status : MonoBehaviour
    {
        // Singleton instance
        public static Status Instance { get; private set; }

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

        [SerializeField] private GameObject LoseScreen;
        [SerializeField] private GameObject HUD;

        [SerializeField] private GameObject player;

        public float currentFuel;
        public GameObject[] fuelBars;

        private bool isDead = false; // Add this to prevent multiple executions

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
            else if (!isDead) // Prevent multiple coroutines
            {
                StartCoroutine(Die());
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
            try
            {
                if (fuelBars == null || fuelBars.Length == 0)
                {
                    // Debug.LogWarning("[Status] fuelBars array is null or empty. Make sure it is assigned in the Inspector.");
                    return;
                }

                int activeBars = Mathf.CeilToInt(currentFuel / 10); // Using Ceil to fix off-by-one issue

                for (int i = 0; i < fuelBars.Length; i++) // Use `fuelBars.Length` to prevent out-of-bounds errors
                {
                    if (fuelBars[i] != null)
                    {
                        fuelBars[i].SetActive(i < activeBars);
                    }
                    else
                    {
                        // Debug.LogWarning($"[Status] fuelBars[{i}] is NULL. Check the Inspector setup.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[Status] Exception in UpdateFuelBars: {ex.Message}\n{ex.StackTrace}");
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

        IEnumerator Die()
        {

            if (isDead) yield break; // Stop if already dead
            isDead = true;

            // Play VFX twice with a slight delay in between (optional)
            OB_VFX.Instance.PlayVFX("CFXR2 Skull Head Alt", player.transform.position, Quaternion.identity, 1f, 1f, 0f);
            OB_VFX.Instance.PlayVFX("DeathEffect", player.transform.position, Quaternion.identity, 1f, 1f, 0f);
            Debug.Log("Player died due to fuel depletion!");

            //OB_POSTFX.Instance.p

            player.SetActive(false);

            yield return new WaitForSeconds(2f); // Adjust delay as needed

            LoseScreen.SetActive(true);
        }
    }
}
