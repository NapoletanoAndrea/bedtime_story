using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Darkness : MonoBehaviour {
	[SerializeField] private float deathSeconds;
	[SerializeField] private Slider lightSlider;
	private float count;
	
	private void Start() {
		lightSlider.minValue = 0;
		lightSlider.maxValue = deathSeconds;
		lightSlider.value = deathSeconds;

		EventManager.Instance.darknessEvent += StartCountdown;
		EventManager.Instance.darknessBanishedEvent += StopCountdown;
	}

	private void StartCountdown() {
		count = deathSeconds;
		lightSlider.gameObject.SetActive(true);
		StartCoroutine(CountdownCoroutine());
	}

	private IEnumerator CountdownCoroutine() {
		while (count >= 0) {
			count -= Time.deltaTime;
			lightSlider.value = count;
			yield return null;
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void StopCountdown() {
		StopAllCoroutines();
		lightSlider.value = deathSeconds;
		lightSlider.gameObject.SetActive(false);
	}
}
