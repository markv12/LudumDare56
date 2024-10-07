using UnityEngine;

public class GnomeFaceColorAnomaly : RoomAnomaly {
    public SpriteRenderer gnomeFace;
    public Transform gnomeFaceT;
    public SpriteSet[] doorSpriteSetsLCR; //Left Center Right
    public RoomSpawnLocation[] spawnLocationsLCR; //Left Center Right
    public Material shiftMaterial;
    private static Material[] hueMaterials;

    private void Awake() {
        if(hueMaterials == null) {
            float[] colors = ColorShiftAnomaly.colorHueShifts;
            hueMaterials = new Material[colors.Length];
            for (int i = 0; i < colors.Length; i++) {
                Material hueMat = new Material(shiftMaterial);
                hueMat.SetFloat("_HueShift", colors[i % colors.Length]);
                hueMaterials[i] = hueMat;
            }
        }
        Material startHueMat = hueMaterials[Random.Range(0, hueMaterials.Length)];
        for (int i = 0; i < doorSpriteSetsLCR.Length; i++) {
            doorSpriteSetsLCR[i].SetMaterial(startHueMat);
        }
        gnomeFace.gameObject.SetActive(false);
    }

    public override void Activate(int correctDoorIndex) {
        int colorIndexOffset = Random.Range(0, ColorShiftAnomaly.colorHueShifts.Length);
        int faceDoorIndex = (correctDoorIndex + Random.Range(0, doorSpriteSetsLCR.Length - 2)) % doorSpriteSetsLCR.Length;
        for (int i = 0; i < doorSpriteSetsLCR.Length; i++) {
            SpriteSet dss = doorSpriteSetsLCR[i];
            Material hueMat = hueMaterials[(i + colorIndexOffset) % hueMaterials.Length];
            dss.SetMaterial(hueMat);
            if(i == correctDoorIndex) {
                gnomeFace.material = hueMat;
                gnomeFace.gameObject.SetActive(true);
            }
            if(i == faceDoorIndex) {
                Door faceDoor = spawnLocationsLCR[i].airlock.entranceDoor;
                gnomeFaceT.SetParent(faceDoor.mainT, false);
                gnomeFaceT.SetLocalPositionAndRotation(new Vector3(0, 0, 0.15f), Quaternion.identity);
            }
        }
        for (int i = 0; i < spawnLocationsLCR.Length; i++) {
            spawnLocationsLCR[i].airlock.entranceDoor.SetOpenRotation(new Vector3(0, 0, 110f));
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
