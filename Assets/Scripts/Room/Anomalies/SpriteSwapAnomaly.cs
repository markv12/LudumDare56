using UnityEngine;

public class SpriteSwapAnomaly : RoomAnomaly {
    [UnityEngine.Serialization.FormerlySerializedAs("spriteSets")]
    public SpriteSet[] spriteSetsLCR; //Left Center Right
    public override void Activate(int correctDoorIndex) {
        SpriteSet sprites = spriteSetsLCR[correctDoorIndex];
        SpriteRenderer toSwap = sprites.sprites[Random.Range(0, sprites.sprites.Length)];
        toSwap.sprite = SpriteAnomalyMasterList.Instance.GetAnomalySprite(toSwap.sprite);
    }

    [System.Serializable]
    public class SpriteSet {
        public SpriteRenderer[] sprites;
    }
}
