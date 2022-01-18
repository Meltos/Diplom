using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTowers : MonoBehaviour
{
    [SerializeField] private Money _money;
    [SerializeField] private GameObject _editTower;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private List<GameObject> _menus;
    [SerializeField] private List<GameObject> _panels;

    private Tower _flyingBuilding;
    private Camera _mainCamera;
    private GameObject _upgradeYet;
    private Tower _lastTower;

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
        if (Input.GetKeyDown(KeyCode.Escape) && _flyingBuilding != null)
        {
            CancelBuilding();
        }
        else if (_upgradeYet != null && Input.GetKeyDown(KeyCode.Escape))
        {
            CancelUpgrade();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
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
                    }
                    else
                    {
                        menu.SetActive(true);
                        Time.timeScale = 0;
                    }
                }
            }
        }
        if (_flyingBuilding != null)
        {
            if (_upgradeYet != null)
            {
                _upgradeYet.GetComponent<EditTower>().TowerObj.transform.GetChild(2).gameObject.SetActive(false);
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
                            _money.CoinMinus(_flyingBuilding.Cost);
                            place.occupied = true;
                            place.Tower = _flyingBuilding.GetComponent<Tower>();
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
        else if (_flyingBuilding == null && Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tower" && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    if (_upgradeYet != null)
                    {
                        _upgradeYet.GetComponent<EditTower>().TowerObj.transform.GetChild(2).gameObject.SetActive(false);
                        Destroy(_upgradeYet.gameObject);
                        _upgradeYet = null;
                    }
                    GameObject upgrade = Instantiate(_editTower, Vector3.zero, Quaternion.identity);
                    upgrade.GetComponent<EditTower>().TowerObj = hit.collider.gameObject;
                    upgrade.GetComponent<EditTower>().Offset = hit.collider.gameObject.GetComponent<Tower>().Offset;
                    upgrade.GetComponent<EditTower>().Money = _money;
                    upgrade.transform.SetParent(_canvas.transform);
                    upgrade.GetComponent<EditTower>().TowerObj.transform.GetChild(2).gameObject.SetActive(true);
                    _upgradeYet = upgrade;
                }
                else if (hit.collider.tag != "Tower" && _upgradeYet != null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    _upgradeYet.GetComponent<EditTower>().TowerObj.transform.GetChild(2).gameObject.SetActive(false);
                    Destroy(_upgradeYet.gameObject);
                    _upgradeYet = null;
                }
            }
        }
        else if (_flyingBuilding == null && _lastTower != null && Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift) && _money.Count >= _lastTower.Cost)
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
                        _lastTower = Instantiate(_lastTower);
                        _lastTower.transform.position = hit.transform.position;
                        _money.CoinMinus(_lastTower.Cost);
                        place.occupied = true;
                        place.Tower = _lastTower.GetComponent<Tower>();
                        _lastTower.TowerPlace = place;
                        _lastTower.IsPlaced = true;
                        _lastTower.SetNormal();
                    }
                }
            }
        }
        else if (_flyingBuilding == null && Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tower" && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() 
                    && hit.collider.gameObject.GetComponent<Tower>().Cost <= _money.Count 
                    && hit.collider.gameObject.GetComponent<Tower>().NextLevelTower != null)
                {
                    GameObject upgrade = Instantiate(_editTower, Vector3.zero, Quaternion.identity);
                    upgrade.GetComponent<EditTower>().TowerObj = hit.collider.gameObject;
                    upgrade.GetComponent<EditTower>().Offset = hit.collider.gameObject.GetComponent<Tower>().Offset;
                    upgrade.GetComponent<EditTower>().Money = _money;
                    upgrade.transform.SetParent(_canvas.transform);
                    upgrade.GetComponent<EditTower>().TowerObj.transform.GetChild(2).gameObject.SetActive(true);
                    UpgradeBuilding(upgrade.GetComponent<EditTower>());
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
        _lastTower = buildingPrefab;
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
        Tower newTower = upgrade.TowerObj.GetComponent<Tower>().NextLevelTower;
        GameObject newTowerObject = Instantiate(newTower.gameObject, upgrade.TowerObj.transform.position, Quaternion.identity);
        newTowerObject.GetComponent<Tower>().TowerPlace = upgrade.TowerObj.GetComponent<Tower>().TowerPlace;
        newTowerObject.GetComponent<Tower>().IsPlaced = true;
        newTowerObject.GetComponent<Tower>().SetNormal();
        upgrade.Money.CoinMinus(newTower.Cost);
        Destroy(upgrade.TowerObj.gameObject);
        Destroy(upgrade.gameObject);
    }
    
    /// <summary>
    ///  Удаление башни.
    /// </summary>
    public void RemoveBuilding(EditTower remove)
    {
        remove.Money.CoinPlus(remove.TowerObj.GetComponent<Tower>().Cost / 2);
        remove.TowerObj.GetComponent<Tower>().TowerPlace.occupied = false;
        Destroy(remove.TowerObj.gameObject);
        Destroy(remove.gameObject);
    }

    public void CancelBuilding()
    {
        if (_flyingBuilding != null)
        {
            Destroy(_flyingBuilding.gameObject);
            _flyingBuilding = null;
            _lastTower = null;
        }
    }

    public void CancelUpgrade()
    {
        if (_upgradeYet != null)
        {
            _upgradeYet.GetComponent<EditTower>().TowerObj.transform.GetChild(2).gameObject.SetActive(false);
            Destroy(_upgradeYet.gameObject);
            _upgradeYet = null;
        }
    }

    #endregion
}
