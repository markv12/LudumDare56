using UnityEngine;

public class ColorShiftAnomaly : RoomAnomaly {
    public SpriteSet indicatorSprites;
    public SpriteSet[] doorSpriteSetsLCR; //Left Center Right
    public Material shiftMaterial;
    private static readonly float[] colorHueShifts = new float[] { 0.05f, 0.3f, 0.55f, 0.80f };
    private static Material[] hueMaterials;

    private void Awake() {
        if(hueMaterials == null) {
            hueMaterials = new Material[colorHueShifts.Length];
            for (int i = 0; i < colorHueShifts.Length; i++) {
                Material hueMat = new Material(shiftMaterial);
                hueMat.SetFloat("_HueShift", colorHueShifts[i % colorHueShifts.Length]);
                hueMaterials[i] = hueMat;
            }
        }
        Material startHueMat = hueMaterials[Random.Range(0, hueMaterials.Length)];
        for (int i = 0; i < doorSpriteSetsLCR.Length; i++) {
            doorSpriteSetsLCR[i].SetMaterial(startHueMat);
        }
        for (int i = 0; i < indicatorSprites.sprites.Length; i++) {
            indicatorSprites.sprites[i].gameObject.SetActive(false);
        }
    }

    public override void Activate(int correctDoorIndex) {
        int indexOffset = Random.Range(0, colorHueShifts.Length);
        for (int i = 0; i < doorSpriteSetsLCR.Length; i++) {
            SpriteSet dss = doorSpriteSetsLCR[i];
            Material hueMat = hueMaterials[(i + indexOffset) % hueMaterials.Length];
            dss.SetMaterial(hueMat);
            if(i == correctDoorIndex) {
                SpriteRenderer indicator = indicatorSprites.sprites[Random.Range(0, indicatorSprites.sprites.Length)];
                indicator.material = hueMat;
                indicator.gameObject.SetActive(true);
            }
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
