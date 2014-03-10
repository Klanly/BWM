using UnityEditor;
using UnityEngine;


public class ModelPostprocess : AssetPostprocessor
{
	// This method is called just before importing an FBX.
	void OnPreprocessModel()
	{
		ModelImporter mi = (ModelImporter)assetImporter;
		if((assetPath.Contains("Sk_Female") && assetPath.Contains("@"))
		   ||(assetPath.Contains("Sk_Male") && assetPath.Contains("@")))
		{
			mi.animationType = ModelImporterAnimationType.Human;
			foreach (ModelImporterClipAnimation ma in mi.clipAnimations)
			{
				ma.maskType = ClipAnimationMaskType.CopyFromOther;
				//var prefab = Object.Instantiate(Resources.Load("Prefabs/Models/body/Sk_Female")) as GameObject;
				//ma.maskSource = Resources.Load("New Human Template.ht") as UnityEditorInternal.AvatarMask;
			}
		}

		// 模型不需要导出动画
		if (assetPath.Contains("Female_Body") || assetPath.Contains("Male_Body"))
		{
			mi.importAnimation = false;
			mi.animationType = ModelImporterAnimationType.Human;
		}
	}
	
	// This method is called immediately after importing an FBX.
	void OnPostprocessModel(GameObject go)
	{
		// Assume an animation FBX has an @ in its name,
		// to determine if an fbx is a character or an animation.
		if (assetPath.Contains("@"))
		{
			// For animation FBX's all unnecessary Objects are removed.
			// This is not required but improves clarity when browsing assets.
			
			// Remove SkinnedMeshRenderers and their meshes.
			foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>())
			{
				Object.DestroyImmediate(smr.sharedMesh, true);
				Object.DestroyImmediate(smr.gameObject);
			}

			// Remove the bones.
			foreach (Transform o in go.transform)
				Object.DestroyImmediate(o.gameObject);
		}

		// body不需要骨架
		if (assetPath.Contains("Female_Body") || assetPath.Contains("Male_Body"))
		{
			foreach (Transform o in go.transform)
			{
				if(o.gameObject.name == "Bip01")
					Object.DestroyImmediate(o.gameObject);
			}
		}

	}
}