using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.UI;

public class EnemyHPScript : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    public GameObject EnemyObj;

    #region BODY

    private void Update()
    {
        if (EnemyObj.GetComponent<Enemy>().HP <= 0)
        {
            Destroy(gameObject);
        }
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(EnemyObj.transform.GetChild(0).transform.position + _offset);
        GetComponent<Slider>().value = EnemyObj.GetComponent<Enemy>().HP / EnemyObj.GetComponent<Enemy>().MaxHP;
    }

    #endregion

    #region CALLBACKS

    /// <summary>
    ///  Вычетает из здоровья моба количество передающегося урона
    /// </summary>
    public void Damage(float DMGcount)
    {
        EnemyObj.GetComponent<Enemy>().HP -= DMGcount;
    }

    #endregion
}
