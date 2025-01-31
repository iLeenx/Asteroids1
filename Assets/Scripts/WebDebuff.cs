
using UnityEngine;
using System.Collections;

namespace Descent
{
    public class WebDebuff : MonoBehaviour
    {
        private float originalSpeed;
        private PlayerMovement playerMovement;
        private bool isSlowed = false;

        void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            originalSpeed = playerMovement.moveSpeed;
        }

        public void ApplyWebEffect(float slowAmount, float duration)
        {
            if (!isSlowed)
            {
                StartCoroutine(SlowEffect(slowAmount, duration));
            }
        }

        private IEnumerator SlowEffect(float slowPercent, float duration)
        {
            isSlowed = true;
            playerMovement.moveSpeed = originalSpeed * (1 - slowPercent / 100);

            yield return new WaitForSeconds(duration);

            playerMovement.moveSpeed = originalSpeed;
            isSlowed = false;
        }
    }
}