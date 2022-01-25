using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTower : MonoBehaviour
{
    [SerializeField] private EditTower _parent;
    [SerializeField] private Text _cost;

    private Button _thisButton;
    private bool _fullLevel;

    #region MONO

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
    }

    #endregion

    #region BODY

    void Update()
    {
        if (_parent.TowerObj.NextLevelTower == null && !_fullLevel)
        {
            gameObject.SetActive(false);
            Transform remove = _parent.transform.GetChild(1).transform;
            remove.localPosition = new Vector3(7.629395e-06f, remove.localPosition.y, remove.localPosition.z);
            _fullLevel = true;
        }
        else if (!_fullLevel)
        {
            if (_parent.Money.Count < _parent.TowerObj.NextLevelTower.Cost)
            {
                _thisButton.interactable = false;
            }
            else
            {
                _thisButton.interactable = true;
            }
            _cost.text = _parent.TowerObj.NextLevelTower.Cost.ToString();
        }
    }

    #endregion
}
