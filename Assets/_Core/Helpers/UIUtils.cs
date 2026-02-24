using UnityEngine;
using UnityEngine.UI;

namespace VTLTools
{
    public static class UIUtils
    {
        public static Vector3 WorldToCanvasPoint(Vector3 worldPos, Canvas canvas)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                canvas.worldCamera,
                out Vector2 localPos
            );
            return localPos;
        }

        public static string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
            return $"{minutes:00}:{seconds:00}";
        }

        public static Color FromHex(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color c);
            return c;
        }

        public static void SetImageAspect(Image img, AspectRatioType type, float targetWidth = -1, float targetHeight = -1, bool isFollowHighestResolution = false)
        {
            if (img == null) return;
            img.SetNativeSize();

            float nativeWidth = img.rectTransform.rect.width;
            float nativeHeight = img.rectTransform.rect.height;
            float aspect = nativeWidth / nativeHeight;
            if (isFollowHighestResolution)
            {
                type = nativeWidth > nativeHeight ? AspectRatioType.FitWidth : AspectRatioType.FitHeight;
            }
            switch (type)
            {
                case AspectRatioType.FitWidth:
                    img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
                    img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetWidth / aspect);
                    break;
                case AspectRatioType.FitHeight:
                    img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
                    img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetHeight * aspect);
                    break;
                case AspectRatioType.Fill:
                    img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
                    img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
                    break;
                case AspectRatioType.Custom:
                    img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
                    img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
                    break;
            }
        }
    }

    public enum AspectRatioType
    {
        FitWidth,
        FitHeight,
        Fill,
        Custom
    }
}