using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AuraAPI;

public class EnableAura : MonoBehaviour
{
    public GameObject[] volumes;

    void Start() {
        for (int i = 0; i < volumes.Length; i++) {
            volumes[i].GetComponent<AuraVolume>().enabled = true;
        }
    }
}
