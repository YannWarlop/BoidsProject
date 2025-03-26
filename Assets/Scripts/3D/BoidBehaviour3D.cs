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
    
    // Base Data for Compute Shader
    public Vector3 position; // Boid Position
    public Vector3 direction; // Boid Direction

    //Updated By Compute Shader
    public Vector3 meanSOIDirection; // Average Boid Direction in Sphere Of Influence 

    private void Update() {
        BoidUpdate();
    }

    public void BoidUpdate() {
        Vector3 dirSum = Vector3.zero; //Reset the Direction
        
        //Apply the different Influences
        dirSum += direction;
        dirSum += meanSOIDirection.normalized *_meanDirForce;
        
        
        dirSum.Normalize(); //After all influences are set, normalize the Direction
        //Move the Boid
        transform.position += dirSum * _boidSpeed * Time.deltaTime; //Move
        transform.LookAt(transform.position + dirSum); //Adjust Rotation
        
        //Actualize Data for next Compute
        position = transform.position;
        direction = dirSum;
    }
}


