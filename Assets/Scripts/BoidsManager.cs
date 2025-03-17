using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager : MonoBehaviour
{
    [Header("References-Boids")]
    [SerializeField] private GameObject _boidPrefab;
    [Header("Attributes-Boids")]
    [SerializeField] private int _boidsAmount;

    private void Awake() {
        for (int i = 0; i < _boidsAmount; i++) {
            Instantiate(_boidPrefab, transform);
        }
    }
}