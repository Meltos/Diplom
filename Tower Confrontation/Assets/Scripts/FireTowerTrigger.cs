using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTowerTrigger : MonoBehaviour
{
    public List<Enemy> BurnedEnemies = new List<Enemy>();
    public Tower Tower;

    private bool _isShoot = false;

    #region BODY

    private void OnTriggerStay(Collider other)
    {
        Enemy otherEnemy;
        if (other.CompareTag("Enemy"))
            otherEnemy = other.GetComponent<Enemy>();
        else
            return;
        if (Tower.IsPlaced
            && !otherEnemy.IsFire
            && otherEnemy.HP > 0
            && !BurnedEnemies.Contains(otherEnemy)
            && !otherEnemy.InvulnerabilityToTowers.Contains(Tower.Type))
        {
            otherEnemy.BurnDamage = Tower.Damage;
            BurnedEnemies.Add(otherEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && Tower.IsPlaced && BurnedEnemies.Contains(other.GetComponent<Enemy>()))
        {
            BurnedEnemies.Remove(other.GetComponent<Enemy>());
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
        List<Enemy> nullEnemies = new List<Enemy>();
        foreach(var burnedEnemy in BurnedEnemies)
        {
            if (burnedEnemy != null)
            {
                burnedEnemy.IsFire = true;
                burnedEnemy.TimeFire = 5;
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
