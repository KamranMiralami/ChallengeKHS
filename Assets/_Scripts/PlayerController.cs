using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using static Equipable;
using static EquipLocation;

public class PlayerController : MonoBehaviour
{
    // movement and input variables
    public Camera Camera;
    [SerializeField] private float sensitivity;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    PlayerInputActions input;
    Vector2 lookInput;
    Vector2 moveInput;
    float yaw;
    float pitch;

    // equipment variables
    [SerializeField] private Hand leftHand;
    [SerializeField] private Hand rightHand;
    [SerializeField] private Transform head;

    bool isPressedL;
    bool isPressedR;

    // general variables
    bool canPerformActions = true;
    public void DisableMovement()
    {
        input.Player.Disable();
    }
    public void EnableMovement()
    {
        input.Player.Enable();
    }
    void Awake()
    {
        leftHand.Initialize(this);
        rightHand.Initialize(this);

        input = new PlayerInputActions();
         
        input.Player.Look.performed += val => lookInput = val.ReadValue<Vector2>();
        input.Player.Look.canceled += _ => lookInput = Vector2.zero;

        input.Player.Move.performed += val => moveInput = val.ReadValue<Vector2>();
        input.Player.Move.canceled += _ => moveInput = Vector2.zero;

        input.Player.Inventory.performed += _ => InventoryButtonClicked();

        input.Player.Crouch.performed += val => ChangeGunMode();
    }

    private void ChangeGunMode()
    {
        var item = leftHand.ItemInHand;
        if(item is Gun gunL)
        {
            gunL.ChangeMode();
            return;
        }
        item = rightHand.ItemInHand;
        if(item is Gun gunR)
        {
            gunR.ChangeMode();
            return;
        }
    }

    void InventoryButtonClicked()
    {
        if (InventoryManager.Instance.IsOpen)
        {
            InventoryManager.Instance.Close();
            LockMouse();
            canPerformActions = true;
        }
        else
        {
            InventoryManager.Instance.Open();
            UnLockMouse();
            canPerformActions = false;
        }
    }

    private void ResetInputs()
    {
        leftHand.ResetItem();
        rightHand.ResetItem();
    }
    public void EquipItem(LocationType locationType, EquipableType itemType, int numberOfUses) 
    {
        switch (locationType)
        {
            case LocationType.LeftHand:
                if (leftHand.HasItem()) break;
                Debug.Log("Equipped item in left hand");
                leftHand.EquipItem(itemType, numberOfUses);
                break;
            case LocationType.RightHand:
                if (rightHand.HasItem()) break;
                Debug.Log("Equipped item in right hand");
                rightHand.EquipItem(itemType, numberOfUses);
                break;
            case LocationType.Head:
                Debug.Log("Equipped item on head");
                break;
            default:
                Debug.LogError("Invalid location to equip item");
                break;
        }
    }
    public void UnEquipItem(LocationType localtionType)
    {
        switch (localtionType)
        {
            case LocationType.LeftHand:
                if (!leftHand.HasItem()) return;
                leftHand.UnEquipItem();
                break;
            case LocationType.RightHand:
                if (!rightHand.HasItem()) return;
                rightHand.UnEquipItem();
                break;
            case LocationType.Head:
                break;
            default:
                break;
        }
    }

    private void UsedRightHand()
    {
        rightHand.UseItem();
    }

    private void UsedLeftHand()
    {
        leftHand.UseItem();
    }

    void OnEnable()
    {
        EnableMovement();
    }
    void OnDisable()
    {
        DisableMovement();
    }

    void Start()
    {
        LockMouse();
        yaw = transform.eulerAngles.y;
        pitch = Camera.transform.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f;
    }
    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void UnLockMouse()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    void Update()
    {
        if (!canPerformActions) return;
        HandleMovement();
        HandleInput();
    }

    private void HandleInput()
    {
        if (input.Player.Attack.IsPressed())
        {
            UsedLeftHand();
            isPressedL = true;
        }
        else
        {
            if (isPressedL) // button released
            {
                ResetInputs();
                isPressedL = false;
            }
        }
        if (input.Player.Attack2.IsPressed())
        {
            UsedRightHand();
            isPressedR = true;
        }
        else
        {
            if (isPressedR) // button released
            {
                ResetInputs();
                isPressedR = false;
            }
        }
    }

    private void HandleMovement()
    {
        yaw += lookInput.x * sensitivity * Time.deltaTime;
        pitch -= lookInput.y * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        Camera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        Vector3 dir = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        transform.position += moveSpeed * Time.deltaTime * dir;
    }
}
