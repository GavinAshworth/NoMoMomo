using UnityEngine;

public class ParentPlatformSpawners : MonoBehaviour
{
    [SerializeField] Sprite spriteLong;
    [SerializeField] Sprite spriteShort;
    [SerializeField] GameObject platformShort;
    [SerializeField] GameObject platformLong;
    [SerializeField] int level;

    public Sprite GetSpriteLong() => spriteLong;
    public Sprite GetSpriteShort() => spriteShort;
    public GameObject GetPlatformShort() => platformShort;
    public GameObject GetPlatformLong() => platformLong;
    public int GetSpawnerLevel() => level;

}
