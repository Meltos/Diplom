using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenWarrior : MonoBehaviour
{
    [SerializeField] private Text _warriorLevelText;
    [SerializeField] private List<GameObject> _warriors = new List<GameObject>();

    public int WarriorLevel;
    public HiringWarriors HiringWarriors;

    #region BODY

    void Update()
    {
        if (!HiringWarriors.IsOpen)
        {
            GetComponent<Button>().interactable = false;
            _warriorLevelText.text = "";
        }
        else
        {
            GetComponent<Button>().interactable = true;
            _warriorLevelText.text = WarriorLevel.ToString();
        }
    }

    #endregion

    #region CALLBACKS

    /// <summary>
    ///  Установка имени и параметров кнопкам панели улучшения воина.
    /// </summary>
    public void SetNameWarriorLevels(PanelControl upgradePanelControl)
    {
        var splitNameWarrior = HiringWarriors.Warrior.name.Split();
        string nameWarrior = splitNameWarrior[0];
        int expCost = 0;
        for (int i = 1; i < 6; i++)
        {
            Transform btnUpgrade = upgradePanelControl.transform.GetChild(i);
            btnUpgrade.gameObject.GetComponent<UpgradeWarriors>().OpenWarrior = this;
            btnUpgrade.gameObject.GetComponent<UpgradeWarriors>().Level = i;
            btnUpgrade.gameObject.GetComponent<UpgradeWarriors>().Warrior = _warriors[i - 1];
            if (i > WarriorLevel)
            {
                btnUpgrade.gameObject.GetComponent<UpgradeWarriors>().EXPCost = _warriors[i - 1].GetComponent<Enemy>().EXPCost + expCost;
                btnUpgrade.gameObject.GetComponent<UpgradeWarriors>().IsOpen = false;
                expCost += _warriors[i - 1].GetComponent<Enemy>().EXPCost;
            }
            else
                btnUpgrade.gameObject.GetComponent<UpgradeWarriors>().IsOpen = true;
            btnUpgrade.transform.GetChild(0).GetComponent<Text>().text = nameWarrior;
        }
    }

    /// <summary>
    ///  Установка уровня воина после его улучшения.
    /// </summary>
    public void SetWarriorLevel(int level)
    {
        WarriorLevel = level;
    }

    #endregion
}
