using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OffsetButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector2 OffsetAmount;

    private Image _image;
    private new RectTransform transform => base.transform as RectTransform;
    [SerializeField]
    private Vector2 _defaultPos;
    [SerializeField]
    private Vector2 _offsetPos;
    private Vector2 _targetPos;
    private float _colorOffset;
    public float Speed = 10f;

    private Camera _cam;

    public void Awake()
    {
        _image = GetComponent<Image>();
        _defaultPos = transform.anchoredPosition;
        _targetPos = _defaultPos;
        _offsetPos = _defaultPos + OffsetAmount;
        _cam = Camera.main;
        _colorOffset = Random.value;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetPos = _offsetPos;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetPos = _defaultPos;
    }

    public void Update()
    {
        transform.anchoredPosition = Vector2.Lerp(transform.anchoredPosition, _targetPos, Time.deltaTime * Speed);
        var color = _cam.backgroundColor;
        Vector3 hsvColor;
        Color.RGBToHSV(color, out hsvColor.x, out hsvColor.y, out hsvColor.z);
        hsvColor.x = (hsvColor.x + _colorOffset) % 1;
        _image.color = Color.HSVToRGB(hsvColor.x, hsvColor.y, hsvColor.z);
    }
}