using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VTLTools;
using Watermelon;

namespace DucDevGame
{
    public class HealthBarBehavior : MonoBehaviour
    {
        [Space]
        [SerializeField] private Transform healthBarTransform;
        [SerializeField] private SlicedFilledImage healthBarFill;
        [SerializeField] private SlicedFilledImage maskHealthBarFill;
        [SerializeField] private SlicedFilledImage energyBarFill;
        [SerializeField] private Image healthBarBackground;
        [SerializeField] private RectTransform healthBGRect;
        [SerializeField] private CanvasGroup healthBarCanvasGroup;

        [Space]
        [SerializeField] private Color enemyHealthColor = Color.red;
        [SerializeField] private Color allyHealthColor = Color.green;
        [SerializeField] private Color energyColor = Color.blue;

        [Space]
        [ShowInInspector, ReadOnly] private Vector3 defaultOffset = new Vector3(0, 0.5f, 0);
        [ShowInInspector, ReadOnly] private Transform parentTransform;

        [Space]
        [SerializeField] private List<HealthBarViewConfig> healthBarViewSettings;

        [Space]
        private bool isDisabled;
        private bool isInit;
        private IHealth targetHealth;
        private Camera mainCamera;
        private Tween fadeTween;
        private Tween maskTween;
        public void Init(Transform parentTransform, IHealth targetHealth, Vector3 defaultOffset, LevelStar rarity, bool showAlways = true)
        {
            isInit = true;
            this.parentTransform = parentTransform;
            this.targetHealth = targetHealth;
            this.defaultOffset = defaultOffset;
            mainCamera = Camera.main;

            // Reset bar parent
            healthBarTransform.SetParent(null);
            healthBarTransform.gameObject.SetActive(true);

            healthBarCanvasGroup.alpha = showAlways ? 1f : 0f;
            RedrawFrameBG(rarity);
            healthBarFill.fillAmount = maskHealthBarFill.fillAmount = GetHealthPercent();
        }

        public void FollowUpdate()
        {
            if (isInit)
            {
                healthBarTransform.position = parentTransform.position + defaultOffset;
                healthBarTransform.rotation = mainCamera.transform.rotation;
            }
        }

        public void OnHealthChanged()
        {
            if (targetHealth == null || isDisabled) return;
            healthBarFill.fillAmount = GetHealthPercent();

            maskTween?.Kill();
            maskTween = maskHealthBarFill.DOFillAmount(healthBarFill.fillAmount, 0.3f).SetEase(Ease.InQuint);
        }

        public void ActiveBar(bool isActive, bool isImmediate = false)
        {
            if (isActive == !isDisabled) return;
            isDisabled = !isActive;
            if (isImmediate)
                healthBarCanvasGroup.gameObject.SetActive(isActive);
            else
            {
                fadeTween?.Kill();
                if (isActive)
                {
                    healthBarCanvasGroup.gameObject.SetActive(true);
                    healthBarCanvasGroup.alpha = 0f;
                    fadeTween = healthBarCanvasGroup.DOFade(1f, 0.3f).SetEase(Ease.InOutSine);
                }
                else
                {
                    fadeTween = healthBarCanvasGroup.DOFade(0f, 0.3f).SetEase(Ease.InOutSine).OnComplete(() =>
                    {
                        healthBarCanvasGroup.gameObject.SetActive(false);
                    });
                }
            }
        }


        public void RedrawFrameBG(LevelStar rarity)
        {
            HealthBarViewConfig config = healthBarViewSettings.Find(c => c.rarity == rarity);
            if (config != null)
            {
                healthBarBackground.sprite = config.healthBarSprite;
                healthBGRect.sizeDelta = new Vector2(config.healthBarWidth, healthBGRect.sizeDelta.y);
            }
        }

        public float GetHealthPercent()
        {
            Debug.Log($"<color=green>[DA]</color> {targetHealth.CurrentHealth} / {targetHealth.MaxHealth}");
            return (float)targetHealth.CurrentHealth / targetHealth.MaxHealth;
        }

        public bool CheckHealthPercent(float percent)
        {
            return GetHealthPercent() > percent;
        }

        public void DestroySelf()
        {
            ObjectPool.Recycle(this);
        }

        [Button]
        private void Test(LevelStar rarity)
        {
            HealthBarViewConfig config = healthBarViewSettings.Find(c => c.rarity == rarity);
            healthBarBackground.sprite = config.healthBarSprite;
            healthBGRect.sizeDelta = new Vector2(config.healthBarWidth, healthBGRect.sizeDelta.y);

        }

    }
}