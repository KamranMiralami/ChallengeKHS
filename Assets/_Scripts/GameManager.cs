using SFXSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public Transform Crosshair;
    [SerializeField] PlayerController currentPlayer;
    public PlayerController CurrentPlayer => currentPlayer;
    private void Start()
    {
        InventoryManager.Instance.Initialize();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
