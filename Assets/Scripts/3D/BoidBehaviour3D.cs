using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Serialization;

public class BoidBehaviour3D : MonoBehaviour
{
    [Header("Boid Settings")]
    [SerializeField] private float _boidSpeed;
    [SerializeField] private float _meanDirForce;
    [SerializeField] private float _colAvoidForce;
    [SerializeField] private float _fovRadius;
    
    // Base Data for Compute Shader
    public Vector3 position; // Boid Position
    public Vector3 direction; // Boid Direction

    //Updated By Compute Shader
    public Vector3 meanSOIDirection; // Average Boid Direction in Sphere Of Influence 
    
    //For Sphere Collision Check
    [Header("Spherical Collision Settings")]
    [SerializeField] private int _collisionNumbers;
    private Vector3[] _collisionDirections;

    private void Start()
    {
        //Precalculate Spherical collision points
        //Could be more optimized if calculated on a single instance instead of every boid
        //Singleton could be possible
        _collisionDirections = new Vector3[_collisionNumbers]; //Array of Points to check from the boid to them
        float angleIncrement = Mathf.PI * 2 * ((1 + Mathf.Sqrt(5)) / 2); // Calculate incrementation of points
        for (int i = 0; i < _collisionNumbers; i++) {
            float phi = Mathf.Acos(1 - 2 * ((float)i / _collisionNumbers)); //Angles
            float theta = angleIncrement * i;
            //Angles to Coordinates via pherical projection
            float x = Mathf.Sin(phi) * Mathf.Cos(theta), y = Mathf.Sin(phi) * Mathf.Sin(theta), z = Mathf.Cos(phi);
            //Store position in array
            _collisionDirections[i] = new Vector3(x, y, z);
        }
    }
    private void Update() {
        BoidUpdate();
    }

    public void BoidUpdate() {
        Vector3 dirSum = Vector3.zero; //Reset the Direction
        
        //Apply the different Influences
        dirSum += direction;
        dirSum += meanSOIDirection.normalized *_meanDirForce;
        
        //AvoidCollision
        if (Physics.Raycast(transform.position, direction, _fovRadius)) { //If Obstacle
            dirSum += FindBestDirection() * _colAvoidForce;
        } else {
            Debug.DrawRay(transform.position, direction * _fovRadius, Color.green);
        }
        
        //Adjust Values
        dirSum = dirSum.normalized;
        transform.position += dirSum.normalized * _boidSpeed * Time.deltaTime; //Move the Boid
        transform.LookAt(transform.position + dirSum); //Adjust Rotation
        
        //Actualize Data for next Compute
        position = transform.position;
        direction = dirSum;
    }

    private bool CollisionCheck() { //Simple Raycast Forward
        if (Physics.Raycast(transform.position, transform.forward, _fovRadius)) return true; // If Heading for collision Return Bool
        else return false;
    }

    private Vector3 FindBestDirection() {
        //Plot points on Sphere
        Vector3 bestDirection = transform.forward;
        float biggestClearDistance = 0;
        RaycastHit hit;
        for (int i = 0; i < _collisionDirections.Length; i++) { //Foreach points
            Vector3 dir = transform.TransformDirection(_collisionDirections[i]); //fetch dir in worldspace
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


