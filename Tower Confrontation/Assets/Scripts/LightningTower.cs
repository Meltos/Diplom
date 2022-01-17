using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTower : MonoBehaviour
{
    public Transform Target;
    public Tower Tower;

    private bool _isShoot;

    #region BODY

    void Update()
    {
        if (Target && !_isShoot)
        {
            transform.LookAt(Target);
            StartCoroutine(Sparkle());
        }
    }

    private IEnumerator Sparkle()
    {
        _isShoot = true;
        transform.localScale += new Vector3(0, 0, Vector3.Distance(transform.position, Target.position) - 1);
        if (Target.GetComponent<Enemy>().HP > 0)
            Target.GetComponent<Enemy>().Hp.GetComponent<EnemyHPScript>().Damage(Tower.Damage);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    #endregion
}
