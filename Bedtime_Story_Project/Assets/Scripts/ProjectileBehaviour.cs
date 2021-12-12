using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {
	[SerializeField] private float power;
	private Rigidbody rb;
	private Vector3 startPosition;
	private bool isPartOfPlayer;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		isPartOfPlayer = true;
		startPosition = transform.localPosition;
	}

	public void Shoot(Vector3 direction) {
		if (!isPartOfPlayer) {
			return;
		}
		transform.parent = null;
		isPartOfPlayer = false;
		rb.constraints = RigidbodyConstraints.None;
		rb.AddForce(direction * power, ForceMode.Impulse);
	}

	public void Reallocate(Transform parent) {
		isPartOfPlayer = true;
		gameObject.layer = 8;
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeAll;
		transform.parent = parent;
		transform.localPosition = startPosition;
	}

	private void OnCollisionEnter(Collision other) {
		if (isPartOfPlayer) {
			return;
		}
		rb.useGravity = true;
		gameObject.layer = 0;

		ILightInteractable lightInteractable = other.gameObject.GetComponent<ILightInteractable>();
		if (lightInteractable != null) {
			lightInteractable.LightInteract();
		}
	}
}
