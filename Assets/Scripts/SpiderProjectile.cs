using UnityEngine;
namespace Descent
{

    public class SpiderProjectile : MonoBehaviour
    {
        [Header("Web Effects")]
        [SerializeField] private float fuelReduction = 5f; // 5% reduction
        [SerializeField] private float slowAmount = 30f; // 30% speed reduction
        [SerializeField] private float slowDuration = 1f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                // Get player components
                Status playerStatus = collision.GetComponent<Status>();
                WebDebuff webDebuff = collision.GetComponent<WebDebuff>();

                if (playerStatus != null)
                {
                    playerStatus.ReduceFuel(fuelReduction);
                }

                if (webDebuff != null)
                {
                    webDebuff.ApplyWebEffect(slowAmount, slowDuration);
                }

                Destroy(gameObject); // Destroy web after impact
            }
        }
    }
}