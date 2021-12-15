using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {
	[SerializeField] private float power;
	[SerializeField] private float range;
	private Rigidbody rb;
	private Transform parent;
	private Vector3 startPosition;
	private Vector3 startShootPosition;
	private bool isPartOfPlayer;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		parent = transform.parent;
		isPartOfPlayer = true;
		startPosition = transform.localPosition;
	}

	private void Start() {
		EventManager.Instance.deathEvent += Reallocate;
	}

	public void Shoot(Vector3 direction) {
		if (!isPartOfPlayer) {
			return;
		}
		transform.parent = null;
		startShootPosition = transform.position;
		isPartOfPlayer = false;
		rb.constraints = RigidbodyConstraints.None;
		rb.AddForce(direction * power, ForceMode.Impulse);
		StartCoroutine(CheckRangeCoroutine());
	}

	private IEnumerator CheckRangeCoroutine() {
		while (Vector3.Distance(startShootPosition, transform.position) < range) {
			yield return null;
		}
		Debug.Log("Stopped");
		rb.useGravity = true;
		gameObject.layer = 0;
		rb.velocity = Vector3.zero;
	}

	public void Reallocate() {
		if (isPartOfPlayer) {
			return;
		}
		
		isPartOfPlayer = true;
		gameObject.layer = 8;
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeAll;
		transform.parent = parent;
		transform.localPosition = startPosition;
	}

	private void OnCollisionEnter(Collision other) {
		ILightInteractable lightInteractable = other.gameObject.GetComponent<ILightInteractable>();
		if (lightInteractable != null) {
			lightInteractable.LightInteract();
		}
		
		if (isPartOfPlayer) {
			return;
		}
		
		ILightShotInteractable lightShotInteractable = other.gameObject.GetComponent<ILightShotInteractable>();
		if (lightShotInteractable != null) {
			lightShotInteractable.LightShotInteract();
		}
		
		StopAllCoroutines();
		rb.useGravity = true;
		gameObject.layer = 0;

		rb.velocity = Vector3.zero;
	}
}
