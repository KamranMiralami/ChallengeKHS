using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : SingletonBehaviour<GameManager>
{
    public Transform Crosshair;
    [SerializeField] PlayerController currentPlayer;
    public PlayerController CurrentPlayer => currentPlayer;
    private void Start()
    {
        InventoryManager.Instance.Initialize();
    }
}
