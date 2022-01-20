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
        if (other.CompareTag("Enemy")
            && Tower.IsPlaced
            && !other.gameObject.GetComponent<Enemy>().IsFire
            && other.gameObject.GetComponent<Enemy>().HP > 0
            && !BurnedEnemies.Contains(other.gameObject)
            && !other.GetComponent<Enemy>().InvulnerabilityToTowers.Contains(Tower.Type))
        {
            other.gameObject.GetComponent<Enemy>().BurnDamage = Tower.Damage;
            BurnedEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && Tower.IsPlaced && BurnedEnemies.Contains(other.gameObject))
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

    /// <summary>
    ///  Накладывание эффекта поджога.
    /// </summary>

    IEnumerator Fire()
    {
        _isShoot = true;
        yield return new WaitForSeconds(Tower.ShootDelay);
        List<GameObject> nullEnemies = new List<GameObject>();
        foreach(var burnedEnemy in BurnedEnemies)
        {
            if (burnedEnemy != null)
            {
                burnedEnemy.GetComponent<Enemy>().IsFire = true;
                burnedEnemy.GetComponent<Enemy>().TimeFire = 5;
            }
            else
            {
                nullEnemies.Add(burnedEnemy);
            }
        }
        foreach(var nullEnemy in nullEnemies)
        {
            BurnedEnemies.Remove(nullEnemy);
        }
        _isShoot = false;
    }

    #endregion
}
