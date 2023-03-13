using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraSettings _cameraSettings;
    [SerializeField] private Transform _target;
    private void LateUpdate()
    {
        Vector3 desPos = _target.position + _cameraSettings.offset;
        transform.position = Vector3.Lerp(transform.position, desPos, _cameraSettings.lerpValue);
    }

}
