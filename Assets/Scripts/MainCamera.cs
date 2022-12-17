using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private GameObject _car;
    [SerializeField] private float cameraHeight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         Vector3 pos = _car.transform.position;
         pos.z += cameraHeight;
         transform.position = pos;
        transform.position = pos;
    }
}
