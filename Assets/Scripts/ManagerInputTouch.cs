using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;

public class ManagerInputTouch : SingletonManager<ManagerInputTouch> {

    [SerializeField] private int _sensitivityHorizontal = 20;
    [SerializeField] private int _sensitivityVertical = 20;
    [SerializeField] private float _sensitivityAxisThreshhold = 10;

    private Vector2 _previousUnitPosition = Vector2.zero;
    private Vector2 _direction = Vector2.zero;

    private bool moved = false;

    private Touch _touch;

    //private void Start() {
    //}

#if UNITY_ANDROID
    private void FixedUpdate() {
        CheckUserInput();
    }
#endif

    public void CheckUserInput() {
        if (Input.touchCount > 0) {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began) {
                _previousUnitPosition = new Vector2(_touch.position.x, transform.position.y);
                moved = false;
            }
            else if (_touch.phase == TouchPhase.Moved) {
                Vector2 touchDeltaPosition = _touch.deltaPosition;
                _direction = touchDeltaPosition.normalized;

                var touchMovedHor = Mathf.Abs(_touch.position.x - _previousUnitPosition.x);
                var touchMovedVert = Mathf.Abs(_touch.position.x - _previousUnitPosition.x);
                bool touchHorizontal = _touch.deltaPosition.y > -10 &&
                                       _touch.deltaPosition.y < _sensitivityAxisThreshhold;
                bool touchVertical = _touch.deltaPosition.x > -10 &&
                                       _touch.deltaPosition.x < _sensitivityAxisThreshhold;

                if (touchMovedHor >= _sensitivityHorizontal && touchHorizontal) {
                    if (_direction.x < 0)       // Left
                    {
                        if (ManagerTetroGame.I.CurrentTetromino != null)
                            ManagerTetroGame.I.CurrentTetromino.MoveLeft();
                    }
                    else if(_direction.x > 0)   // Right
                    {
                        if (ManagerTetroGame.I.CurrentTetromino != null)
                            ManagerTetroGame.I.CurrentTetromino.MoveRight();
                    }

                    _previousUnitPosition = _touch.position;
                    moved = true;
                }
                // Down
                else if (touchMovedVert >= _sensitivityVertical && _direction.y < 0 && touchVertical) {
                    if (ManagerTetroGame.I.CurrentTetromino != null)
                        ManagerTetroGame.I.CurrentTetromino.TryMoveDown();

                    _previousUnitPosition = _touch.position;
                    moved = true;
                }
            }

            else if (_touch.phase == TouchPhase.Ended) {
                if (!moved)    //&& _touch.position.x > Screen.width / 6
                { 
                    if (ManagerTetroGame.I.CurrentTetromino != null)
                        ManagerTetroGame.I.CurrentTetromino.Rotate();
                }
            }

        }
    }
}
