using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Animator _animator;

    public float turnSpeed, speed, lerpValue;
    public LayerMask layer;

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
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            Vector3 hitVec = hit.point;
            hitVec.y = transform.position.y;

            transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, hitVec, lerpValue), speed * Time.deltaTime);
            Vector3 newMovePoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newMovePoint - transform.position), turnSpeed * Time.deltaTime);

            if (!_animator.GetBool("running"))
            {
                _animator.SetBool("running", true);
            }

        }
    }
}
