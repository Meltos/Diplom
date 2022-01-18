using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditTower : MonoBehaviour
{
    public GameObject TowerObj;
    public Vector3 Offset;
    public Money Money;

    #region BODY

    void Update()
    {
        if (TowerObj == null)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(TowerObj.transform.position + Offset);
        }
    }

    #endregion
}
