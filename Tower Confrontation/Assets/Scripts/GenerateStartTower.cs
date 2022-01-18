using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStartTower : MonoBehaviour
{
    [SerializeField] private List<GameObject> _startTowers = new List<GameObject>();
    [SerializeField] private GameObject _towerPlace;

    private void Awake()
    {
        int i = Random.Range(0, 4);
        Instantiate(_startTowers[i], _towerPlace.transform.position, Quaternion.identity);
    }
}
