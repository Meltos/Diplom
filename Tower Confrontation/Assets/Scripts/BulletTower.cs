using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTower : MonoBehaviour
{
    [SerializeField] private float _speed;

    public Transform Target;
    public Tower Tower;

    private Vector3 _oldTarget;

    #region BODY

    void Update()
    {
        if (Target)
        {
            transform.LookAt(Target);
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Time.deltaTime * _speed);
            _oldTarget = new Vector3(Target.position.x, 0, Target.position.z);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _oldTarget, Time.deltaTime * _speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == Target)
        {
            if (Target.GetComponent<Enemy>().HP > 0)
                Target.GetComponent<Enemy>().Hp.GetComponent<EnemyHPScript>().Damage(Tower.Damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
