using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SmoothCameraColorCycler : MonoBehaviour
{
    public bool Enabled = true;

    [Space(10)]
    private Color _currentColor;
    public float InterpSpeed = 6f;
    private Camera _cam;
    private float _hue;
    private float _sat;
    private float _val;

    public void Awake()
    {
        _cam = GetComponent<Camera>();

        Color.RGBToHSV(_cam.backgroundColor, out _hue, out _sat, out _val);
    }
    public void Update()
    {
        if (Enabled && _cam)
        {
            _currentColor = Color.HSVToRGB(_hue, _sat, _val);
            _cam.backgroundColor = _currentColor;
            _hue += (Time.deltaTime / 100f) * InterpSpeed;
            if (_hue > 1)
                _hue = 0f;
        }
    }
}