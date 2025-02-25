using UnityEngine;

public class ParentProjectileSpawner : MonoBehaviour
{
    [SerializeField] Sprite projectileSprite;
    [SerializeField] GameObject projectileObject;
    [SerializeField] int level;

    public Sprite GetSprite() => projectileSprite;
    public GameObject GetProjectileObject() => projectileObject;
    public int GetSpawnerLevel() => level;

}
