using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.UI;

public class EnemyHPScript : MonoBehaviour
{
    public Vector3 Offset;
    public Enemy EnemyObj;

    #region BODY

    private void Update()
    {
        if (EnemyObj.GetComponent<Enemy>().HP <= 0)
        {
            Destroy(gameObject);
        }
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(EnemyObj.transform.GetChild(0).transform.position + Offset);
        GetComponent<Slider>().value = EnemyObj.HP / EnemyObj.MaxHP;
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
