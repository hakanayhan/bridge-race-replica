using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBricks : MonoBehaviour
{
    [SerializeField] public static GenerateBricks instance;
    [SerializeField] private GameObject _redBrick, _greenBrick, _blueBrick;
    [SerializeField] private Transform _redBrickParent, _greenBrickParent, _blueBrickParent;
    [SerializeField] private int _minX, _maxX, _minZ, _maxZ;
    [SerializeField] private LayerMask _layerMask;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // 0 blue, 1 green, 2 red
    public void GenerateBrick(int number, AIController AIController=null)
    {
        if (number == 0)
        {
            Generate(_blueBrick, _blueBrickParent);
        }
        else if (number == 1)
        {
            Generate(_greenBrick, _greenBrickParent, AIController);
        }
        else if (number == 2)
        {
            Generate(_redBrick, _redBrickParent, AIController);
        }
    }

    private void Generate(GameObject gameObject, Transform parent, AIController AIController = null)
    {
        GameObject g = Instantiate(gameObject);
        Vector3 desPos = GiveRandomPos();
        g.SetActive(false);

        Collider[] colliders = Physics.OverlapSphere(desPos, 1, _layerMask);
        while (colliders.Length>0)
        {
            desPos = GiveRandomPos();
            colliders = Physics.OverlapSphere(desPos, 1, _layerMask);
        }
        g.SetActive(true);
        g.transform.position = desPos;
        if (AIController!=null)
        {
            AIController.targetBricks.Add(g);
        }
    }

    private Vector3 GiveRandomPos()
    {
        return new Vector3(Random.Range(_minX, _maxX), _redBrick.transform.position.y, Random.Range(_minZ, _maxZ));
    }
}
