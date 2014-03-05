using UnityEngine;
using UnityEditor;

public partial class TexturePreprocess : AssetPostprocessor
{
	void OnPreprocessTexture()
	{
		TextureImporter importer = assetImporter as TextureImporter;
		importer.maxTextureSize = 4096;
	}
}