using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTowerTrigger : MonoBehaviour
{
    [SerializeField] private float _freezePower;

    public List<GameObject> FreezesEnemy = new List<GameObject>();
    public Tower Tower;

    private bool _isShoot;

    #region BODY

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && !other.GetComponent<Enemy>().IsFreeze && !FreezesEnemy.Contains(other.gameObject) && Tower.IsPlaced && other.gameObject.GetComponent<Enemy>().HP > 0)
        {
            other.GetComponent<Enemy>().IsFreeze = true;
            other.GetComponent<Enemy>().TimeFreeze = 2;
            other.GetComponent<Enemy>().FreezePower = _freezePower;
            other.GetComponent<Enemy>().IceTower = Tower;
            FreezesEnemy.Add(other.gameObject);
        }
        else if (other.CompareTag("Enemy") && !FreezesEnemy.Contains(other.gameObject) && Tower.IsPlaced && other.gameObject.GetComponent<Enemy>().HP > 0)
        {
            if (Tower.NextLevelTower == null && other.GetComponent<Enemy>().IceTower.NextLevelTower != null)
            {
                other.GetComponent<Enemy>().IceTower.gameObject.transform.GetChild(1).gameObject.GetComponent<IceTowerTrigger>().FreezesEnemy.Remove(other.gameObject);
                other.GetComponent<Enemy>().IceTower = Tower;
                FreezesEnemy.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && Tower.IsPlaced && FreezesEnemy.Contains(other.gameObject))
        {
            FreezesEnemy.Remove(other.gameObject);
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
        List<GameObject> nullEnemies = new List<GameObject>();
        foreach (var freezeEnemy in FreezesEnemy)
        {
            if (freezeEnemy != null)
            {
                freezeEnemy.GetComponent<Enemy>().IsFreeze = true;
                freezeEnemy.GetComponent<Enemy>().TimeFreeze = 2;
                if (Tower.NextLevelTower == null && freezeEnemy.GetComponent<Enemy>().HP > 0)
                    freezeEnemy.GetComponent<Enemy>().Hp.GetComponent<EnemyHPScript>().Damage(Tower.Damage);
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
