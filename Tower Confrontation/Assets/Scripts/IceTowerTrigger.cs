using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTowerTrigger : MonoBehaviour
{
    [SerializeField] private float _freezePower;

    public List<GameObject> FreezesEnemy = new List<GameObject>();
    public Tower Tower;
    public bool IsDestroy;

    private bool _isShoot = false;
    private List<GameObject> _notFreezesEnemy = new List<GameObject>();

    #region MONO

    public void DestroyParent()
    {
        if (FreezesEnemy.Count == 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    #endregion

    #region BODY

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && !other.GetComponent<Enemy>().IsFreeze && Tower.IsPlaced && !IsDestroy)
        {
            other.GetComponent<Enemy>().IsFreeze = true;
            FreezesEnemy.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.GetComponent<Enemy>().IsFreeze && Tower.IsPlaced)
        {
            other.GetComponent<Enemy>().IsFreeze = false;
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
    ///  Замораживание мобов с задержкой.
    /// </summary>
    public IEnumerator Freeze()
    {
        _isShoot = true;
        foreach (var freezeEnemy in FreezesEnemy)
        {
            if (freezeEnemy != null)
                freezeEnemy.GetComponent<MoveToWayPoints>().Speed *= 1 - _freezePower;
        }
        yield return new WaitForSeconds(Tower.ShootDelay);
        foreach (var freezeEnemy in FreezesEnemy)
        {
            if (freezeEnemy != null)
            {
                if (Tower.NextLevelTower == null && freezeEnemy.GetComponent<Enemy>().HP > 0)
                    freezeEnemy.GetComponent<Enemy>().Hp.GetComponent<EnemyHPScript>().Damage(Tower.Damage);
                freezeEnemy.GetComponent<MoveToWayPoints>().Speed = freezeEnemy.GetComponent<MoveToWayPoints>().MaxSpeed;
            }
        }
        _notFreezesEnemy.AddRange(FreezesEnemy);
        foreach (var notFreezeEnemy in _notFreezesEnemy)
        {
            if (notFreezeEnemy == null)
                FreezesEnemy.Remove(notFreezeEnemy);
            else if (notFreezeEnemy.GetComponent<Enemy>().IsFreeze == false)
                FreezesEnemy.Remove(notFreezeEnemy);
        }
        _notFreezesEnemy = new List<GameObject>();
        if (IsDestroy == true)
        {
            foreach (var freezeEnemy in FreezesEnemy)
            {
                freezeEnemy.GetComponent<Enemy>().IsFreeze = false;
            }
            Destroy(transform.parent.gameObject);
        }
        _isShoot = false;
    }

    #endregion
}
