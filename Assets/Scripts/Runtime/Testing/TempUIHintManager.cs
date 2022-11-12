using System;
using System.Collections;
using DG.Tweening;
using Runtime.ScriptableObjects;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Testing
{
    public class TempUIHintManager : MonoBehaviour
    {
        #region Singleton

        public static TempUIHintManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<TempUIHintManager>();
                }

                if (!_instance)
                {
                    Debug.LogError("No Temp UI Hint Manager Exist, Please check the scene.");
                }

                return _instance;
            }
        }

        private static TempUIHintManager _instance;

        #endregion

        // simple hint text
        [SerializeField] private RectTransform hintTextRect;
        
        // resource change hint
        [SerializeField] private GameObject resourceChangeHintTextPrefab;
        [SerializeField] private RectTransform moneyResTransform;
        [SerializeField] private RectTransform metalResTransform;
        [SerializeField] private RectTransform woodResTransform;
        [SerializeField] private RectTransform clothResTransform;
        [SerializeField] private RectTransform paintResTransform;
        
        public void HintText(string text)
        {
            hintTextRect.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
            hintTextRect.gameObject.SetActive(true);
            
            // stop the current coroutine and start a new one
            StopCoroutine(DisableHintTextCoroutine());
            StartCoroutine(DisableHintTextCoroutine());
        }

        public void HintResourceChange(GResourceType type, int delta)
        {
            // this expression is really new to me. 
            RectTransform parent = type switch
            {
                GResourceType.Money => moneyResTransform,
                GResourceType.Wood => woodResTransform,
                GResourceType.Metal => metalResTransform,
                GResourceType.Cloth => clothResTransform,
                GResourceType.Paint => paintResTransform,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Resource Type undefined.")
            };
            var instantiatedRect = Instantiate(resourceChangeHintTextPrefab, parent).GetComponent<RectTransform>();
            instantiatedRect
                .DOLocalMove(Vector3.up * Random.Range(50.0f, 75.0f) + Vector3.left * Random.Range(-50.0f, 50.0f),
                    0.35f).SetEase(Ease.OutQuad);
            var tmpUGUI = instantiatedRect.GetComponent<TextMeshProUGUI>();
            tmpUGUI.text = delta < 0 ? delta.ToString() : ("+" + delta.ToString());
            tmpUGUI.color = delta < 0 ? Color.red : Color.green;
            StartCoroutine(DestroyHintTextCoroutine(instantiatedRect));
        }

        private IEnumerator DestroyHintTextCoroutine(RectTransform target, float seconds = 0.5f)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(target.gameObject);
        }

        private IEnumerator DisableHintTextCoroutine()
        {
            yield return new WaitForSeconds(5.0f);
            hintTextRect.gameObject.SetActive(false);
        }
    }
}