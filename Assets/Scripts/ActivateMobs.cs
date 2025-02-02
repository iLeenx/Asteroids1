using UnityEngine;

namespace Descent
{
    /// <summary>
    /// this script is for alpha only before emplemnting the mobs spawner
    /// </summary>
    public class ActivateMobs : MonoBehaviour
    {
        public GameObject targetObject;  // The game object to activate

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))  // Adjust tag as needed
            {
                if (targetObject != null)
                {
                    targetObject.SetActive(true);  // Activates the target game object
                }
            }
        }
    }
}