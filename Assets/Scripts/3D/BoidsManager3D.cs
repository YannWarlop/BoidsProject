using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager3D : MonoBehaviour
{
    [Header("Spawn")] 
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private int boidCount;
    [SerializeField] private float boidSpawnRadius;

    private void Awake() {
        for (int i = 0; i < boidCount; i++) {
            Vector3 _spawnPoint = new Vector3( //Random Point in Spawn Radius
                Random.Range(-boidSpawnRadius, boidSpawnRadius), 
                Random.Range(-boidSpawnRadius, boidSpawnRadius), 
                Random.Range(-boidSpawnRadius, boidSpawnRadius)); 
            Vector3 _spawnDirection = new Vector3( //Random Direction
                Random.Range(0,360),
                Random.Range(0,360),
                Random.Range(0,360)); 
             
            var _boid = Instantiate(boidPrefab, _spawnPoint, Quaternion.Euler(_spawnDirection)); //Store GameObject 
            var _boidComponent = _boid.GetComponent<BoidBehaviour3D>(); //Store Component
            _boidComponent.position = _spawnPoint; //Initialize public variables for Compute
            _boidComponent.direction = _spawnDirection;
        }
    }
}