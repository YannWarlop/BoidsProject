using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager : MonoBehaviour
{
    [Header("References-Boids")] 
    [SerializeField] private GameObject _boidPrefab;
    [Header("Attributes-Boids")]
    [SerializeField] private int _boidSpanwDelta;
    [SerializeField] private int _boidsAmount;
    
    private ComputeShader _compute;
    private BoidBehaviour[] _boids;


    private void OnEnable() {
        _boids = new BoidBehaviour[_boidsAmount];
        for (int i = 0; i < _boidsAmount; i++)
        {
            Vector3 SpawnPoint = new Vector3(Random.Range(-_boidSpanwDelta, _boidSpanwDelta),
                Random.Range(-_boidSpanwDelta, _boidSpanwDelta), Random.Range(-_boidSpanwDelta, _boidSpanwDelta));
            Vector3 SpawnAngle = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            var boid = Instantiate(_boidPrefab, SpawnPoint, Quaternion.Euler(SpawnAngle));
            
            _boids[i] = boid.GetComponent<BoidBehaviour>(); // Store Boid in Boidarray to Compute Dispatch
        }
    }

    private void Update()
    {
        //Initial Data to compute
        var boidsData = new BoidData[_boidsAmount];
        for (int i = 0; i < _boidsAmount; i++) {
            boidsData[i].position = _boids[i].transform.position;
            boidsData[i].rawDirection = _boids[i].transform.forward;
        }
        //Set Compute
        var BoidsBuffer = new ComputeBuffer(_boidsAmount, BoidData.DataSize );
        BoidsBuffer.SetData(boidsData);
        
        _compute.SetBuffer(0,"boids", BoidsBuffer);
        _compute.SetInt("boidsCount", _boidsAmount);
        
    }
    
    
    public struct BoidData {
        public Vector3 position; //Boid Pos
        public Vector3 rawDirection; //Boid Current Direction

        public Vector3 meanHeading; // Neighboring Boids Mean Direction
        public Vector3 meanCenter; //Neighboring Boids Mean Position

        int numNeighbors; //Number of Neighboring Boids
        
        public static int DataSize => sizeof(float) * 3 * 4 + sizeof(int);
    }
}