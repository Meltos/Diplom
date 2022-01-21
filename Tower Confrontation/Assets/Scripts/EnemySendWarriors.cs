using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySendWarriors : MonoBehaviour
{
    [SerializeField] private List<Enemy> _wave1;
    [SerializeField] private List<Enemy> _wave2;
    [SerializeField] private List<Enemy> _wave3;
    [SerializeField] private List<Enemy> _wave4;
    [SerializeField] private List<Enemy> _wave5;
    [SerializeField] private List<Enemy> _wave6;
    [SerializeField] private List<Enemy> _wave7;
    [SerializeField] private List<Enemy> _wave8;
    [SerializeField] private List<Enemy> _wave9;
    [SerializeField] private List<Enemy> _wave10;
    [SerializeField] private List<Enemy> _wave11;
    [SerializeField] private List<Enemy> _wave12;
    [SerializeField] private List<Enemy> _wave13;
    [SerializeField] private List<Enemy> _wave14;
    [SerializeField] private List<Enemy> _wave15;
    [SerializeField] private List<Enemy> _wave16;
    [SerializeField] private List<Enemy> _wave17;
    [SerializeField] private List<Enemy> _wave18;
    [SerializeField] private List<Enemy> _wave19;
    [SerializeField] private List<Enemy> _wave20;

    private List<List<Enemy>> _allWaves;
    private int _countWaves;
    private bool _isWaveGo;
    private SendWarriors _thisSendWarriors;

    void Awake()
    {
        _allWaves = new List<List<Enemy>> { _wave1, _wave2, _wave3, _wave4, _wave5, _wave6, _wave7, _wave8, _wave9, _wave10, _wave11,
            _wave12, _wave13, _wave14, _wave15, _wave16, _wave17, _wave18, _wave19, _wave20 };
        _countWaves = 0;
        _thisSendWarriors = GetComponent<SendWarriors>();
    }

    void Update()
    {
        if (!_isWaveGo && _countWaves < 20)
        {
            StartCoroutine(WaveGo());
        }
    }

    IEnumerator WaveGo()
    {
        _isWaveGo = true;
        yield return new WaitForSeconds(15f);
        Dictionary<string, Enemy> waveWarriors = new Dictionary<string, Enemy>();
        for (int i = 0; i < _allWaves[_countWaves].Count; i++)
        {
            waveWarriors.Add("Enemy " + _allWaves[_countWaves][i].name + (i + 1), _allWaves[_countWaves][i]);
        }
        _thisSendWarriors.Warriors = waveWarriors;
        _thisSendWarriors.SendTroops();
        _countWaves++;
        _isWaveGo = false;
    }
}
