using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMovementSettings _movementSettings;


    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Movement();
        }
        else if(_animator.GetBool("running"))
        {
            _animator.SetBool("running", false);
        }
    }

    void Movement()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _cam.transform.localPosition.z;

        Ray ray = _cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, _movementSettings.layer))
        {
            Vector3 hitVec = hit.point;
            hitVec.y = transform.position.y;

            transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, hitVec, _movementSettings.lerpValue), _movementSettings.speed * Time.deltaTime);
            Vector3 newMovePoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newMovePoint - transform.position), _movementSettings.turnSpeed * Time.deltaTime);

            if (!_animator.GetBool("running"))
            {
                _animator.SetBool("running", true);
            }

        }
    }
}
