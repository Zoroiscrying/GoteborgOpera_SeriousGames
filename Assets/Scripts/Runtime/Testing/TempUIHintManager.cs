using System.Collections;
using TMPro;
using UnityEngine;

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

        [SerializeField] private RectTransform hintTextRect;
        
        public void HintText(string text)
        {
            hintTextRect.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
            hintTextRect.gameObject.SetActive(true);
            
            // stop the current coroutine and start a new one
            StopCoroutine(DisableHintTextCoroutine());
            StartCoroutine(DisableHintTextCoroutine());
        }

        private IEnumerator DisableHintTextCoroutine()
        {
            yield return new WaitForSeconds(5.0f);
            hintTextRect.gameObject.SetActive(false);
        }
    }
}