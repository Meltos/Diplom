using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTowers : MonoBehaviour
{
    [SerializeField] private Money _money;
    [SerializeField] private EditTower _editTower;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private List<GameObject> _menus;
    [SerializeField] private List<GameObject> _panels;

    private Tower _flyingBuilding;
    private Camera _mainCamera;
    private EditTower _upgradeYet;
    private Tower _lastTower;
    private bool _isPause;
    private bool _isGameOver;

    #region MONO

    void Awake()
    {
        _mainCamera = Camera.main;
        Time.timeScale = 1;
    }

    #endregion

    #region BODY

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _flyingBuilding != null && !_isGameOver)
        {
            CancelBuilding();
        }
        else if (_upgradeYet != null && Input.GetKeyDown(KeyCode.Escape) && !_isGameOver)
        {
            CancelUpgrade();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !_isGameOver)
        {
            bool check = false;
            foreach (var panel in _panels)
            {
                if (panel.activeSelf)
                    check = true;
            }
            if (!check)
            {
                foreach (var menu in _menus)
                {
                    if (menu.activeSelf)
                    {
                        menu.SetActive(false);
                        Time.timeScale = 1;
                        _isPause = false;
                    }
                    else
                    {
                        menu.SetActive(true);
                        Time.timeScale = 0;
                        _isPause = true;
                    }
                }
            }
        }
        if (_flyingBuilding != null)
        {
            if (_upgradeYet != null)
            {
                _upgradeYet.TowerObj.transform.GetChild(2).gameObject.SetActive(false);
                Destroy(_upgradeYet.gameObject);
                _upgradeYet = null;
            }
            var groundPlane = new Plane(Vector3.up, -0.3f);
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                _flyingBuilding.transform.position = worldPosition;
                _flyingBuilding.SetTransparent(false);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "TowerPlace")
                    {
                        TowerPlaces place = hit.collider.GetComponent<TowerPlaces>();
                        _flyingBuilding.transform.position = hit.transform.position;
                        if (!place.occupied)
                        {
                            _flyingBuilding.SetTransparent(true);
                        }
                        else
                        {
                            _flyingBuilding.SetTransparent(false);
                        }
                        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && !place.occupied)
                        {
                            _lastTower = _flyingBuilding;
                            _money.CoinMinus(_flyingBuilding.Cost);
                            place.occupied = true;
                            place.Tower = _flyingBuilding;
                            _flyingBuilding.GetComponent<Collider>().isTrigger = false;
                            _flyingBuilding.SetNormal();
                            _flyingBuilding.transform.GetChild(1).gameObject.SetActive(true);
                            _flyingBuilding.IsPlaced = true;
                            _flyingBuilding.transform.localScale = new Vector3(_flyingBuilding.transform.localScale.x - 0.0001f, _flyingBuilding.transform.localScale.y - 0.0001f, _flyingBuilding.transform.localScale.z - 0.0001f);
                            _flyingBuilding.TowerPlace = place;
                            _flyingBuilding.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                            _flyingBuilding = null;
                        }
                    }
                }
            }
        }
        else if (_flyingBuilding == null && Input.GetMouseButtonDown(0) && !_isPause && !_isGameOver)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tower" && !hit.collider.GetComponent<Tower>().IsBot && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    if (_upgradeYet != null)
                    {
                        _upgradeYet.TowerObj.transform.GetChild(2).gameObject.SetActive(false);
                        Destroy(_upgradeYet.gameObject);
                        _upgradeYet = null;
                    }
                    EditTower upgrade = Instantiate(_editTower, Vector3.zero, Quaternion.identity);
                    upgrade.TowerObj = hit.collider.gameObject.GetComponent<Tower>();
                    upgrade.Offset = hit.collider.gameObject.GetComponent<Tower>().Offset;
                    upgrade.Money = _money;
                    upgrade.transform.SetParent(_canvas.transform);
                    upgrade.TowerObj.transform.GetChild(2).gameObject.SetActive(true);
                    _upgradeYet = upgrade;
                }
                else if (hit.collider.tag != "Tower" && _upgradeYet != null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    _upgradeYet.TowerObj.transform.GetChild(2).gameObject.SetActive(false);
                    Destroy(_upgradeYet.gameObject);
                    _upgradeYet = null;
                }
            }
        }
        else if (_flyingBuilding == null && _lastTower != null && Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift) && _money.Count >= _lastTower.Cost && !_isPause && !_isGameOver)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "TowerPlace")
                {
                    TowerPlaces place = hit.collider.GetComponent<TowerPlaces>();
                    if (!place.occupied && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    {
                        Tower lastTower = Instantiate(_lastTower);
                        lastTower.transform.position = hit.transform.position;
                        _money.CoinMinus(lastTower.Cost);
                        place.occupied = true;
                        place.Tower = _lastTower;
                        lastTower.TowerPlace = place;
                        lastTower.IsPlaced = true;
                        lastTower.SetNormal();
                    }
                }
            }
        }
        else if (_flyingBuilding == null && Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift) && !_isPause && !_isGameOver)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tower" && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() 
                    && hit.collider.gameObject.GetComponent<Tower>().NextLevelTower.Cost <= _money.Count 
                    && hit.collider.gameObject.GetComponent<Tower>().NextLevelTower != null
                    && !hit.collider.GetComponent<Tower>().IsBot)
                {
                    EditTower upgrade = Instantiate(_editTower, Vector3.zero, Quaternion.identity);
                    upgrade.TowerObj = hit.collider.gameObject.GetComponent<Tower>();
                    upgrade.Offset = hit.collider.gameObject.GetComponent<Tower>().Offset;
                    upgrade.Money = _money;
                    upgrade.transform.SetParent(_canvas.transform);
                    upgrade.TowerObj.transform.GetChild(2).gameObject.SetActive(true);
                    UpgradeBuilding(upgrade);
                }
            }
        }
    }

    #endregion

    #region CALLBACKS

    /// <summary>
    ///  Установка новой летающей башни и удаление старой.
    /// </summary>
    public void StartPlacingBuilding(Tower buildingPrefab)
    {
        if (_flyingBuilding != null)
        {
            Destroy(_flyingBuilding.gameObject);
        }
        _flyingBuilding = Instantiate(buildingPrefab);
        _flyingBuilding.GetComponent<Collider>().isTrigger = true;
        _flyingBuilding.transform.GetChild(1).gameObject.SetActive(false);
        _flyingBuilding.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        _flyingBuilding.transform.localScale = new Vector3(_flyingBuilding.transform.localScale.x + 0.0001f, _flyingBuilding.transform.localScale.y + 0.0001f, _flyingBuilding.transform.localScale.z + 0.0001f);
    }

    /// <summary>
    ///  Улучшение башни.
    /// </summary>
    public void UpgradeBuilding(EditTower upgrade)
    {
        Tower newTower = upgrade.TowerObj.NextLevelTower;
        Tower newTowerObject = Instantiate(newTower, upgrade.TowerObj.transform.position, Quaternion.identity);
        newTowerObject.TowerPlace = upgrade.TowerObj.TowerPlace;
        newTowerObject.IsPlaced = true;
        newTowerObject.SetNormal();
        upgrade.Money.CoinMinus(newTower.Cost);
        Destroy(upgrade.TowerObj.gameObject);
        Destroy(upgrade.gameObject);
    }
    
    /// <summary>
    ///  Удаление башни.
    /// </summary>
    public void RemoveBuilding(EditTower remove)
    {
        remove.Money.CoinPlus(remove.TowerObj.Cost / 2);
        remove.TowerObj.TowerPlace.occupied = false;
        Destroy(remove.TowerObj.gameObject);
        Destroy(remove.gameObject);
    }

    /// <summary>
    ///  Отмена строительства башни.
    /// </summary>
    public void CancelBuilding()
    {
        if (_flyingBuilding != null)
        {
            Destroy(_flyingBuilding.gameObject);
            _flyingBuilding = null;
        }
    }

    /// <summary>
    ///  Удаление меню улучшения.
    /// </summary>
    public void CancelUpgrade()
    {
        if (_upgradeYet != null)
        {
            _upgradeYet.TowerObj.transform.GetChild(2).gameObject.SetActive(false);
            Destroy(_upgradeYet.gameObject);
            _upgradeYet = null;
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
        foreach (var panel in _panels)
        {
            panel.SetActive(false);
        }
        foreach (var menu in _menus)
        {
            menu.SetActive(false);
        }
        CancelBuilding();
        CancelUpgrade();
        Time.timeScale = 0;
    }

    #endregion
}
