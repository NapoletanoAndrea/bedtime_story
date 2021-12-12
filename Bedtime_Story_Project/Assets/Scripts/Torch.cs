using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, ILightInteractable {
	[SerializeField] private Animator lightAnimator;
	private static readonly int Light = Animator.StringToHash("light");

	public void LightInteract() {
		lightAnimator.SetTrigger(Light);
	}
}
