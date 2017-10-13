using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionInstructions : MonoBehaviour
{
    public string[] SceneNames;
    public Sprite[] InstructionSprites;
    private Dictionary<string, Sprite> _instructionMap;

    public void Awake()
    {
        Debug.Assert(SceneNames.Length == InstructionSprites.Length, "SceneNames.Length and InstructionSprites.Length not equal!");

        _instructionMap = new Dictionary<string, Sprite>();
        for (int i = 0; i < SceneNames.Length; ++i)
        {
            _instructionMap.Add(SceneNames[i], InstructionSprites[i]);
        }
    }
    public void Start()
    {
        var img = GetComponent<Image>();
        if (_instructionMap.ContainsKey(SceneLoader.ActiveLoader.NextSceneName))
        {
            img.sprite = _instructionMap[SceneLoader.ActiveLoader.NextSceneName];
        }
        else
        {
            img.color = Color.clear;
        }
    }
}