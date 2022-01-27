using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditTower : MonoBehaviour
{
    public Tower TowerObj;
    public Vector3 Offset;
    public Money Money;
    public Text LevelTowerText;

    private RectTransform _thisRectTransform;
    private Camera _mainCamera;

    #region MONO

    private void Awake()
    {
        _thisRectTransform = GetComponent<RectTransform>();
        _mainCamera = Camera.main;
    }

    #endregion

    #region BODY

    void Update()
    {
        if (TowerObj == null)
        {
            Destroy(gameObject);
        }
        else
        {
            _thisRectTransform.position = _mainCamera.WorldToScreenPoint(TowerObj.transform.position + Offset);
            LevelTowerText.text = TowerObj.Level.ToString();
        }
    }

    #endregion
}
