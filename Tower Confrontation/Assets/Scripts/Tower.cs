using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Renderer _mainRenderer;
    [SerializeField] private Transform _shootElement;
    [SerializeField] private GameObject bullet;

    public float Damage;
    public int Cost;
    public Transform target;
    public float ShootDelay;
    public bool IsPlaced = false;
    public Tower NextLevelTower;
    public TowerPlaces TowerPlace;
    public Vector3 Offset;
    public TowerType Type;

    private bool _isShoot;

    #region BODY

    void Update()
    {
        if (target && IsPlaced && !_isShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    #endregion

    #region CALLBACKS

    /// <summary>
    ///  Установка цвета башни, в зависимости от возможности установить её на выбранное место.
    /// </summary>
    public void SetTransparent(bool available)
    {
        if (available)
        {
            _mainRenderer.material.color = Color.green;
        }
        else
        {
            _mainRenderer.material.color = Color.red;
        }
    }

    /// <summary>
    ///  Установка стандартного цвета башни.
    /// </summary>
    public void SetNormal()
    {
        _mainRenderer.material.color = Color.white;
    }

    /// <summary>
    ///  Выстрел башни с задержкой.
    /// </summary>
    IEnumerator Shoot()
    {
        _isShoot = true;
        yield return new WaitForSeconds(ShootDelay);
        if (target && bullet != null)
        {
            GameObject b = Instantiate(bullet, _shootElement.position, Quaternion.identity);
            if (bullet.tag == "Lightning")
            {
                b.GetComponent<LightningTower>().Target = target;
                b.GetComponent<LightningTower>().Tower = this;
            }
            else
            {
                b.GetComponent<BulletTower>().Target = target;
                b.GetComponent<BulletTower>().Tower = this;
            }
        }
        _isShoot = false;
    }

    #endregion
}
