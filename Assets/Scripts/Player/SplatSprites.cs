using UnityEngine;

[CreateAssetMenu(fileName = "SplatSprites", menuName = "Splat Sprites")]
public class SplatSprites : ScriptableObject {
    public Sprite[] sprites;

    private static SplatSprites instance;
    public static SplatSprites Instance {
        get {
            if (instance == null) {
                instance = Resources.Load<SplatSprites>("SplatSprites");
            }
            return instance;
        }
    }
}
