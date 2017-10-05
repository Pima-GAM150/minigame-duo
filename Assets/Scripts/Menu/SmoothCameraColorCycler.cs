using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SmoothCameraColorCycler : MonoBehaviour
{
    public bool Enabled = true;

    public float InterpSpeed = 6f;

    [Space(10)]
    private Color _currentColor;

    private Camera _cam;
    private float _h;
    private float _s;
    private float _v;

    public void Awake()
    {
        _cam = GetComponent<Camera>();

        Color.RGBToHSV(_cam.backgroundColor, out _h, out _s, out _v);
    }

    public void Update()
    {
        if (Enabled && _cam)
        {
            _currentColor = Color.HSVToRGB(_h, _s, _v);
            _cam.backgroundColor = _currentColor;
            _h += (Time.deltaTime / 100f) * InterpSpeed;
            if (_h > 1)
                _h = 0f;
        }
    }
}