using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Experience : MonoBehaviour
{
    public int Count;

    private Text _countText;

    private void Awake()
    {
        _countText = GetComponent<Text>();
    }

    #region BODY

    void Update()
    {
        _countText.text = Count.ToString();
    }

    #endregion

    #region CALLBACKS

    public void ExpPlus(int countEXP)
    {
        Count += countEXP;
    }

    public void ExpMinus(int countEXP)
    {
        Count -= countEXP;
    }

    #endregion
}
