using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeResetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.IsPlayer()) {
            Player.instance.ResetTime();
        }
    }
}
