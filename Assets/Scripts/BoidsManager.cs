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
    
    private ComputeBuffer _compute;
    private BoidBehaviour[] _boids;


    private void OnEnable() {
        _boids = new BoidBehaviour[_boidsAmount];
        _compute = new ComputeBuffer(_boidsAmount, sizeof(float) * 3);
        for (int i = 0; i < _boidsAmount; i++)
        {
            Vector3 SpawnPoint = new Vector3(Random.Range(-_boidSpanwDelta, _boidSpanwDelta),
                Random.Range(-_boidSpanwDelta, _boidSpanwDelta), Random.Range(-_boidSpanwDelta, _boidSpanwDelta));
            Vector3 SpawnAngle = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            var boid = Instantiate(_boidPrefab, SpawnPoint, Quaternion.Euler(SpawnAngle));
            
            _boids[i] = boid.GetComponent<BoidBehaviour>(); // Store Boid in Boidarray to Compute Dispatch
        }
    }

    private void OnDisable() {
        _compute.Release();
        _compute = null;
    }
}