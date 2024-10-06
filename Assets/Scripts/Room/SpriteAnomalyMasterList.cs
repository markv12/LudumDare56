using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteAnomalyMasterList", menuName = "MasterLists/Sprite Anomaly Master List")]
public class SpriteAnomalyMasterList : ScriptableObject {
    [SerializeField] private SpriteAnomalyPair[] pairs;
    private readonly Dictionary<string, Sprite> spriteMap = new Dictionary<string, Sprite>(16);

    private static SpriteAnomalyMasterList instance;
    public static SpriteAnomalyMasterList Instance {
        get {
            if (instance == null) {
                instance = Resources.Load<SpriteAnomalyMasterList>("SpriteAnomalyMasterList");
                instance.SetupSpriteMap();
            }
            return instance;
        }
    }

    private void SetupSpriteMap() {
        spriteMap.Clear();
        for (int i = 0; i < pairs.Length; i++) {
            SpriteAnomalyPair sap = pairs[i];
            spriteMap[sap.normalSprite.name] = sap.anomalySprite;
        }
    }

    public Sprite GetAnomalySprite(Sprite sprite) {
        if(spriteMap.TryGetValue(sprite.name, out Sprite anomalySprite)) {
            return anomalySprite;
        } else {
            Debug.LogError("Anomaly sprite not found: " + sprite.name);
            return null;
        }
    }

    [System.Serializable]
    public class SpriteAnomalyPair {
        public Sprite normalSprite;
        public Sprite anomalySprite;
    }
}
