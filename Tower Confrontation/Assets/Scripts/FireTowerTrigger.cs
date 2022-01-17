using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTowerTrigger : MonoBehaviour
{
    public List<GameObject> BurnedEnemies = new List<GameObject>();
    public Tower Tower;

    private bool _isShoot = false;

    #region BODY

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && Tower.IsPlaced)
        {
            other.gameObject.GetComponent<Enemy>().IsFire = true;
            other.gameObject.GetComponent<Enemy>().TimeFire = 5;
            BurnedEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && Tower.IsPlaced)
        {
            BurnedEnemies.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (BurnedEnemies.Count > 0 && !_isShoot)
        {
            StartCoroutine(Fire());
        }
    }

    #endregion

    #region CALLBACKS

    IEnumerator Fire()
    {
        _isShoot = true;
        yield return new WaitForSeconds(Tower.ShootDelay);
        foreach(var burnedEnemy in BurnedEnemies)
        {
            if (burnedEnemy != null)
            {
                burnedEnemy.GetComponent<Enemy>().IsFire = true;
                burnedEnemy.GetComponent<Enemy>().TimeFire = 5;
            }
        }
        _isShoot = false;
    }

    #endregion
}
