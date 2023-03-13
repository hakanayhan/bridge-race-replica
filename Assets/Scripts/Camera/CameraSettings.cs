using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bridge Race/Camera/Camera Settings")]
public class CameraSettings : ScriptableObject
{
    public Vector3 offset;
    public float lerpValue;
}
