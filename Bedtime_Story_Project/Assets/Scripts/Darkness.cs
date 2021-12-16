using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Darkness : MonoBehaviour {
	[SerializeField] private Light directionalLight;
	[SerializeField] private float lightChangeSeconds;
	[SerializeField] private float deathSeconds;
	[SerializeField] private Slider lightSlider;
	private float count;

	private void Awake() {
		lightSlider.minValue = 0;
		lightSlider.maxValue = deathSeconds;
		lightSlider.value = deathSeconds;
	}

	private void Start() {
		EventManager.Instance.bossDeathEvent += DisableDarknessEvents;
	}

	public void EnableDarknessEvents() {
		EventManager.Instance.darknessEvent += StartCountdown;
		EventManager.Instance.darknessBanishedEvent += StopCountdown;
		StartCoroutine(ChangeLightIntensityCoroutine(.1f));
	}

	public void DisableDarknessEvents() {
		EventManager.Instance.darknessEvent -= StartCountdown;
		EventManager.Instance.darknessBanishedEvent -= StopCountdown;
		StopCountdown();
		StartCoroutine(ChangeLightIntensityCoroutine(1f));
	}
	
	private IEnumerator ChangeLightIntensityCoroutine(float targetIntensity) {
		float c = 0;
		float t = 0;

		float startIntensity = directionalLight.intensity;
        
		while (c <= lightChangeSeconds)
		{
			c += Time.deltaTime;
			t += Time.deltaTime / lightChangeSeconds;
			directionalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
			yield return null;
		}

		directionalLight.intensity = targetIntensity;
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

		EventManager.Instance.OnDeath();
	}

	private void StopCountdown() {
		StopAllCoroutines();
		lightSlider.value = deathSeconds;
		lightSlider.gameObject.SetActive(false);
	}
}
