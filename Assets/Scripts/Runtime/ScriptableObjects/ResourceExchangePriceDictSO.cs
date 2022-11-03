using System.Collections.Generic;
using Runtime.ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Testing
{
    [CreateAssetMenu(fileName = "ResourceExchangePriceDictionary", menuName = "GoteborgProject/ResourcePriceDictionary", order = 1)]
    public class ResourceExchangePriceDictSO : ScriptableObject
    {
        public IDictionary<GResourceType, int> ResourceExchangeDict => resourceExchangeDict;

        [SerializeField]
        private GenericDictionary<GResourceType, int> resourceExchangeDict = 
            new()
            {
                { GResourceType.Money, 1},
                { GResourceType.Wood, 1},
                { GResourceType.Metal, 2},
                { GResourceType.Cloth, 1},
                { GResourceType.Paint, 1},
            };
        
    }
}