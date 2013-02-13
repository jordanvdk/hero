using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// This is a custom asset processor. Do NOT modify this class unless you know what you're doing.
/// Even compilation errors in this script could lead to terrible things happening to your assets.
/// </summary>
public class SpritePostProcessor : AssetPostprocessor
{
	/// <summary>
	/// Gets called before textures are imported.
	/// </summary>
	void OnPreprocessTexture ()
	{
		// Only treat textures in the Sprite Textures folder.
		if (assetPath.StartsWith("Assets/Sprite Textures/"))
		{
			TextureImporter textureImporter = (TextureImporter)assetImporter;

			// 2D textures usually don't wrap around things, so clamp them.
			textureImporter.wrapMode = TextureWrapMode.Clamp;
			// 2D games need awesome textures, so use truecolor.
			textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;

			// Set to point filter mode if we're expecting pixel art.
			#if SMOOTH_TEXTURES
			textureImporter.filterMode = FilterMode.Bilinear;
			#else
			textureImporter.filterMode = FilterMode.Point;
			#endif
		}
	}

	/// <summary>
	/// Gets called after a texture has been imported.
	/// </summary>
	/// <param name="texture">The texture that was imported.</param>
	void OnPostprocessTexture (Texture2D texture)
	{
		// Only treat textures in the Sprite Textures folder.
		if (assetPath.StartsWith("Assets/Sprite Textures/"))
		{
			List<string> allAssetPaths = new List<string>(AssetDatabase.GetAllAssetPaths());

			string[] pathParts = assetPath.Replace("Assets/Sprite Textures/",
				"Assets/Sprite Materials/").Split('/');

			string growingPath = "Assets";

			// We're going to automatically generate a material for the texture in an
			// identical folder structure, so ensure that all parent directories exist.
			for (int i = 1; i < pathParts.Length - 1; i++)
			{
				string newGrowingPath = growingPath + "/" + pathParts[i];

				if (!allAssetPaths.Contains(newGrowingPath))
				{
					AssetDatabase.CreateFolder(growingPath, pathParts[i]);
					allAssetPaths.Add(newGrowingPath);
				}

				growingPath = newGrowingPath;
			}

			string materialPath = growingPath + "/" +
				Path.GetFileNameWithoutExtension(assetPath) + ".mat";

			materialPath = materialPath.Replace(
				"Assets/Sprite Textures/", "Assets/Sprite Materials/");

			// Only create a new material if it doesn't exist. Otherwise, reimporting will
			// mess up all of our material references in teh scene.
			if (!allAssetPaths.Contains(materialPath))
			{
				// Use a default shader that is appropriate for 2D games.
				Material material = new Material(Shader.Find("Unlit/Transparent"));

				// Create the new material asset.
				AssetDatabase.CreateAsset(material, materialPath);
			}
		}
	}

	/// <summary>
	/// Called after all assets have been imported and finalized.
	/// </summary>
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets,
		string[] movedAssets, string[] movedFromPath)
	{
		// We use this method because at this point, all the imported textures have
		// settled into their final paths, and it is now safe to make references to them.

		foreach (string assetPath in importedAssets)
		{
			// Only treat textures in the Sprite Textures folder.
			if (assetPath.StartsWith("Assets/Sprite Textures/"))
			{
				string extension = Path.GetExtension(assetPath);
				int extensionLocation = assetPath.IndexOf(extension);

				string materialPath = assetPath.Substring(0, extensionLocation).
					Replace("Assets/Sprite Textures/", "Assets/Sprite Materials/") + ".mat";

				// Assign the texture to the corresponding generated material.
				((Material)AssetDatabase.LoadAssetAtPath(materialPath, typeof(Material))).mainTexture =
					(Texture2D)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D));
			}
		}
	}
}