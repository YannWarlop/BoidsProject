using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpherePoint : MonoBehaviour
{
    [SerializeField] private GameObject _debugSphere;
    [SerializeField] private int _collisionPoints;
    [SerializeField] private float _radius;
    [SerializeField] private float _fov;

    void Start() {
        float angleIncrement = Mathf.PI * 2 * ((1 + Mathf.Sqrt(5)) / 2);
        for (int i = 0; i < _collisionPoints; i++) {
            float phi = Mathf.Acos(1 - 2 * ((float)i / _collisionPoints));
            float theta = angleIncrement * i;

            float x = Mathf.Sin(phi) * Mathf.Cos(theta), y = Mathf.Sin(phi) * Mathf.Sin(theta), z = Mathf.Cos(phi);

            if (Vector3.Angle(transform.forward, new Vector3(x, y, z)) <= _fov) {
                Instantiate(_debugSphere, new Vector3(x, y, z), Quaternion.identity);
            }
        }
    }
}
