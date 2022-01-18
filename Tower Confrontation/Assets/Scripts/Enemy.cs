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
    public bool IsFreeze;
    public bool IsFire;
    public float TimeFire;
    public float TimeFreeze;
    public List<TowerType> InvulnerabilityToTowers;
    public float BurnDamage;
    public float FreezePower;
    public Tower IceTower;

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
            Money.GetComponent<Money>().CoinPlus(Cost);
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
        yield return new WaitForSeconds(1f);
        TimeFire--;
        if (TimeFire == 0)
            IsFire = false;
        Hp.GetComponent<EnemyHPScript>().Damage(BurnDamage);
        _isBurn = false;
    }

    IEnumerator Freeze()
    {
        _isFrozen = true;
        gameObject.GetComponent<MoveToWayPoints>().Speed *= 1 - FreezePower;
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<MoveToWayPoints>().Speed = gameObject.GetComponent<MoveToWayPoints>().MaxSpeed;
        TimeFreeze--;
        if (TimeFreeze == 0)
            IsFreeze = false;
        _isFrozen = false;
    }

    #endregion
}
