using UnityEngine;

namespace Runtime.Managers
{
    public class SharedAssetsManager : MonoBehaviour
    {
        #region Singleton

        public static SharedAssetsManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<SharedAssetsManager>();
                }

                if (!_instance)
                {
                    Debug.LogError("No Shared Asset Manager Exist, Please check the scene.");
                }

                return _instance;
            }
        }

        private static SharedAssetsManager _instance;

        #endregion

        #region Properties and Variables

        [SerializeField] private Sprite customParticleSystemEffectSprite;
        public Sprite CustomParticleSystemEffectSprite => customParticleSystemEffectSprite;
        
        [SerializeField] private Sprite customOrchestraSprite;
        public Sprite CustomOrchestraSprite => customOrchestraSprite;

        [SerializeField] private Sprite customPropObjectSprite;
        public Sprite CustomPropObjectSprite => customPropObjectSprite;

        [SerializeField] private Sprite customLightObjectSprite;

        public Sprite CustomLightObjectSprite => customLightObjectSprite;

        [SerializeField] private GameObject previewStageObjectPrefab;

        public GameObject PreviewStageObjectPrefab => previewStageObjectPrefab;
        
        [SerializeField] private Color blackBgColor; public Color BlackBgColor => blackBgColor;
        [SerializeField] private Color menuColor; public Color MenuColor => menuColor;
        [SerializeField] private Color menuHighlightColor; public Color MenuHighlightColor => menuHighlightColor;
        [SerializeField] private Color textColor; public Color TextColor => textColor;
        [SerializeField] private Color textAccentColor; public Color TextAccentColor => textAccentColor;
        [SerializeField] private Color pinkContrastColor; public Color PinkContrastColor => pinkContrastColor;
        [SerializeField] private Color optionalWhiteColor; public Color OptionalWhiteColor => optionalWhiteColor;

        #endregion
        
        private void Awake()
        {
            _instance = this;
        }
    }
}