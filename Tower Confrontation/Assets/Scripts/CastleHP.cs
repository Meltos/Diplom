using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHP : MonoBehaviour
{
    [SerializeField] private bool _isBot;
    [SerializeField] private BuildingTowers _buildingTowers;
    [SerializeField] private GameObject _gameOverPanel;

    public float HP;
    public float MaxHP;

    #region BODY

    void Update()
    {
        GetComponent<Text>().text = HP.ToString();

        if (_isBot && HP <= 0)
        {
            _buildingTowers.GameOver();
            _gameOverPanel.SetActive(true);
        }
        else if (!_isBot && HP <= 0)
        {
            _buildingTowers.GameOver();
            _gameOverPanel.SetActive(true);
        }
    }

    #endregion

    #region CALLBACKS

    /// <summary>
    ///  Вычетание из здоровья замка количества переданного урона.
    /// </summary>
    public void Damage(float DMGcount)
    {
        HP -= DMGcount;
    }

    #endregion
}
