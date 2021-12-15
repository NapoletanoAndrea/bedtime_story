using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, ILightInteractable {
	[SerializeField] private Animator lightAnimator;
	[SerializeField] private Collider lightTrigger;
	private static readonly int Light = Animator.StringToHash("light");

	public void LightInteract() {
		lightAnimator.SetTrigger(Light);
		lightTrigger.enabled = true;
	}

	private void OnTriggerStay(Collider other) {
		if (other.CompareTag("Player")) {
			EventManager.Instance.OnDarknessBanished();
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			if (!other.GetComponent<PlayerBehaviour>().hasLight) {
				EventManager.Instance.OnDarkness();
			}
		}
	}
}
