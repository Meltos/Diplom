using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyToBuild : MonoBehaviour
{
    [SerializeField] private GameObject _objectBuild;
    [SerializeField] private GameObject _currency;

    #region BODY

    void Update()
    {
        int Cost = 0;
        int storage = 0;
        if (_objectBuild.GetComponent<Tower>() != null && _currency.GetComponent<Money>() != null)
        {
            Cost = _objectBuild.GetComponent<Tower>().Cost;
            storage = _currency.GetComponent<Money>().Count;
        }
        else if (_objectBuild.GetComponent<Enemy>() != null && _currency.GetComponent<Money>() != null)
        {
            Cost = _objectBuild.GetComponent<Enemy>().Cost;
            storage = _currency.GetComponent<Money>().Count;
        }
        if (storage < Cost)
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
