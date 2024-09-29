using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject canvas;
    public GameObject PlayerPrefab;

    public void Awake()
    {
        canvas.SetActive(false);
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            canvas.SetActive(true);
            Runner.Spawn(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
