using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjects/Input Reader")]
public class InputReaderSO : ScriptableObject {
    [SerializeField] private string horizontalAxisString;
    [SerializeField] private string verticalAxisString;
    [SerializeField] private KeyCode dashKey;

    public UnityAction<Vector2> movementEvent;
    public UnityAction dashEvent;
    public UnityAction aimPressedEvent;
    public UnityAction aimEvent;
    public UnityAction aimReleasedEvent;

    public void OnMove() {
        movementEvent?.Invoke(new Vector2(Input.GetAxisRaw(horizontalAxisString), Input.GetAxisRaw(verticalAxisString)));
    }

    public void OnDashKeyPressed() {
        if (Input.GetKeyDown(dashKey)) {
            dashEvent?.Invoke();
        }
    }

    public void OnAimPressed() {
        if (Input.GetMouseButtonDown(1)) {
            aimPressedEvent?.Invoke();
        }
    }

    public void OnAim() {
        if (Input.GetMouseButton(1)) {
            aimEvent?.Invoke();
        }
    }

    public void OnAimReleased() {
        if (Input.GetMouseButtonUp(1)) {
            aimReleasedEvent?.Invoke();
        }
    }
}
