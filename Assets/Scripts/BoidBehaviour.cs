using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehaviour : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private GameObject _debugSphere;
    
    [Header("Attributes")]
    [SerializeField] private float _boidSpeed;
    [SerializeField] private float _fovRadius;
    [SerializeField] private float _fovAngle;
    
    [Header("Attributes - Colllision")]
    [SerializeField] private int _collisionPoints;

    private Vector3 _currentDirection;
    private Vector3[] _directions;
    private void Start() {
        //Precalculate Spherical collision points
        _directions = new Vector3[_collisionPoints];    
        for (int i = 0; i < _collisionPoints; i++) { //ForallPoints
            float phi = (float) Math.Acos(1-2*i/_collisionPoints); //Get Spherical Coordinates
            float theta = Mathf.PI * (1 + Mathf.Pow(5, (float)0.5)) * i;
            //Map Points
            float x = Mathf.Cos(theta) * Mathf.Sin(phi), y = Mathf.Sin(theta) * Mathf.Sin(phi), z = Mathf.Cos(phi);
            _directions[i] = new Vector3(x, y, z);
        }
        _currentDirection = transform.forward;
    }

    private void Update() {
        //MoveForward
        transform.position += _currentDirection * _boidSpeed * Time.deltaTime;
        transform.eulerAngles = _currentDirection.normalized;
        //AvoidCollision
        if (Physics.Raycast(transform.position, _currentDirection, _fovRadius)) { //If Obstacle
            Debug.DrawRay(transform.position, _currentDirection * _fovRadius, Color.red);
            _currentDirection = FindBestDirection(); //Update direction
        } else {
            Debug.DrawRay(transform.position, _currentDirection * _fovRadius, Color.green);
        }
        //Match Speed
        
        //Target Center
        
        //Target Object
    }

    private Vector3 FindBestDirection() {
        //Plot points on Sphere
        Vector3 bestDirection = transform.forward;
        float biggestClearDistance = 0;
        //CALCULATE BEST DIR https://youtu.be/bqtqltqcQhw
    }
}

