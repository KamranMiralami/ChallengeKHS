using UnityEngine;
using static Equipable;

[CreateAssetMenu(fileName = "ResourceManager", menuName = "ScriptableObjects/ResourceManager")]
public class ResourceManager : SingletonScriptableObject<ResourceManager, ICreationMethodLocated>
{
    public Bullet BulletPrefab;
    public  Gun GunPrefab;
    public Magazine MagazinePrefab;
    public Rock RockPrefab;
    public Flashlight FlashlightPrefab;
    public Hat HatPrefab;

    public Equipable GetItemPrefab(EquipableType type)
    {
        switch (type)
        {
            case EquipableType.Gun:
                return GunPrefab;
            case EquipableType.Magazine:
                return MagazinePrefab;
            case EquipableType.Rock:
                return RockPrefab;
            case EquipableType.Flashlight:
                return FlashlightPrefab;
            case EquipableType.Hat:
                return HatPrefab;
            default:
                Debug.LogError("No prefab found for type: " + type);
                return null;
        }
    }
}
