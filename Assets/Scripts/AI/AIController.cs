using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class AIController : MonoBehaviour
{
    public enum Colors { Blue = 0, Green = 1, Red = 2 }
    [SerializeField] private Colors _selectColor;

    public GameObject targetBrickParent;
    public List<GameObject> targetBricks = new List<GameObject>();

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _radius;
    [SerializeField] private Transform _backpackObject;
    [SerializeField] private List<GameObject> _bricks = new List<GameObject>();
    [SerializeField] private Transform[] _bridges;
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

        int randomN = Random.Range(0, 3);

        if (randomN == 0 && _bricks.Count >= 5)
        {
            int randomRope = Random.Range(0, _bridges.Length);
            List<Transform> ropesNonActiveChild = new List<Transform>();
            foreach (Transform item in _bridges[randomRope])
            {
                if (!item.GetComponent<MeshRenderer>().enabled || item.GetComponent<MeshRenderer>().enabled && item.gameObject.tag != "Diz" + _selectColor.ToString())
                {
                    ropesNonActiveChild.Add(item);
                }
            }
            _targetTransform = _bricks.Count > ropesNonActiveChild.Count ? ropesNonActiveChild[ropesNonActiveChild.Count - 1].position : ropesNonActiveChild[_bricks.Count].position;
        }
        else
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
            _bricks.Add(target.gameObject);
            target.tag = "Untagged";
            targetBricks.Remove(target.gameObject);
            _haveTarget = false;

            GenerateBricks.instance.GenerateBrick((int)_selectColor, this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
