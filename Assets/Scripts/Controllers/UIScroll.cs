using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScroll : MonoBehaviour
{
    [SerializeField]
    float   _maxX, 
            _minX, 
            _maxY, 
            _minY, 
            _maxZ, 
            _minZ;
    [SerializeField]
    float   _dragFactor,
            _scrollFactor;

    Vector3 _clickStart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += new Vector3(0, 0, Input.mouseScrollDelta.y * _scrollFactor);
        if (transform.localPosition.z > _maxZ)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _maxZ);
        if (transform.localPosition.z < _minZ)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _minZ);
        float z = transform.localPosition.z;
        float hz = transform.localPosition.z / 2;
        if (transform.localPosition.x > _maxX - z)
            transform.localPosition = new Vector3(_maxX - z, transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.x < _minX + z)
            transform.localPosition = new Vector3(_minX + z, transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.y > _maxY - hz)
            transform.localPosition = new Vector3(transform.localPosition.x, _maxY - hz, transform.localPosition.z);
        if (transform.localPosition.y < _minY + hz)
            transform.localPosition = new Vector3(transform.localPosition.x, _minY + hz, transform.localPosition.z);
    }

    private void OnMouseDown()
    {
        _clickStart = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        Vector3 delta = _clickStart - Input.mousePosition;
        Vector3 dir = delta.normalized;
        float speed = delta.magnitude;
        transform.Translate(dir * speed * _dragFactor, Space.World);
        _clickStart = Input.mousePosition;
    }
}
