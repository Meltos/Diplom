using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendWarriors : MonoBehaviour
{
    [SerializeField] private Dictionary<string, Enemy> _activeWarriors = new Dictionary<string, Enemy>();
    [SerializeField] private int _sendDelay;
    [SerializeField] private float _enemyInterval;
    [SerializeField] private float _startTime;
    [SerializeField] private Transform _spanwPoints;
    [SerializeField] private Transform _waypoints;
    [SerializeField] private EnemyHPScript _hp;
    [SerializeField] private CastleHP _castleHp;
    [SerializeField] private Money _money;
    [SerializeField] private Experience _exp;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private bool isBot;
    [SerializeField] private Text _costArmyText;
    [SerializeField] private Text _timeToSendAgainText;

    public int CountAllEnemy;
    public List<Button> Enemies;
    public int CostArmy;
    public Dictionary<string, Enemy> Warriors = new Dictionary<string, Enemy>();

    private int _lastRandom = 0;
    private int _lastlastRandom = 0;
    private bool _checkMismatch;
    private Enemy _enemy;
    private List<Transform> _allWaypoints = new List<Transform>();
    private Button _thisButton;
    private int _fullSendDelay;

    #region MONO

    private void Awake()
    {
        for (int i = 0; i < _waypoints.childCount; i++)
        {
            _allWaypoints.Add(_waypoints.GetChild(i).transform);
        }
        if (!isBot)
        {
            _thisButton = GetComponent<Button>();
            _fullSendDelay = _sendDelay;
            _timeToSendAgainText.text = _sendDelay.ToString();
            _sendCheck = true;
            InvokeRepeating("SendTimer", 0, 1f);
        }
    }

    #endregion

    #region BODY

    void Update()
    {
        if (!isBot)
        {
            if (_money.Count < CostArmy || _sendDelay !)
            {
                _thisButton.interactable = false;
            }
            else
            {
                _thisButton.interactable = true;
            }
            _costArmyText.text = CostArmy.ToString();
        }

        if (_activeWarriors.Count == 0)
        {
            CancelInvoke("SpawnEnemy");
        }
    }

    #endregion

    #region CALLBACKS

    /// <summary>
    ///  Отправка мобой на сторону противника, указанных в панели.
    /// </summary>
    public void SendTroops()
    {
        foreach (KeyValuePair<string, Enemy> kvp in Warriors)
        {
            _activeWarriors.Add(kvp.Key, kvp.Value);
        }
        Warriors.Clear();
        InvokeRepeating("SpawnEnemy", _startTime, _enemyInterval);
        if (!isBot)
        {
            _timeToSendAgainText.text = _sendDelay.ToString();
            InvokeRepeating("SendTimer", 0, 1f);
        }
        if (Enemies.Count > 0)
        {
            _money.Count -= CostArmy;
            foreach (var enemy in Enemies)
            {
                enemy.GetComponent<HiringWarriors>().CountEnemy = 0;
            }
        }
        CountAllEnemy = 0;
        CostArmy = 0;
    }

    /// <summary>
    ///  Цикличный спавн мобов на стороне противника.
    /// </summary>
    private void SpawnEnemy()
    {
        if (_activeWarriors.Count == 0)
        {
            return;
        }
        string warriorRemoveKey = null;
        foreach (KeyValuePair<string, Enemy> kvp in _activeWarriors)
        {
            warriorRemoveKey = kvp.Key;
            _enemy = kvp.Value;
            break;
        }
        var name = warriorRemoveKey.Split();
        _activeWarriors.Remove(warriorRemoveKey);
        Enemy enemy = null;
        int rndCount = 0;
        bool _checkCycle = false;
        while (!_checkCycle)
        {
            rndCount = Random.Range(1, 4);
            if (rndCount != _lastRandom && rndCount != _lastlastRandom && !_checkMismatch)
            {
                _lastRandom = rndCount;
                _lastlastRandom = 0;
                _checkCycle = true;
                _checkMismatch = true;
            }
            else if (rndCount != _lastRandom && rndCount != _lastlastRandom && _checkMismatch)
            {
                _lastlastRandom = _lastRandom;
                _lastRandom = rndCount;
                _checkMismatch = false;
                _checkCycle = true;
            }
        }
        List<Transform> waypoints = new List<Transform>();
        switch (rndCount)
        {
            case 1:
                enemy = Instantiate(_enemy, _spanwPoints.GetChild(0).transform.position + _enemy.GetComponent<MoveToWayPoints>().Offset, Quaternion.identity);
                foreach(var waypoint in _allWaypoints)
                {
                    waypoints.Add(waypoint.GetChild(0).transform);
                }
                enemy.GetComponent<MoveToWayPoints>().Waypoints = waypoints;
                break;
            case 2:
                enemy = Instantiate(_enemy, _spanwPoints.GetChild(1).transform.position + _enemy.GetComponent<MoveToWayPoints>().Offset, Quaternion.identity);
                foreach (var waypoint in _allWaypoints)
                {
                    waypoints.Add(waypoint.GetChild(1).transform);
                }
                enemy.GetComponent<MoveToWayPoints>().Waypoints = waypoints;
                break;
            case 3:
                enemy = Instantiate(_enemy, _spanwPoints.GetChild(2).transform.position + _enemy.GetComponent<MoveToWayPoints>().Offset, Quaternion.identity);
                foreach (var waypoint in _allWaypoints)
                {
                    waypoints.Add(waypoint.GetChild(2).transform);
                }
                enemy.GetComponent<MoveToWayPoints>().Waypoints = waypoints;
                break;
        }
        enemy.CastleHP = _castleHp;
        enemy.Money = _money;
        enemy.EXP = _exp;
        if (name[0] == "Enemy")
            enemy.IsEnemy = true;
        else
            enemy.IsEnemy = false;
        EnemyHPScript hp = Instantiate(_hp, Vector3.zero, Quaternion.identity);
        hp.EnemyObj = enemy;
        hp.Offset = enemy.HpOffset;
        hp.transform.SetParent(_canvas.transform);
        hp.transform.SetAsFirstSibling();
        enemy.Hp = hp;
    }

    /// <summary>
    ///  Отсчёт интервала между отправками мобов.
    /// </summary>
    private void SendTimer()
    {
        _sendDelay--;
        _timeToSendAgainText.text = _sendDelay.ToString();
        if (_sendDelay == 0)
        {
            _sendDelay = _fullSendDelay;
            CancelInvoke("SendTimer");
        }
    }

    #endregion
}
