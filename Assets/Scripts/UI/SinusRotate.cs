using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    void Update()
    {
        transform.rotation = Quaternion.Euler(rotation * Mathf.Sin(Time.time * speed));
    }
}
