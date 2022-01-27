using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    [SerializeField] private Mine _mine;

    public int Count;

    private bool _isTick;
    private int _coinInTick;
    private Text _countText;

    #region MONO

    public void Awake()
    {
        _countText = GetComponent<Text>();
        SetTickMoney();
    }

    #endregion

    #region BODY

    void Update()
    {
        _countText.text = Count.ToString();
        if (!_isTick)
        {
            StartCoroutine(tick());
        }
    }

    #endregion

    #region CALLBACKS

    /// <summary>
    ///  ��������� ���������� ����� � �������.
    /// </summary>
    public void SetTickMoney()
    {
        _coinInTick = _mine.GetTickMoney();
    }

    /// <summary>
    ///  ���������� ���������� �����.
    /// </summary>
    public void CoinPlus(int plusCoin)
    {
        Count += plusCoin;
    }

    /// <summary>
    ///  ���������� ���������� �����.
    /// </summary>
    public void CoinMinus(int minusCoin)
    {
        Count -= minusCoin;
    }

    /// <summary>
    ///  ������������ ���������� ����� � ����������� �� ������ �����.
    /// </summary>
    IEnumerator tick()
    {
        _isTick = true;
        yield return new WaitForSeconds(1);
        CoinPlus(_coinInTick);
        _isTick = false;
    }

    #endregion
}
