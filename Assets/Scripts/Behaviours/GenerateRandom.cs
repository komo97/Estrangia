using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandom : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _prefabs;
    [SerializeField]
    private int          _spawnNo;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _spawnNo; ++i)
        {
            GameObject go = Instantiate(_prefabs[Random.Range(0, _prefabs.Length - 1)], transform);
            go.transform.localPosition = new Vector3(Random.Range(-19.0f, 19.0f), Random.Range(-9.0f, 9.0f), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
