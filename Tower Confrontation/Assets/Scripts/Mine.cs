using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mine : MonoBehaviour
{
    [SerializeField] private int _level;
    [SerializeField] private int _levelTwoCost;
    [SerializeField] private int _levelThreeCost;
    [SerializeField] private int _levelFourCost;
    [SerializeField] private int _levelFiveCost;
    [SerializeField] private int _levelSixCost;
    [SerializeField] private int _tickMoneyOne;
    [SerializeField] private int _tickMoneyTwo;
    [SerializeField] private int _tickMoneyThree;
    [SerializeField] private int _tickMoneyFour;
    [SerializeField] private int _tickMoneyFive;
    [SerializeField] private int _tickMoneySix;
    [SerializeField] private Experience _exp;
    [SerializeField] private Money _money;

    #region BODY

    void Update()
    {
        if (_level == 6 || _exp.Count < GetUpgradeMineCost())
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    #endregion

    #region CALLBACKS

    /// <summary>
    ///  Улучшение шахты.
    /// </summary>
    public void UpgradeMine()
    {
        _exp.Count -= GetUpgradeMineCost();
        _level++;
        _money.SetTickMoney();
    }

    /// <summary>
    ///  Получение стоимости улучшения шахты.
    /// </summary>
    private int GetUpgradeMineCost()
    {
        switch (_level)
        {
            case 1:
                return _levelTwoCost;
            case 2:
                return _levelThreeCost;
            case 3:
                return _levelFourCost;
            case 4:
                return _levelFiveCost;
            case 5:
                return _levelSixCost;
        }
        return 0;
    }

    /// <summary>
    ///  Получение количества монет в секунду.
    /// </summary>
    public int GetTickMoney()
    {
        switch (_level)
        {
            case 1:
                return _tickMoneyOne;
            case 2:
                return _tickMoneyTwo;
            case 3:
                return _tickMoneyThree;
            case 4:
                return _tickMoneyFour;
            case 5:
                return _tickMoneyFive;
            case 6:
                return _tickMoneySix;
        }
        return 0;
    }

    #endregion
}
