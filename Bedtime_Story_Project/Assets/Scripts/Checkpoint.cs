using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	[SerializeField] private Transform checkPositionTransform;
	
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			EventManager.Instance.checkPosition = checkPositionTransform.position;
		}
	}
}
