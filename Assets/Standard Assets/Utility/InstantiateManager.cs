using System;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateManager : Singleton<InstantiateManager> {
    private readonly List<InstantiateRequest> toInstantiate = new List<InstantiateRequest>(128);
    private int lastInstantiateFrame;
    public void Request(GameObject _prefab, Vector3 _position, Quaternion _rotation, Action<GameObject> _onComplete = null) {
        toInstantiate.Add(new InstantiateRequest() {
            prefab = _prefab,
            pos = _position,
            rotation = _rotation,
            onComplete = _onComplete,
        });
    }

    public void Request(GameObject _prefab, Transform _parent, Action<GameObject> _onComplete = null) {
        toInstantiate.Add(new InstantiateRequest() {
            prefab = _prefab,
            parent = _parent,
            onComplete = _onComplete,
        });
    }

    private void Update() {
        if (Time.frameCount > lastInstantiateFrame && toInstantiate.Count > 0) {
            lastInstantiateFrame = Time.frameCount;
            InstantiateRequest request = toInstantiate[0];
            GameObject go;
            if (request.parent) {
                go = Instantiate(request.prefab, request.parent);
            } else {
                go = Instantiate(request.prefab);
                go.transform.SetLocalPositionAndRotation(request.pos, request.rotation);
            }
            go.SetActive(true);
            toInstantiate.RemoveAt(0);

            request.onComplete?.Invoke(go);
        }
    }

    private struct InstantiateRequest {
        public GameObject prefab;
        public Vector3 pos;
        public Quaternion rotation;
        public Transform parent;
        public Action<GameObject> onComplete;
    }
}
