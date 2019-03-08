using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoVisuals : MonoBehaviour {
    public TetrominoLogic Logic;

    [SerializeField] protected List<SpriteRenderer> _SpriteRenderers = new List<SpriteRenderer>();
    [SerializeField] private Sprite _sprite;
    [SerializeField] private DataColor _dataColor;

    private void Awake()
    {
        if (_SpriteRenderers.Count == 0)
            foreach (Transform square in transform) {
                _SpriteRenderers.Add(square.GetComponent<SpriteRenderer>());
            }
    }
    
    //private void Update()
    //{
    //}

    internal void Initialize(TetrominoLogic logic) {
        Logic = logic;
        _sprite = ManagerVisuals.I.GetTetrominoSprite();
        _dataColor = ManagerVisuals.I.GetTetrominoColor(logic);

        if (_SpriteRenderers.Count == 0)
            foreach (Transform square in transform)
            {
                _SpriteRenderers.Add(square.GetComponent<SpriteRenderer>());
            }

        foreach (var spriteRenderer in _SpriteRenderers) {
            spriteRenderer.sprite = _sprite;
            spriteRenderer.color = _dataColor.Color;

        }
    }
}
