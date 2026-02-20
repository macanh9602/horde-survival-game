using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DucDevGame;
using Watermelon;

namespace DucDevGame
{
    public class ExpShopView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button buyButton;
        [SerializeField] private Text priceText;
        [SerializeField] private Text levelText;
        [SerializeField] private Image fillImage;

        [Header("Config")]
        [SerializeField] private int expBonus = 5;
        [SerializeField] private int buyPrice = 4;

        private void Start()
        {
            if (buyButton != null)
            {
                buyButton.onClick.AddListener(OnBuyExpClicked);
            }

            UpdateUI();
        }

        [Button]
        private void OnBuyExpClicked()
        {
            if (ExperienceController.Instance == null)
            {
                //Debug.LogError("[ExpShopView] ExperienceController not found!");
                return;
            }

            // TODO: Check if player has enough currency
            // For now, assume player has currency
            if (HasEnoughCurrency(buyPrice))
            {
                // Deduct currency
                DeductCurrency(buyPrice);

                // Add exp
                ExperienceController.Instance.AddExp(expBonus);

                // Save
                SaveController.MarkAsSaveIsRequired();
                SaveController.Save(forceSave: true);

                //Debug.Log($"[ExpShopView] Bought {expBonus} XP for {buyPrice} currency");
                UpdateUI();
            }
            else
            {
                //Debug.LogWarning("[ExpShopView] Not enough currency!");
                // TODO: Show UI feedback (toast, error message)
            }
        }

        public void UpdateUI()
        {
            //Debug.Log($"<color=green>[DA]</color> UpdateUI");
            if (priceText != null)
                priceText.text = buyPrice.ToString();

            if (levelText != null)
                levelText.text = (ExperienceController.Instance.GetLevel()).ToString();

            if (fillImage != null)
            {
                //0.1 -> 0.315
                float fillAmount = Mathf.Lerp(0.1f, 0.315f, ExperienceController.Instance.GetExpProgress());
                fillImage.fillAmount = fillAmount;
            }
        }

        private bool HasEnoughCurrency(int amount)
        {
            // TODO: Replace with actual currency check
            // Example: return CurrencyController.Instance.GetGold() >= amount;
            return true; // Temporary, always true
        }

        private void DeductCurrency(int amount)
        {
            // TODO: Replace with actual currency deduction
            // Example: CurrencyController.Instance.RemoveGold(amount);
            //Debug.Log($"[ExpShopView] Deducted {amount} currency");
        }
    }
}
