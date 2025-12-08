using UnityEngine;
using static Equipable;

[CreateAssetMenu(fileName = "ResourceManager", menuName = "ScriptableObjects/ResourceManager")]
public class ResourceManager : SingletonScriptableObject<ResourceManager, ICreationMethodLocated>
{
    public Bullet BulletPrefab;
    public  Gun GunPrefab;

    public Equipable GetItemPrefab(EquipableType type)
    {
        switch (type)
        {
            case EquipableType.Gun:
                return GunPrefab;
            default:
                Debug.LogError("No prefab found for type: " + type);
                return null;
        }
    }
}
