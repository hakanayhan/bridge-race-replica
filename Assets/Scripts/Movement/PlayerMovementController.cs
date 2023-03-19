using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovementController : MonoBehaviour
{

    [SerializeField] private Camera _cam;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMovementSettings _movementSettings;

    public enum Colors { Blue = 0, Green = 1, Red = 2 }
    [SerializeField] private Colors _selectColor;
    [SerializeField] private Transform _backpackObject;
    [SerializeField] private GameObject _prevObject;
    [SerializeField] private List<GameObject> _bricks = new List<GameObject>();
    [SerializeField] private Material _material;


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

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == _selectColor.ToString())
        {
            target.transform.SetParent(_backpackObject);
            Vector3 pos = _prevObject.transform.localPosition;
            pos.y += 0.22f;
            pos.x = 0;
            pos.z = 0;

            target.transform.localRotation = new Quaternion(0, 0.7071068f, 0, 0.7071068f);
            target.transform.DOLocalMove(pos, 0.2f);
            _prevObject = target.gameObject;
            _bricks.Add(target.gameObject);
            target.tag = "Untagged";

            GenerateBricks.instance.GenerateBrick((int)_selectColor);
        }

        if (target.gameObject.tag == "SetRed" || target.gameObject.tag != "Set" + _selectColor.ToString() && target.gameObject.tag.StartsWith("Set"))
        {
            if (_bricks.Count > 1)
            {
                GameObject obj = _bricks[_bricks.Count - 1];
                _bricks.RemoveAt(_bricks.Count - 1);
                Destroy(obj);

                target.GetComponent<MeshRenderer>().material = _material;
                target.GetComponent<MeshRenderer>().enabled = true;
                target.tag = "Set" + _selectColor.ToString();
            }
        }
    }
}
