using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions {
    public static void EnsureStopAndRestartCoroutine(this MonoBehaviour value, ref Coroutine routine, IEnumerator newRoutine) {
        value.EnsureCoroutineStopped(ref routine);
        routine = value.StartCoroutine(newRoutine);
    }
    public static void EnsureCoroutineStopped(this MonoBehaviour value, ref Coroutine routine) {
        if (routine != null) {
            value.StopCoroutine(routine);
            routine = null;
        }
    }

    public static Coroutine CreateUIAnimRoutine(this MonoBehaviour value, float duration, Action<float> changeFunction, Action onComplete = null) {
        return value.StartCoroutine(UIAnimRoutine(duration, changeFunction, onComplete));
    }

    public static IEnumerator UIAnimRoutine(float duration, Action<float> changeFunction, Action onComplete = null) {
        float elapsedTime = 0;
        float progress = 0;
        while (progress <= 1) {
            changeFunction(progress);
            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / duration;
            yield return null;
        }
        changeFunction(1);
        onComplete?.Invoke();
    }

    public static Coroutine CreateWorldAnimRoutine(this MonoBehaviour value, float duration, Action<float> changeFunction, Action onComplete = null) {
        return value.StartCoroutine(WorldAnimRoutine(duration, changeFunction, onComplete));
    }

    public static IEnumerator WorldAnimRoutine(float duration, Action<float> changeFunction, Action onComplete = null) {
        float elapsedTime = 0;
        float progress = 0;
        while (progress <= 1) {
            changeFunction(progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / duration;
            yield return null;
        }
        changeFunction(1);
        onComplete?.Invoke();
    }

    public const float ANIM_DURATION = 0.8f;
    public static void AnimateTransform(this MonoBehaviour value, Transform t, Vector3 startPos, Quaternion startRotation, Vector3 endPos, Quaternion endRotation, Action onComplete) {
        value.CreateWorldAnimRoutine(ANIM_DURATION, (float progress) => {
            float easedProgress = Easing.easeInOutSine(0f, 1f, progress);
            t.SetPositionAndRotation(Vector3.Lerp(startPos, endPos, easedProgress), Quaternion.Lerp(startRotation, endRotation, easedProgress));
        }, () => {
            onComplete?.Invoke();
        });
    }

    public static T EnsureScriptableObjectPresent<T>(T scriptableObject, string scriptName, string controllerName) where T : ScriptableObject {
        if (scriptableObject == null) {
            Debug.LogError("No " + scriptName + " assigned to " + controllerName + ", using default.");
            return ScriptableObject.CreateInstance<T>();
        }
        return scriptableObject;
    }

    public static T EnsureComponentPresent<T>(this MonoBehaviour value, T component, string componentName) where T : Component {
        if (component == null) {
            component = value.GetComponent<T>();
            if (component == null) {
                Debug.LogError("No " + componentName + " component found on Enemy.");
            }
        }
        return component;
    }

    public static T[] GetComponentsOnlyInChildrenNested<T>(this MonoBehaviour script) where T : class {
        //https://answers.unity.com/questions/496958/how-can-i-get-only-the-childrens-of-a-gameonbject.html
        List<T> group = new List<T>();

        //collect only if its an interface or a Component
        if (typeof(T).IsInterface
            || typeof(T).IsSubclassOf(typeof(Component))
            || typeof(T) == typeof(Component)) {
            foreach (Transform child in script.transform) {
                group.AddRange(child.GetComponentsInChildren<T>());
            }
        }

        return group.ToArray();
    }

    public static T[] GetComponentsOnlyInChildren<T>(this MonoBehaviour script) where T : class {
        //https://answers.unity.com/questions/496958/how-can-i-get-only-the-childrens-of-a-gameonbject.html
        List<T> group = new List<T>();

        //collect only if its an interface or a Component
        if (typeof(T).IsInterface
            || typeof(T).IsSubclassOf(typeof(Component))
            || typeof(T) == typeof(Component)) {
            foreach (Transform child in script.transform) {
                group.AddRange(child.GetComponents<T>());
            }
        }

        return group.ToArray();
    }

    public const string PLAYER_TAG = "Player";
    public static bool IsPlayer(this GameObject value) {
        return value.CompareTag(PLAYER_TAG);
    }
}
