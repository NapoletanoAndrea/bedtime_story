using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {
    private Transform moveCam;
    private Vector3 direction;
    private Rigidbody rb;
    private Animator animator;
    private int currentAnimationState = -1;
    
    [SerializeField] private InputReaderSO InputReader;
    [SerializeField] private float speed;
    [SerializeField] private float turnSmoothTime;
    private float turnSmoothVelocity;
    
    private static readonly int State = Animator.StringToHash("State");

    private void Awake() {
        moveCam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
        InputReader.movementEvent += OnMove;
        InputReader.aimPressedEvent += DisableMovement;
        InputReader.aimEvent += Aim;
        InputReader.aimReleasedEvent += EnableMovement;
    }

    private void Update() {
        InputReader.OnMove();
        InputReader.OnAimPressed();
        InputReader.OnAim();
        InputReader.OnAimReleased();
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnMove(Vector2 movement) {
        direction = moveCam.right.normalized * movement.x + moveCam.forward.normalized * movement.y;
        direction.y = 0;

        if (direction.magnitude > .1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void Aim() {
        Debug.Log("Aiming...");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            transform.LookAt(hit.point);
        }
    }

    private void EnableMovement() {
        Debug.Log("Enabled");
        InputReader.movementEvent += OnMove;
    }

    private void DisableMovement() {
        Debug.Log("Disabled");
        InputReader.movementEvent -= OnMove;
        direction = Vector3.zero;
    }

    private void OnDisable() {
        InputReader.movementEvent -= OnMove;
    }
}
