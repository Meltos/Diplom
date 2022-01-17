using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HiringWarriors : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SendWarriors _sendWarriors;
    [SerializeField] private Experience _exp;
    [SerializeField] private Color _colorEXP;
    [SerializeField] private Color _colorText;

    public bool IsOpen;
    public int CountEnemy;
    public Text CountEnemeyText;
    public GameObject Warrior;

    #region MONO

    private void Awake()
    {
        if (!IsOpen)
        {
            CountEnemeyText.color = _colorEXP;
        }
        CountEnemeyText.text = Warrior.GetComponent<Enemy>().EXPCost.ToString();
    }

    #endregion

    #region BODY

    void Update()
    {
        if (IsOpen)
            CountEnemeyText.text = CountEnemy.ToString();
        if (_exp.Count < Warrior.GetComponent<Enemy>().EXPCost)
            GetComponent<Button>().interactable = false;
        else
            GetComponent<Button>().interactable = true;
    }

    #endregion

    #region CALLBACKS

    /// <summary>
    ///  ���������� ��� ���������� ���������� ����� ��� �������� ����������.
    /// </summary>
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!IsOpen && _exp.Count >= Warrior.GetComponent<Enemy>().EXPCost)
        {
            _exp.Count -= Warrior.GetComponent<Enemy>().EXPCost;
            CountEnemeyText.color = _colorText;
            IsOpen = true;
        }
        else if (IsOpen)
        {
            //Use this to tell when the user left-clicks on the Button
            if (pointerEventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && _sendWarriors.CountAllEnemy < 12)
            {
                for (int i = 0; i < 12 - _sendWarriors.CountAllEnemy; i++)
                {
                    CountEnemy++;
                    _sendWarriors.CostArmy += Warrior.GetComponent<Enemy>().Cost;
                    _sendWarriors.Warriors.Add(Warrior.name + CountEnemy, Warrior);
                }
                _sendWarriors.CountAllEnemy = 12;
            }
            //Use this to tell when the user right-clicks on the Button
            else if (pointerEventData.button == PointerEventData.InputButton.Right && Input.GetKey(KeyCode.LeftShift) && CountEnemy > 0)
            {
                int pasteCountEnemy = CountEnemy;
                for (int i = 0; i < pasteCountEnemy; i++)
                {
                    _sendWarriors.Warriors.Remove(Warrior.name + CountEnemy);
                    CountEnemy--;
                    _sendWarriors.CostArmy -= Warrior.GetComponent<Enemy>().Cost;
                }
                _sendWarriors.CountAllEnemy -= pasteCountEnemy;
            }
            else if (pointerEventData.button == PointerEventData.InputButton.Left && _sendWarriors.CountAllEnemy < 12)
            {
                CountEnemy++;
                _sendWarriors.CostArmy += Warrior.GetComponent<Enemy>().Cost;
                _sendWarriors.Warriors.Add(Warrior.name + CountEnemy, Warrior);
                _sendWarriors.CountAllEnemy++;
            }
            else if (pointerEventData.button == PointerEventData.InputButton.Right && CountEnemy > 0)
            {
                _sendWarriors.Warriors.Remove(Warrior.name + CountEnemy);
                CountEnemy--;
                _sendWarriors.CostArmy -= Warrior.GetComponent<Enemy>().Cost;
                _sendWarriors.CountAllEnemy--;
            }
        }
    }

    /// <summary>
    ///  ������� ���� ������ ����������� ������.
    /// </summary>
    public int RemoveOldHiringWarriors()
    {
        int pasteCountEnemy = CountEnemy;
        for (int i = 0; i < pasteCountEnemy; i++)
        {
            _sendWarriors.Warriors.Remove(Warrior.name + CountEnemy);
            CountEnemy--;
            _sendWarriors.CostArmy -= Warrior.GetComponent<Enemy>().Cost;
        }
        _sendWarriors.CountAllEnemy -= pasteCountEnemy;
        return pasteCountEnemy;
    }

    /// <summary>
    ///  ��������� ������� �� ������ �������� ������.
    /// </summary>
    public void AddNewHiringWarriors(int pasteCountEnemy)
    {
        for (int i = 0; i < pasteCountEnemy; i++)
        {
            CountEnemy++;
            _sendWarriors.CostArmy += Warrior.GetComponent<Enemy>().Cost;
            _sendWarriors.Warriors.Add(Warrior.name + CountEnemy, Warrior);
        }
        _sendWarriors.CountAllEnemy += pasteCountEnemy;
    }

    #endregion
}