using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Watermelon;
namespace DucDevGame
{
    public class StageView : MonoBehaviour
    {
        [SerializeField] private Text stageInfoText;
        [SerializeField] private SlicedFilledImage stageWaitFillImage;

        public void PlayFillAnimation(float duration, float delay = 0, Action onComplete = null)
        {
            stageWaitFillImage.fillAmount = 1;
            stageWaitFillImage.DOFillAmount(0, duration, delay, onComplete: () => onComplete?.Invoke());
        }
    }
}