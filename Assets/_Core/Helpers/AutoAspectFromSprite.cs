using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(AspectRatioFitter))]
public class AutoAspectFromSprite : MonoBehaviour
{
    private Image img;
    private AspectRatioFitter fitter;

    private Sprite lastSprite;

    void Awake()
    {
        Setup();
    }

    void LateUpdate()
    {
        UpdateAspect();
    }

    [Button]
    public void UpdateAspect()
    {
        Setup();
        if (img.sprite == null) return;
        if (img.sprite == lastSprite) return;

        lastSprite = img.sprite;

        float w = img.sprite.rect.width;
        float h = img.sprite.rect.height;

        if (h > 0f)
            fitter.aspectRatio = w / h;
    }

    void Setup()
    {
        if (img == null) img = GetComponent<Image>();
        if (fitter == null) fitter = GetComponent<AspectRatioFitter>();
    }
}