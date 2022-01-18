using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHPScript : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    public CastleHP CastleHP;
    public GameObject Castle;

    void Update()
    {
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(Castle.transform.position + _offset);
        GetComponent<Slider>().value = CastleHP.HP / CastleHP.MaxHP;
    }
}
