using UnityEngine;
using UnityEngine.Windows;

public class Gun : Equipable
{
    [SerializeField] private Transform shootingPos;
    [SerializeField] private int MagazineSize = 20;
    int bulletCount;
    public int BulletCount
    {
        get
        {
            return bulletCount;
        }
        set
        {
            value = Mathf.Clamp(value, 0, MagazineSize);
            bulletCount = value;
            UIManager.Instance.ChangeAmmo(bulletCount);
            owner.CurrentItem.NumberOfUses = bulletCount;
        }
    }
    bool singleMode;
    bool SingleMode {
        get {
            return singleMode;
        }
        set
        {
            singleMode = value;
            if (singleMode)
            {
                cooldown = float.MaxValue;
            }
            else
            {
                cooldown = 0.2f;
            }
            UIManager.Instance.ChangeMode(value ? "Single" : "Auto");
        }
    }
    float currentCooldown;
    float cooldown;
    private void Awake()
    {
        SingleMode = false;
    }
    public override void Equip(EquipLocation owner)
    {
        this.owner = owner;
        UIManager.Instance.DisplayAmmo(true);
        UIManager.Instance.DisplayMode(true);
        base.Equip(owner);
        // probably run animation here
    }
    public override void Unequip()
    {
        // probably run animation here
        UIManager.Instance.DisplayAmmo(false);
        UIManager.Instance.DisplayMode(false);
        Destroy(gameObject);
    }
    public override void OnUse(Vector3 targetPoint)
    {
        if (BulletCount <= 0) return;
        Vector3 direction = (targetPoint - shootingPos.position).normalized;
        var bullet = Instantiate(ResourceManager.Instance.BulletPrefab, shootingPos.position, Quaternion.LookRotation(direction));
        bullet.Shoot(direction);
        currentCooldown = cooldown;
        BulletCount--;
    }
    public override bool CanBeUsed()
    {
        return currentCooldown <= 0 && BulletCount > 0;
    }
    public void ChangeMode()
    {
        Debug.Log("Changing gun mode");
        SingleMode = !SingleMode;
    }
    private void Update()
    {
        if (currentCooldown > 0 && currentCooldown < 100)
        {
            currentCooldown -= Time.deltaTime;
        }
    }
    public override void ResetItem()
    {
        if(currentCooldown <= 0 || currentCooldown >= 100) // this is basically here to prevent auto mouse clickers (kinda a cheat)
            currentCooldown = 0.2f;
    }
    public override void SetUseCount(int count)
    {
        BulletCount = count;
    }
    public override int UseCount => BulletCount;
}
