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
    [SerializeField] private List<Enemy> _wave21;
    [SerializeField] private List<Enemy> _wave22;
    [SerializeField] private List<Enemy> _wave23;
    [SerializeField] private List<Enemy> _wave24;
    [SerializeField] private List<Enemy> _wave25;
    [SerializeField] private List<Enemy> _wave26;
    [SerializeField] private List<Enemy> _wave27;
    [SerializeField] private List<Enemy> _wave28;
    [SerializeField] private List<Enemy> _wave29;
    [SerializeField] private List<Enemy> _wave30;
    [SerializeField] private List<Enemy> _wave31;
    [SerializeField] private List<Enemy> _wave32;
    [SerializeField] private List<Enemy> _wave33;
    [SerializeField] private List<Enemy> _wave34;
    [SerializeField] private List<Enemy> _wave35;
    [SerializeField] private List<Enemy> _wave36;
    [SerializeField] private List<Enemy> _wave37;
    [SerializeField] private List<Enemy> _wave38;
    [SerializeField] private List<Enemy> _wave39;
    [SerializeField] private List<Enemy> _wave40;
    [SerializeField] private List<Enemy> _wave41;
    [SerializeField] private List<Enemy> _wave42;
    [SerializeField] private List<Enemy> _wave43;
    [SerializeField] private List<Enemy> _wave44;
    [SerializeField] private List<Enemy> _wave45;
    [SerializeField] private List<Enemy> _allFullLevelEnemy;

    private List<List<Enemy>> _allWaves;
    private int _countWaves;
    private bool _isWaveGo;
    private SendWarriors _thisSendWarriors;
    private float _timerRandomWave = 20;
    private int _countWavesToArmagedon;

    void Awake()
    {
        _allWaves = new List<List<Enemy>> { _wave1, _wave2, _wave3, _wave4, _wave5, _wave6, _wave7, _wave8, _wave9, _wave10, _wave11,
            _wave12, _wave13, _wave14, _wave15, _wave16, _wave17, _wave18, _wave19, _wave20, _wave21,_wave22,_wave23,_wave24,_wave25
        ,_wave26,_wave27,_wave28,_wave29,_wave30,_wave31,_wave32,_wave33,_wave34,_wave35,_wave36,_wave37,_wave38,_wave39,_wave40,_wave41
        ,_wave42,_wave43,_wave44,_wave45};
        _countWaves = 0;
        _thisSendWarriors = GetComponent<SendWarriors>();
    }

    void Update()
    {
        if (!_isWaveGo && _countWaves < 45)
        {
            StartCoroutine(WaveGo());
        }
        else if(!_isWaveGo && _countWaves >= 45)
        {
            StartCoroutine(RandomWaveGo());
        }
    }

    IEnumerator WaveGo()
    {
        _isWaveGo = true;
        yield return new WaitForSeconds(20f);
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

    IEnumerator RandomWaveGo()
    {
        _isWaveGo = true;
        yield return new WaitForSeconds(_timerRandomWave);
        _countWavesToArmagedon++;
        if (_countWavesToArmagedon == 5)
            _timerRandomWave = 6f;
        Dictionary<string, Enemy> waveWarriors = new Dictionary<string, Enemy>();
        for (int i = 0; i < 12; i++)
        {
            int indexEnemy = Random.Range(0, 3);
            waveWarriors.Add("Enemy " + _allFullLevelEnemy[indexEnemy] + (i + 1), _allFullLevelEnemy[indexEnemy]);
        }
        _thisSendWarriors.Warriors = waveWarriors;
        _thisSendWarriors.SendTroops();
        _countWaves++;
        _isWaveGo = false;
    }
}
