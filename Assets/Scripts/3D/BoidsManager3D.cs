using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager3D : MonoBehaviour
{
    [Header("BoidsSpawn")] 
    //Related to Spawning the Boids
    [SerializeField] private GameObject _boidPrefab; //Boid Prefab
    [SerializeField] private float _boidSpanwDelta; //Area in wich to spawn the Boids Randomly
    [SerializeField] private int _boidsAmount; //Amount of Boids to Spawn
    
    [Header("BoidBuffer")]
    //Related to the Boid Compute Buffer
    [SerializeField] public ComputeShader boidsShader;
    const int _shaderThreadSize = 1024;
    private BoidBehaviour3D[] _boidsArray; //Stored array containing all Boids
    //Boid data struct located at the bottom of the file
    
    private void OnEnable() {
        //SPAWN
        for (int i = 0; i < _boidsAmount; i++) {
            Vector3 spawnPoint = new Vector3(Random.Range(-_boidSpanwDelta, _boidSpanwDelta),
                Random.Range(-_boidSpanwDelta, _boidSpanwDelta), Random.Range(-_boidSpanwDelta, _boidSpanwDelta));
            Vector3 spawnAngle = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject Boid = Instantiate(_boidPrefab, spawnPoint, Quaternion.Euler(spawnAngle));
            var _boidbehaviour = Boid.GetComponent<BoidBehaviour3D>();
            _boidbehaviour.position = spawnPoint;
            _boidbehaviour.direction = Boid.transform.forward;

        }
        //GET IN ARRAY
        _boidsArray = FindObjectsOfType<BoidBehaviour3D>(); //Get BoidsBehaviour
    }

    private void Update() {
        //COMPUTE DISPATCH
        BoidData[] boidsData = new BoidData[_boidsAmount]; //Create Struct Data Array
        for (int i = 0; i < _boidsAmount; i++) { //Copy Data in Struct Data Array
            boidsData[i].position = _boidsArray[i].position;
            boidsData[i].direction = _boidsArray[i].direction;
        }
        ComputeBuffer boidsBuffer = new ComputeBuffer(_boidsAmount, BoidData.Size); //Create Buffer
        boidsBuffer.SetData(boidsData); //Set Buffer Data
        
        boidsShader.SetBuffer(0, "boids", boidsBuffer); //Get Buffer in Shader
        boidsShader.SetInt("_boidsAmount", _boidsAmount); 
        boidsShader.SetInt("_shaderThreadSize", _shaderThreadSize);
        boidsShader.SetFloat("_detectRadius", 5);
        boidsShader.SetFloat("_avoidRadius", 0.5f);
        
        
        int _threadGroups = Mathf.CeilToInt((float)_boidsAmount / (float)_shaderThreadSize); //Get ThreadGroupSize
        boidsShader.Dispatch(0, _threadGroups, 1, 1); //Dispatch (Values are to be tested)
        //COMPUTE DATA
        boidsBuffer.GetData(boidsData); // Get Computed Data
        for (int i = 0; i < _boidsAmount; i++) { //Copy Computed Data in Boids
            _boidsArray[i].position = boidsData[i].position;
            _boidsArray[i].direction = boidsData[i].direction;
            
            _boidsArray[i].nearbyBoids = boidsData[i].nearbyBoids;
            _boidsArray[i].alignementHeading = boidsData[i].alignementHeading;
            _boidsArray[i].avoidanceHeading = boidsData[i].avoidanceHeading;
            _boidsArray[i].centerOfMass = boidsData[i].centerOfMass;
            
            _boidsArray[i].ActualizeData(); //Make Boid Refresh data and update Behaviour
        }
        boidsBuffer.Release(); // Release Buffer
    }
    
    public struct BoidData {
        //Own Boid
        public Vector3 position; //BoidPosition
        public Vector3 direction; //Boid Direction

        //BoidsCompute
        public int nearbyBoids; // Number of Boids in Sphere of influence
        public Vector3 alignementHeading; //MeanDir of Boids in SOI
        public Vector3 avoidanceHeading; //Avoidance of Boids too Close
        public Vector3 centerOfMass; //MeanPos of Boids in SOI
        
        public static int Size => sizeof(float) * 3 * 5 + sizeof(int); //Byte Size of Struct
    }
}