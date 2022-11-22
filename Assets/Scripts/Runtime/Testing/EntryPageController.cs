using System;
using DG.Tweening;
using Runtime.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Runtime.Testing
{
    public class EntryPageController : MonoBehaviour
    {
        [SerializeField] private BaseTouchable2DObject startGameButton;
        [SerializeField] private BaseTouchable2DObject gameHintButton;
        [SerializeField] private CanvasGroup canvasGroupToShowWhenStart;

        private bool _startedGame = false;

        private void OnEnable()
        {
            startGameButton.SubscribeOnObjectButtonDown(StartGame);
            gameHintButton.SubscribeOnObjectButtonDown(ShowGameHint);
        }

        private void OnDisable()
        {
            startGameButton.UnsubscribeOnObjectButtonDown(StartGame);
            gameHintButton.UnsubscribeOnObjectButtonDown(ShowGameHint);
        }

        private void StartGame()
        {
            if (_startedGame)
            {
                return;
            }

            _startedGame = true;
            
            //if 3d
            foreach(TextMeshPro tmp in GetComponentsInChildren<TextMeshPro>())
            {
                tmp.DOColor(new Color(0, 0, 0, 0), 1.0f).SetEase(Ease.OutQuad);
                // r.material.color= Color.white;
            }
            //if 2d with sprite 
            foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material.DOColor(new Color(0, 0, 0, 0), 1.0f).SetEase(Ease.OutQuad);
                // sr.material.color = Color.red;
            }
            
            foreach(Light2D light2D in GetComponentsInChildren<Light2D>())
            {
                DOTween.To(() => light2D.intensity, (intensity) => light2D.intensity = intensity, 0.0f, 1.0f)
                    .SetEase(Ease.OutQuad);
            }
            
            canvasGroupToShowWhenStart.DOFade(1.0f, 0.5f).SetEase(Ease.OutQuad).SetDelay(1.0f);
            DOVirtual.DelayedCall(1.0f, () =>
            {
                this.transform.gameObject.SetActive(false);
                StageEditingManager.Instance.ChangeStageEditPermission(false);
            });
            
        }

        private void ShowGameHint()
        {
            
        }
    }
}