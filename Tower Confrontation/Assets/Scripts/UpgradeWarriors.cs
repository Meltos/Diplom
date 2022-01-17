using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWarriors : MonoBehaviour
{
    [SerializeField] private Text _expCostText;
    [SerializeField] private Experience _exp;
    [SerializeField] private PanelControl _panelControl;

    public OpenWarrior OpenWarrior;
    public int Level;
    public bool IsOpen;
    public int EXPCost;
    public GameObject Warrior;

    #region BODY

    void Update()
    {
        if (!IsOpen)
        {
            _expCostText.text = EXPCost.ToString();
            if (_exp.Count < EXPCost)
                GetComponent<Button>().interactable = false;
            else
                GetComponent<Button>().interactable = true;
        }
        else
        {
            _expCostText.text = "";
            GetComponent<Button>().interactable = false;
        }
    }

    #endregion

    #region CALLBACKS

    public void UpgradeWarrior()
    {
        _exp.Count -= EXPCost;
        _expCostText.text = "";
        IsOpen = true;
        OpenWarrior.SetWarriorLevel(Level);
        int countWarriors = OpenWarrior.HiringWarriors.RemoveOldHiringWarriors();
        OpenWarrior.HiringWarriors.Warrior = Warrior;
        OpenWarrior.HiringWarriors.AddNewHiringWarriors(countWarriors);
        OpenWarrior.SetNameWarriorLevels(_panelControl);
    }

    #endregion
}
