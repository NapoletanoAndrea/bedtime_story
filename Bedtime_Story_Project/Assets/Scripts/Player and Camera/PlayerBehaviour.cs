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
    public bool hasLight = true;

    [SerializeField] private ProjectileBehaviour projectile;
    [SerializeField] private Transform looker;
    [SerializeField] private InputReaderSO InputReader;
    [SerializeField] private float speed;
    [SerializeField] private float dashSeconds;
    [SerializeField] private float dashDistance;
    [SerializeField] private float rollSeconds;
    [SerializeField] private float rollDistance;
    [SerializeField] private float turnSmoothTime;
    private float turnSmoothVelocity;
    
    private static readonly int State = Animator.StringToHash("State");

    private bool moveEnabled;
    private bool aimEnabled;

    private void Awake() {
        moveCam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
        InputReader.movementEvent += OnMove;
        InputReader.dashEvent += Dash;
        InputReader.aimPressedEvent += DisableMovement;
        InputReader.aimEvent += Aim;
        InputReader.aimReleasedEvent += EnableMovement;

        moveEnabled = true;
        aimEnabled = true;
    }

    private void Start() {
        EventManager.Instance.deathEvent += DisableMovement;
        EventManager.Instance.deathEvent += DisableAim;
        EventManager.Instance.deathEvent += DieAnimation;

        EventManager.Instance.getUpEvent += EnableMovement;
        EventManager.Instance.getUpEvent += EnableAim;
    }

    private void Update() {
        InputReader.OnMove();
        InputReader.OnInteractionKeyPressed();
        InputReader.OnDashKeyPressed();
        InputReader.OnAimPressed();
        InputReader.OnAim();
        InputReader.OnAimReleased();
        InputReader.OnShootPressed();
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnMove(Vector2 movement) {
        if (moveCam == null) {
            moveCam = Camera.main.transform;
        }
        direction = moveCam.right.normalized * movement.x + moveCam.forward.normalized * movement.y;
        direction.y = 0;

        if (direction.magnitude > .1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        
        ChangeAnimationState(direction.magnitude > .1f ? 1 : 0);
    }

    private void Aim() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Ground")))
        {
            looker.LookAt(hit.point);
            Quaternion rotation = looker.rotation;
            transform.rotation = new Quaternion(0, rotation.y, 0, rotation.w);
        }
        
        ChangeAnimationState(2);
    }

    private void Dash() {
        if (hasLight) {
            StartCoroutine(DashCoroutine());
            return;
        }
        StartCoroutine(RollCoroutine());
    }

    private IEnumerator DashCoroutine() {
        Vector3 dashDirection = direction;
        
        DisableMovement();
        DisableAim();
        
        ChangeAnimationState(4);
        
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + dashDirection.normalized * dashDistance;

        Vector3 stopPosition = Vector3.positiveInfinity;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, dashDirection.normalized, out hit, dashDistance + 10)) {
            stopPosition = hit.point;
            Debug.Log(hit.collider.name);
        }
        
        float count = 0;
        float t = 0;
        
        while (count <= dashSeconds) {
            count += Time.fixedDeltaTime;
            t += Time.fixedDeltaTime / dashSeconds;
            rb.MovePosition(Vector3.Lerp(startPosition, endPosition, t));

            if (Vector3.Distance(transform.position + Vector3.up, stopPosition) <= 1.5f) {
                Debug.Log("Break");
                break;
            }
            
            yield return new WaitForFixedUpdate();
        }
        
        EnableAim();
        EnableMovement();
    }

    private IEnumerator RollCoroutine() {
        Vector3 rollDirection = direction;
        
        DisableMovement();
        DisableAim();
        
        ChangeAnimationState(5);
        
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + rollDirection.normalized * rollDistance;

        Vector3 stopPosition = Vector3.positiveInfinity;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, rollDirection.normalized, out hit, rollDistance + 10)) {
            stopPosition = hit.point;
            Debug.Log(hit.collider.name);
        }
        
        float count = 0;
        float t = 0;
        
        while (count <= rollSeconds) {
            count += Time.fixedDeltaTime;
            t += Time.fixedDeltaTime / rollSeconds;
            rb.MovePosition(Vector3.Lerp(startPosition, endPosition, t));

            if (Vector3.Distance(transform.position + Vector3.up, stopPosition) <= .5f) {
                Debug.Log("Break");
                break;
            }
            
            yield return new WaitForFixedUpdate();
        }
        
        EnableAim();
        EnableMovement();
    }

    private void Shoot() {
        Debug.Log("Shot");
        if (hasLight) {
            projectile.Shoot(transform.forward);
            hasLight = false;
            ChangeAnimationState(3);
            EventManager.Instance.OnDarkness();
        }
    }

    private void DieAnimation() {
        hasLight = true;
        EventManager.Instance.OnDarknessBanished();
        ChangeAnimationState(7);
        StartCoroutine(GetUpCoroutine());
    }

    private IEnumerator GetUpCoroutine() {
        yield return new WaitForSeconds(1);
        EventManager.Instance.OnFadedScreen();
        transform.position = EventManager.Instance.checkPosition;
        ChangeAnimationState(6);
        yield return new WaitForSeconds(7.35f);
        EventManager.Instance.OnGettingUp();
    }

    private void EnableMovement() {
        if(moveEnabled) return;
        Debug.Log("Enabled");
        InputReader.movementEvent += OnMove;
        InputReader.shootPressedEvent -= Shoot;
        InputReader.dashEvent += Dash;
        moveEnabled = true;
    }

    private void DisableMovement() {
        if (!moveEnabled) return;
        Debug.Log("Disabled");
        InputReader.movementEvent -= OnMove;
        InputReader.shootPressedEvent += Shoot;
        InputReader.dashEvent -= Dash;
        direction = Vector3.zero;
        moveEnabled = false;
    }

    private void DisableAim() {
        if (!aimEnabled) return;
        InputReader.aimPressedEvent -= DisableMovement;
        InputReader.aimEvent -= Aim;
        InputReader.aimReleasedEvent -= EnableMovement;
        aimEnabled = false;
    }

    private void EnableAim() {
        if(aimEnabled) return;
        InputReader.aimPressedEvent += DisableMovement;
        InputReader.aimEvent += Aim;
        InputReader.aimReleasedEvent += EnableMovement;
        aimEnabled = true;
    }

    private void OnDisable() {
        InputReader.movementEvent -= OnMove;
        InputReader.dashEvent -= Dash;
        InputReader.aimPressedEvent -= DisableMovement;
        InputReader.aimEvent -= Aim;
        InputReader.aimReleasedEvent -= EnableMovement;
        InputReader.shootPressedEvent -= Shoot;
    }

    private void ChangeAnimationState(int stateNumber) {
        if (currentAnimationState == stateNumber) {
            return;
        }
        animator.SetInteger(State, stateNumber);
        currentAnimationState = stateNumber;
    }

    private void OnCollisionEnter(Collision other) {
        var proj = other.gameObject.GetComponent<ProjectileBehaviour>();
        if (proj != null) {
            proj.Reallocate();
            hasLight = true;
            EventManager.Instance.OnDarknessBanished();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position + Vector3.up, direction * 100);
    }
}