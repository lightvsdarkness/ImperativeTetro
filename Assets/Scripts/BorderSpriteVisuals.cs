using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BorderSpriteVisuals : MonoBehaviour {
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Start() {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        ManagerVisuals.I.BorderGrid.Add(_spriteRenderer);
    }


}