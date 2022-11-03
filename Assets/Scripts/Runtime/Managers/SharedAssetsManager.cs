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
                    Debug.LogError("No Stage Editing Manager Exist, Please check the scene.");
                }

                return _instance;
            }
        }

        private static SharedAssetsManager _instance;

        #endregion

        #region Properties and Variables

        [SerializeField] private Sprite customParticleSystemEffectSprite;
        public Sprite CustomParticleSystemEffectSprite => customParticleSystemEffectSprite;

        [SerializeField] private Sprite customPropObjectSprite;

        public Sprite CustomPropObjectSprite => customPropObjectSprite;

        #endregion
        
        private void Awake()
        {
            _instance = this;
        }
    }
}