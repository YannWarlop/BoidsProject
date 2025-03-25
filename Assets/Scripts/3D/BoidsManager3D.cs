using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager3D : MonoBehaviour
{
    [Header("BoidsSpawn")] 
    //Related to Spawning the Boids
    [SerializeField] private GameObject _boidPrefab; //Boid Prefab
    [SerializeField] private int _boidSpanwDelta; //Area in wich to spawn the Boids Randomly
    [SerializeField] private int _boidsAmount; //Amount of Boids to Spawn
    
    [Header("BoidBuffer")]
    //Related to the Boid Compute Buffer
    [SerializeField] public ComputeShader boidsShader;
    [SerializeField] const int _shaderThreadSize = 1024;
    private BoidBehaviour3D[] _boidsArray; //Stored array containing all Boids
    //Boid data struct located at the bottom of the file
    
    private void OnEnable() {
        //SPAWN
        for (int i = 0; i < _boidsAmount; i++) {
            Vector3 spawnPoint = new Vector3(Random.Range(-_boidSpanwDelta, _boidSpanwDelta),
                Random.Range(-_boidSpanwDelta, _boidSpanwDelta), Random.Range(-_boidSpanwDelta, _boidSpanwDelta));
            Vector3 spawnAngle = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            Instantiate(_boidPrefab, spawnPoint, Quaternion.Euler(spawnAngle));
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
        
        boidsShader.SetBuffer(0, "DataStruct", boidsBuffer); //Get Buffer in Shader
        int _threadGroups = Mathf.CeilToInt(_boidsAmount / _shaderThreadSize); //Get ThreadGroupSize
        boidsShader.Dispatch(0, _threadGroups, 1, 1); //Dispatch (Values are to be tested)
        
        
    }


    public struct BoidData {
        public Vector3 position; //BoidPosition
        public Vector3 direction; //Boid Direction
        
        public static int Size => sizeof(float) * 3 * 2; //Byte Size of Struct
    }
}