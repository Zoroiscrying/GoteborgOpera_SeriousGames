using System.Collections;
using System.Collections.Generic;
using Runtime.ScriptableObjects;
using UnityEditor;
using UnityEngine;

[CustomPreview(typeof(BaseStageObjectBlueprintSO))]
public class StagePropBlueprintPreview : ObjectPreview
{
    private BaseStageObjectBlueprintSO bpTarget;

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        if (!bpTarget)
        {
            bpTarget = target as BaseStageObjectBlueprintSO;
        }

        if (bpTarget && bpTarget.PreviewSprite)
        {
            Texture2D texture = AssetPreview.GetAssetPreview(bpTarget.PreviewSprite);
            GUI.Label(r, target.name + " is being previewed");
            GUI.DrawTexture(r, texture);
        }
        //base.OnPreviewGUI(r, background);
    }

    public override bool HasPreviewGUI()
    {
        return base.HasPreviewGUI() && target;
    }
}
