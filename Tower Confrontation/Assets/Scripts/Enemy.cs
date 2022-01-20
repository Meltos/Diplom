using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _attackDelay;

    public float HP;
    public float MaxHP;
    public int Cost;
    public int EXPCost;
    public int EXPReward;
    public GameObject Hp;
    public GameObject CastleHP;
    public GameObject Money;
    public GameObject EXP;
    public bool IsFreeze;
    public bool IsFire;
    public float TimeFire;
    public float TimeFreeze;
    public List<TowerType> InvulnerabilityToTowers;
    public float BurnDamage;
    public float FreezePower;
    public Tower IceTower;
    public Vector3 HpOffset;
    public GameObject FireEffect;
    public GameObject FreezeEffect;
    public bool IsEnemy;

    private bool _isDeath;
    private bool _isAttack;
    private bool _isBurn;
    private bool _isFrozen;

    #region MONO

    private void Awake()
    {
        HP = MaxHP;
    }

    #endregion

    #region BODY

    void Update()
    {
        if (HP <= 0 && !_isDeath)
        {
            _isDeath = true;
            if (IsEnemy)
                Money.GetComponent<Money>().CoinPlus(Cost);
            else
                EXP.GetComponent<Experience>().ExpPlus(EXPReward);
            gameObject.GetComponent<MoveToWayPoints>().Waypoints = new List<Transform>();
            gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isDead", true);
            StartCoroutine(Death());
        }
        else
        {
            if (!_isBurn && IsFire && TimeFire > 0)
            {
                StartCoroutine(Burn());
            }
            if (!_isFrozen && IsFreeze && TimeFreeze > 0)
            {
                StartCoroutine(Freeze());
            }
        }
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Castle"))
        {
            if (!_isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        _isAttack = true;
        StartCoroutine(AttackAnimation());
        yield return new WaitForSeconds(_attackDelay);
        gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isAttack", false);
        if (!_isDeath)
        {
            CastleHP.GetComponent<CastleHP>().Damage(_damage);
        }
        _isAttack = false;
    }

    IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.75f);
        gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isAttack", true);
    }

    IEnumerator Burn()
    {
        _isBurn = true;
        if (!FireEffect.activeSelf)
            FireEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        TimeFire--;
        if (TimeFire == 0)
        {
            IsFire = false;
            FireEffect.SetActive(false);
        }
        if (HP > 0)
            Hp.GetComponent<EnemyHPScript>().Damage(BurnDamage);
        _isBurn = false;
    }

    IEnumerator Freeze()
    {
        _isFrozen = true;
        if (!FreezeEffect.activeSelf)
            FreezeEffect.SetActive(true);
        gameObject.GetComponent<MoveToWayPoints>().Speed *= 1 - FreezePower;
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<MoveToWayPoints>().Speed = gameObject.GetComponent<MoveToWayPoints>().MaxSpeed;
        TimeFreeze--;
        if (TimeFreeze == 0)
        {
            IsFreeze = false;
            FreezeEffect.SetActive(false);
        }
        _isFrozen = false;
    }

    #endregion
}
