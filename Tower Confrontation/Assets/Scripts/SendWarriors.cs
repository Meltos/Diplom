using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendWarriors : MonoBehaviour
{
    [SerializeField] private Dictionary<string, GameObject> _activeWarriors = new Dictionary<string, GameObject>();
    [SerializeField] private int _sendDelay;
    [SerializeField] private float _enemyInterval;
    [SerializeField] private float _startTime;
    [SerializeField] private Transform _spanwPoints;
    [SerializeField] private Transform _waypoints;
    [SerializeField] private GameObject _hp;
    [SerializeField] private GameObject _castleHp;
    [SerializeField] private GameObject _money;
    [SerializeField] private GameObject _exp;
    [SerializeField] private GameObject _canvas;

    public int CountAllEnemy;
    public List<GameObject> Enemies;
    public int CostArmy;
    public Dictionary<string, GameObject> Warriors = new Dictionary<string, GameObject>();

    private int _lastRandom = 0;
    private int _lastlastRandom = 0;
    private bool _check;
    private GameObject _enemy;
    private bool _sendCheck;
    private List<Transform> _allWaypoints = new List<Transform>();

    #region MONO

    private void Awake()
    {
        for (int i = 0; i < _waypoints.childCount; i++)
        {
            _allWaypoints.Add(_waypoints.GetChild(i).transform);
        }
    }

    #endregion

    #region BODY

    void Update()
    {
        if (_money.GetComponent<Money>().Count < CostArmy || _sendCheck)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
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
        foreach (KeyValuePair<string, GameObject> kvp in Warriors)
        {
            _activeWarriors.Add(kvp.Key, kvp.Value);
        }
        Warriors.Clear();
        _money.GetComponent<Money>().Count -= CostArmy;
        InvokeRepeating("SpawnEnemy", _startTime, _enemyInterval);
        _sendCheck = true;
        Invoke("sendTimer", _sendDelay);
        foreach (var enemy in Enemies)
        {
            enemy.GetComponent<HiringWarriors>().CountEnemy = 0;
        }
        CountAllEnemy = 0;
        CostArmy = 0;
    }

    /// <summary>
    ///  Цикловой спавн мобов на стороне противника.
    /// </summary>
    private void SpawnEnemy()
    {
        if (_activeWarriors.Count == 0)
        {
            return;
        }
        string warriorRemoveKey = null;
        foreach (KeyValuePair<string, GameObject> kvp in _activeWarriors)
        {
            warriorRemoveKey = kvp.Key;
            _enemy = kvp.Value;
            break;
        }
        _activeWarriors.Remove(warriorRemoveKey);
        GameObject enemy = null;
        int rndCount = 0;
        bool _checkRandom = false;
        while (!_checkRandom)
        {
            rndCount = Random.Range(1, 4);
            if (rndCount != _lastRandom && rndCount != _lastlastRandom && !_check)
            {
                _lastRandom = rndCount;
                _lastlastRandom = 0;
                _checkRandom = true;
                _check = true;
            }
            else if (rndCount != _lastRandom && rndCount != _lastlastRandom && _check)
            {
                _lastlastRandom = _lastRandom;
                _lastRandom = rndCount;
                _check = false;
                _checkRandom = true;
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
        enemy.GetComponent<Enemy>().CastleHP = _castleHp;
        enemy.GetComponent<Enemy>().Money = _money;
        enemy.GetComponent<Enemy>().EXP = _exp;
        enemy.GetComponent<Enemy>().IsEnemy = false;
        GameObject hp = Instantiate(_hp, Vector3.zero, Quaternion.identity);
        hp.GetComponent<EnemyHPScript>().EnemyObj = enemy;
        hp.GetComponent<EnemyHPScript>().Offset = enemy.GetComponent<Enemy>().HpOffset;
        hp.transform.SetParent(_canvas.transform);
        hp.transform.SetAsFirstSibling();
        enemy.GetComponent<Enemy>().Hp = hp;
    }

    /// <summary>
    ///  Отсчёт интервала между отправками мобов.
    /// </summary>
    private void sendTimer()
    {
        _sendCheck = false;
    }

    #endregion
}
