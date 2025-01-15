using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopUp3D : MonoBehaviour
{
    public static TextPopUp3D instance;
    [SerializeField] private GameObject textMeshPrefab;
    public List<TextPopUp3DInstance> textMeshes;

    private void Awake()
    {
        instance = this;
    }

    private void OnTextPopUp(Vector3 position, string text, float fontSize, Color textColor, float fadeDuration, float lifeTime)
    {
        TextPopUp3DInstance textInstance = Instantiate(textMeshPrefab, position, 
            textMeshPrefab.transform.rotation).GetComponent<TextPopUp3DInstance>();
        textInstance.textMesh.text = text;
        textInstance.opaqueColor = textColor;
        textInstance.opaqueColor.a = 1;
        textInstance.textMesh.fontSize = fontSize;
        textInstance.transparentColor = new Color(textColor.r, textColor.g, textColor.b, 0);
        textInstance.fadeDuration = fadeDuration;
        textInstance.lifeTime = lifeTime;
        textInstance.textPopupHandler = this;
        textMeshes.Add(textInstance);
    }

    public static void PopUpText(Vector3 position, string text, float fontSize, Color textColor, float fadeDuration, float lifeTime)
    {
        instance.OnTextPopUp(position, text, fontSize, textColor, fadeDuration, lifeTime);
    }
}
