using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descent
{
    public class GameManager : MonoBehaviour
    {
        public void ReloadGameScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// reset the game 
        /// </summary>
        // public Transform player;
        // public Vector3 playerStartPosition;
        // public WeaponController weapon;
        // public CameraMovement cameraController;
        // public Vector3 cameraStartPosition;

        // // Prefabs for respawning
        // public GameObject[] enemyPrefabs;
        // public Vector3[] enemyStartPositions;

        // public GameObject[] triggerPrefabs;
        // public Vector3[] triggerStartPositions;

        // private void Start()
        // {
        //     // Store initial positions
        //     playerStartPosition = player.position;
        //     cameraStartPosition = cameraController.transform.position;

        //     // Store original enemy positions
        //     GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Mob");
        //     enemyPrefabs = new GameObject[existingEnemies.Length];
        //     enemyStartPositions = new Vector3[existingEnemies.Length];

        //     for (int i = 0; i < existingEnemies.Length; i++)
        //     {
        //         enemyPrefabs[i] = existingEnemies[i];
        //         enemyStartPositions[i] = existingEnemies[i].transform.position;
        //     }

        //     // Store original trigger positions
        //     GameObject[] existingTriggers = GameObject.FindGameObjectsWithTag("WaveTrigger");
        //     triggerPrefabs = new GameObject[existingTriggers.Length];
        //     triggerStartPositions = new Vector3[existingTriggers.Length];

        //     for (int i = 0; i < existingTriggers.Length; i++)
        //     {
        //         triggerPrefabs[i] = existingTriggers[i];
        //         triggerStartPositions[i] = existingTriggers[i].transform.position;
        //     }
        // }

        // public void ResetGameState()
        // {
        //     // Reset player position
        //     player.position = playerStartPosition;

        //     // Destroy all enemies
        //     foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Mob"))
        //     {
        //         Destroy(enemy);
        //     }

        //     // Destroy all triggers
        //     foreach (GameObject trigger in GameObject.FindGameObjectsWithTag("WaveTrigger"))
        //     {
        //         Destroy(trigger);
        //     }

        //     // Respawn enemies
        //     for (int i = 0; i < enemyPrefabs.Length; i++)
        //     {
        //         Instantiate(enemyPrefabs[i], enemyStartPositions[i], Quaternion.identity);
        //     }

        //     // Respawn triggers
        //     for (int i = 0; i < triggerPrefabs.Length; i++)
        //     {
        //         Instantiate(triggerPrefabs[i], triggerStartPositions[i], Quaternion.identity);
        //     }

        //     // Reset weapon upgrades
        //     //weapon.ResetUpgrades();

        //     // Reset camera position with y-axis constraint
        //     Vector3 newCameraPosition = cameraStartPosition;
        //     newCameraPosition.y = -10;
        //     cameraController.transform.position = newCameraPosition;
        // }       
    }
}