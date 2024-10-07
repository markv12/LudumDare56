using UnityEngine;

public class ColorShiftAnomaly : RoomAnomaly {
    public SpriteSet indicatorSprites;
    public SpriteSet[] doorSpriteSetsLCR; //Left Center Right
    public Material shiftMaterial;
    private static readonly float[] colorHueShifts = new float[] { 0.15f, 0.3f, 0.45f, 0.6f, 0.7f };
    public override void Activate(int correctDoorIndex) {
        int indexOffset = Random.Range(0, colorHueShifts.Length);
        float correctHueShift = colorHueShifts[indexOffset];
        for (int i = indexOffset; i < doorSpriteSetsLCR.Length + indexOffset; i++) {
            Material hueMat = new Material(shiftMaterial);
            hueMat.SetFloat("_HueShift", colorHueShifts[i % colorHueShifts.Length]);
            if(i == correctDoorIndex) {
                indicatorSprites.SetMaterial(hueMat);
            }
            SpriteSet spriteSet = doorSpriteSetsLCR[i % doorSpriteSetsLCR.Length];
            spriteSet.SetMaterial(hueMat);
        }
    }

    [System.Serializable]
    public class SpriteSet {
        public SpriteRenderer[] sprites;

        public void SetMaterial(Material mat) {
            for (int i = 0; i < sprites.Length; i++) {
                sprites[i].material = mat;
            }
        }
    }
}
