using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager3D : MonoBehaviour
{
    [Header("References-Boids")] 
    [SerializeField] private GameObject _boidPrefab;
    [Header("Attributes-Boids")]
    [SerializeField] private int _boidSpanwDelta;
    [SerializeField] private int _boidsAmount;
    private void OnEnable() {
        for (int i = 0; i < _boidsAmount; i++)
        {
            Vector3 SpawnPoint = new Vector3(Random.Range(-_boidSpanwDelta, _boidSpanwDelta),
                Random.Range(-_boidSpanwDelta, _boidSpanwDelta), Random.Range(-_boidSpanwDelta, _boidSpanwDelta));
            Vector3 SpawnAngle = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            var boid = Instantiate(_boidPrefab, SpawnPoint, Quaternion.Euler(SpawnAngle));
        }
    }
}