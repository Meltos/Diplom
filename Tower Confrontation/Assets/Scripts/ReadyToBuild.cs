using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyToBuild : MonoBehaviour
{
    [SerializeField] private Tower _objectBuild;
    [SerializeField] private Money _currency;

    #region BODY

    void Update()
    {
        int cost = 0;
        int storage = 0;
        if (_objectBuild != null && _currency != null)
        {
            cost = _objectBuild.Cost;
            storage = _currency.Count;
        }
        if (storage < cost)
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
