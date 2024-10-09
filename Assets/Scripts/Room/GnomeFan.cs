using System.Collections;
using System.Collections.Generic;
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

    private static readonly List<Transform> gnomeTs = new List<Transform>(32);
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
            Transform gnomeT = newGnome.transform;
            gnomeT.localPosition = new Vector3(0, radius, 0);
            gnomeT.localScale = scaleVec;
            gnomeTs.Add(gnomeT);
            if(i % 5 == 0) {
                yield return null;
            }
        }
    }

    private void Update() {
        mainT.localEulerAngles = new Vector3(0, 0, mainSpinSpeed * Time.time);
        float sinTime = Mathf.Sin(Time.time);
        Vector3 pos1 = new Vector3(0, radius + sinTime * 0.2f, 0);
        Vector3 pos2 = new Vector3(0, radius + sinTime * -0.2f, 0);
        for (int i = 0; i < gnomeTs.Count; i++) {
            Transform gnomeT = gnomeTs[i];
            gnomeT.localPosition = i % 2 == 0 ? pos1 : pos2;
        }
    }
}
