using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Serialization;

public class BoidBehaviour : MonoBehaviour
{ 
    [Header("Attributes")]
    [SerializeField] private float _boidMoveSpeed;
    [Range(0f,1f)] [SerializeField] private float _boidTurnSpeed;
    [SerializeField] private float _fovRadius;
    [SerializeField] private float _fovAngle;
    
    [Header("Attributes - Colllision")]
    [SerializeField] private int _collisionPoints;
    

    private Vector3 _currentDirection;
    private Vector3[] _directions;
    private void Start() {
        //Precalculate Spherical collision points
        //Could be more optimized if calculated on a single instance instead of every boid
        //Singleton could be possible
        _directions = new Vector3[_collisionPoints];   
        float angleIncrement = Mathf.PI * 2 * ((1 + Mathf.Sqrt(5)) / 2);
        for (int i = 0; i < _collisionPoints; i++) {
            float phi = Mathf.Acos(1 - 2 * ((float)i / _collisionPoints)); //Angles
            float theta = angleIncrement * i;
            //Angles to Coordinates via pherical projection
            float x = Mathf.Sin(phi) * Mathf.Cos(theta), y = Mathf.Sin(phi) * Mathf.Sin(theta), z = Mathf.Cos(phi);
            //Store position in array
            _directions[i] = new Vector3(x, y, z);
        }
        _currentDirection = transform.forward;
    }

    private void FixedUpdate() {
        //AvoidCollision
        if (Physics.Raycast(transform.position, _currentDirection, _fovRadius)) { //If Obstacle
            _currentDirection = Vector3.Lerp(_currentDirection, FindBestDirection(), _boidTurnSpeed);
        } else {
            Debug.DrawRay(transform.position, _currentDirection * _fovRadius, Color.green);
        }
        //Match Speed
        
        //Target Center
        
        //Target Object
        
        //MoveForward
        transform.position += _currentDirection * _boidMoveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + _currentDirection);
    }

    private Vector3 FindBestDirection() {
        //Plot points on Sphere
        Vector3 bestDirection = transform.forward;
        float biggestClearDistance = 0;
        RaycastHit hit;
        for (int i = 0; i < _directions.Length; i++) { //Foreach points
            Vector3 dir = transform.TransformDirection(_directions[i]); //fetch dir in worldspace
            if (Physics.Raycast(transform.position, dir, out hit, _fovRadius)) {
                if (hit.distance > biggestClearDistance) {
                    Debug.DrawRay(transform.position, dir * _fovRadius, Color.red);
                    bestDirection = dir;
                    biggestClearDistance = hit.distance;
                }
            } else { 
                Debug.DrawRay(transform.position, dir * _fovRadius, Color.green);
                return dir; //Clear so return clear dir
            }
        }
        Debug.DrawRay(transform.position, bestDirection * _fovRadius, Color.green);
        return bestDirection; //No clear so retun biggest distance before impact
    }
}

