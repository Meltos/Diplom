using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHP : MonoBehaviour
{
    public float HP;

    #region BODY

    void Update()
    {
        GetComponent<Text>().text = HP.ToString();
    }

    #endregion

    #region CALLBACKS

    /// <summary>
    ///  ��������� �� �������� ����� ���������� ����������� �����.
    /// </summary>
    public void Damage(float DMGcount)
    {
        HP -= DMGcount;
    }

    #endregion
}
