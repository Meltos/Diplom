using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTrigger : MonoBehaviour
{
    [SerializeField] private Tower _tower;

    private bool _enemylock;
    private GameObject _curTarget;

    #region BODY

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && !_enemylock && !other.GetComponent<Enemy>().InvulnerabilityToTowers.Contains(_tower.Type))
        {
            _tower.target = other.gameObject.transform;
            _curTarget = other.gameObject;
            _enemylock = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.gameObject == _curTarget)
        {
            _enemylock = false;
            _tower.target = null;
            _curTarget = null;
        }
    }

    void Update()
    {
        if (!_curTarget)
        {
            _enemylock = false;
        }
        if (_curTarget && _curTarget.GetComponent<Enemy>().HP < 0)
        {
            _enemylock = false;
            _tower.target = null;
            _curTarget = null;
        }
    }

    #endregion
}
