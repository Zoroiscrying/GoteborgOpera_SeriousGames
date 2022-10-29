using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Testing
{
    public class BlueprintPackSO : ScriptableObject
    {
        [SerializeField] private List<StagePropBlueprintScriptableObject> stagePropBlueprints;
        public List<StagePropBlueprintScriptableObject> StagePropBlueprints => stagePropBlueprints;
        
    }
}