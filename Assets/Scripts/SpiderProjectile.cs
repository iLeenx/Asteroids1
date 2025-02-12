using UnityEngine;

namespace Descent
{
    public class SpiderProjectile : MonoBehaviour
    {
        public float lifetime = 5f; // Time before the projectile is destroyed
        public AudioClip shootSound; // Sound to play when shooting
        public AudioClip hitPlayerSound; // Sound when hitting the player
        public AudioClip missSound; // Sound when missing (hitting anything other than the player)

        private AudioSource audioSource; // Audio source component

        private void Start()
        {
            // Attach an AudioSource if not already added
            audioSource = gameObject.AddComponent<AudioSource>();

            // Play the shooting sound
            PlaySound(shootSound);

            // Destroy the projectile after the set time
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Projectile hit the player!");

                // Play the sound for hitting the player
                PlaySound(hitPlayerSound);

                // Destroy the projectile
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Projectile missed and hit: " + other.tag);

                // Play the miss sound when hitting anything other than the player
                PlaySound(missSound);

                // Destroy the projectile
                Destroy(gameObject);
            }
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
