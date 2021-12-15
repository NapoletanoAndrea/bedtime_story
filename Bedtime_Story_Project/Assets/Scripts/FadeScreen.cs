using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    private void Start() {
        EventManager.Instance.deathEvent += DeathFade;
        EventManager.Instance.deathEvent += GetUpFade;
    }

    private void DeathFade() {
        StartCoroutine(FadeTo(fadeImage, 0, 1, 1, 0));
    }

    private void GetUpFade() {
        StartCoroutine(FadeTo(fadeImage, 1, 0, 1, 1));
    }

    private IEnumerator FadeTo(Image image, float startAlpha, float targetAlpha, float seconds, float waitSeconds) {
        if (!fadeImage) {
            yield break;
        }

        yield return new WaitForSeconds(waitSeconds);

        image.gameObject.SetActive(true);
        Color startColor = image.color;
        startColor.a = startAlpha;
        image.color = startColor;
        Color color = startColor;

        float count = 0;
        float t = 0;

        while (count <= seconds) {
            count += Time.deltaTime;
            t += Time.deltaTime / seconds;
            color.a = Mathf.Lerp(startColor.a, targetAlpha, t);
            image.color = color;
            yield return null;
        }
    }
}
