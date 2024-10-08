using System.Collections;
using UnityEngine;

public class GnomeFan : MonoBehaviour {
    public Transform mainT;
    public GameObject gnomePrefab;
    public float gnomeScale = 1;
    public float radius;
    public int fanSegments;
    public float spiralLength = 0;
    public float revolutionCount = 1;
    public float mainSpinSpeed;

    private IEnumerator Start() {
        float anglePerSegment = (revolutionCount * 360f) / (float)fanSegments;
        float zOffsetPerSegment = spiralLength / (float)fanSegments;
        Vector3 scaleVec = new Vector3(gnomeScale, gnomeScale, gnomeScale);
        for (int i = 0; i < fanSegments; i++) {
            GameObject arm = new GameObject("Arm");
            Transform armT = arm.transform;
            armT.SetParent(mainT, false);
            armT.localEulerAngles = new Vector3(0, 0, anglePerSegment * i);
            armT.localPosition = new Vector3(0, 0, zOffsetPerSegment * i);
            GameObject newGnome = Instantiate(gnomePrefab, armT);
            newGnome.transform.localPosition = new Vector3(0, radius, 0);
            newGnome.transform.localScale = scaleVec;
            if(i % 5 == 0) {
                yield return null;
            }
        }
    }

    private void Update() {
        mainT.localEulerAngles = new Vector3(0, 0, mainSpinSpeed * Time.time);
    }
}
