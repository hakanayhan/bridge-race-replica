using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class AIController : MonoBehaviour
{
    public enum Colors { Red, Green, Blue}
    [SerializeField] private Colors _selectColor;

    public GameObject targetBrickParent;
    public List<GameObject> targetBricks = new List<GameObject>();

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _radius;
    [SerializeField] private Transform _backpackObject;
    [SerializeField] private List<GameObject> _cubes = new List<GameObject>();
    [SerializeField] private GameObject _prevObject;
    private Vector3 _targetTransform;
   

    private bool _haveTarget = false;
    private void Start()
    {
        for (int i = 0; i < targetBrickParent.transform.childCount; i++)
        {
            targetBricks.Add(targetBrickParent.transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        if(!_haveTarget && targetBricks.Count > 0)
        {
            FindTarget();
        }
    }

    void FindTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radius);
        List<Vector3> ourColors = new List<Vector3>();
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == _selectColor.ToString())
            {
                ourColors.Add(hitColliders[i].transform.position);
            }
        }

        if (ourColors.Count > 0)
        {
            _targetTransform = ourColors[0];
        }
        else
        {
            int random = Random.Range(0, targetBricks.Count);
            _targetTransform = targetBricks[random].transform.position;
        }

        _agent.SetDestination(_targetTransform);
        if (!_animator.GetBool("running"))
        {
            _animator.SetBool("running", true);
        }
        _haveTarget = true;
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

            target.transform.localRotation = new Quaternion(0,0.7071068f,0, 0.7071068f);
            target.transform.DOLocalMove(pos, 0.2f);
            _prevObject = target.gameObject;
            _cubes.Add(target.gameObject);
            target.tag = "Untagged";
            targetBricks.Remove(target.gameObject);
            _haveTarget = false;
        }
    }
}
