using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bridge Race/Player/Player Movement Settings")]
public class PlayerMovementSettings : ScriptableObject
{
    public float turnSpeed, speed, lerpValue;
    public LayerMask layer;
}
