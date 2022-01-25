using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTowerTrigger : MonoBehaviour
{
    [SerializeField] private float _freezePower;

    public List<Enemy> FreezesEnemy = new List<Enemy>();
    public Tower Tower;

    private bool _isShoot;

    #region BODY

    private void OnTriggerStay(Collider other)
    {
        Enemy otherEnemy;
        if (other.CompareTag("Enemy"))
            otherEnemy = other.GetComponent<Enemy>();
        else
            return;

        if (!otherEnemy.IsFreeze
            && !FreezesEnemy.Contains(other.gameObject.GetComponent<Enemy>()) 
            && Tower.IsPlaced 
            && otherEnemy.HP > 0
            && !otherEnemy.InvulnerabilityToTowers.Contains(Tower.Type))
        {
            otherEnemy.IsFreeze = true;
            otherEnemy.TimeFreeze = 2;
            otherEnemy.FreezePower = _freezePower;
            otherEnemy.IceTower = Tower;
            FreezesEnemy.Add(otherEnemy);
        }
        else if (!FreezesEnemy.Contains(otherEnemy)
            && Tower.IsPlaced
            && otherEnemy.HP > 0
            && !otherEnemy.InvulnerabilityToTowers.Contains(Tower.Type))
        {
            if (Tower.NextLevelTower == null && otherEnemy.IceTower.NextLevelTower != null)
            {
                if (otherEnemy.IceTower != null)
                    otherEnemy.IceTower.transform.GetChild(1).GetComponent<IceTowerTrigger>().FreezesEnemy.Remove(otherEnemy);
                otherEnemy.IceTower = Tower;
                FreezesEnemy.Add(otherEnemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && Tower.IsPlaced && FreezesEnemy.Contains(other.GetComponent<Enemy>()))
        {
            FreezesEnemy.Remove(other.GetComponent<Enemy>());
        }
    }

    private void Update()
    {
        if (FreezesEnemy.Count > 0 && !_isShoot)
        {
            StartCoroutine(Freeze());
        }
    }

    #endregion

    #region CALLBACKS

    /// <summary>
    ///  Накладывание эффекта заморозки.
    /// </summary>
    public IEnumerator Freeze()
    {
        _isShoot = true;
        yield return new WaitForSeconds(Tower.ShootDelay);
        List<Enemy> nullEnemies = new List<Enemy>();
        foreach (var freezeEnemy in FreezesEnemy)
        {
            if (freezeEnemy != null)
            {
                freezeEnemy.IsFreeze = true;
                freezeEnemy.TimeFreeze = 2;
                if (Tower.NextLevelTower == null && freezeEnemy.HP > 0)
                    freezeEnemy.Hp.GetComponent<EnemyHPScript>().Damage(Tower.Damage);
            }
            else
            {
                nullEnemies.Add(freezeEnemy);
            }
        }
        foreach (var nullEnemy in nullEnemies)
        {
            FreezesEnemy.Remove(nullEnemy);
        }
        _isShoot = false;
    }

    #endregion
}
