using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    private Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        var color = img.color;
        color.a = 1f;
        img.color = color;

        img.CrossFadeAlpha(1f, 0f, true);
        FadeTo(0f, 1f);
    }
    public void FadeTo(float alpha, float duration)
    {
        img.CrossFadeAlpha(alpha, duration, true);
    }
}
