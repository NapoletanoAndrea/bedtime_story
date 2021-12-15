using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericLightShotTrigger : MonoBehaviour, ILightShotInteractable {
	public UnityEvent lightShotEvent;

	public void LightShotInteract() {
		lightShotEvent?.Invoke();
	}
}
