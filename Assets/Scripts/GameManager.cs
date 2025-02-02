using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void ResetGame()
    {
        ResetCamera();
        ResetPlayer();
        ResetUpgradesToDefualt();
        ResetMobs();
    }
    //
    public void ResetCamera()
    {
        // camera goes back to it's og position
    }
    public void ResetPlayer()
    {
        // player goes back to it's og position
    }
    public void ResetUpgradesToDefualt()
    {
        // 
    }
    public void ResetMobs()
    {
        // reset waves
    }
    
}
