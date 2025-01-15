using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopUp3DInstance : MonoBehaviour
{
    public TextMeshPro textMesh;
    public TextPopUp3D textPopupHandler;
    public float lifeTime;
    public float fadeDuration;
    private bool duringFade = false;
    public Color opaqueColor = new Color(0, 0, 0, 1);
    public Color transparentColor = new Color(0, 0, 0, 0);

    private void Start()
    {
        StartCoroutine(FadeText(true));
    }

    private void Update()
    {
        transform.position += Vector3.up / 500;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            lifeTime = 10;
            StartCoroutine(FadeText(false));
        }
    }

    private IEnumerator FadeText(bool fadeIn)
    {
        if (duringFade)
        {
            yield break;
        }
        duringFade = true;
        textMesh.color = fadeIn ? transparentColor : opaqueColor;
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (fadeIn)
            {
                textMesh.color = Color.Lerp(transparentColor, opaqueColor, elapsedTime / fadeDuration);
            }
            else
            {
                textMesh.color = Color.Lerp(opaqueColor, transparentColor, elapsedTime / fadeDuration);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (!fadeIn)
        {
            textPopupHandler.textMeshes.Remove(this);
            Destroy(gameObject);
        }
        duringFade = false;
    }
}
