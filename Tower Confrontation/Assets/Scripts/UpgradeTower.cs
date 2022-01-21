using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTower : MonoBehaviour
{
    [SerializeField] private EditTower _parent;

    #region BODY

    void Update()
    {
        if (_parent.TowerObj.NextLevelTower == null)
        {
            GetComponent<Button>().interactable = false;
        }
        else if (_parent.Money.Count < _parent.TowerObj.NextLevelTower.Cost)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    #endregion
}
