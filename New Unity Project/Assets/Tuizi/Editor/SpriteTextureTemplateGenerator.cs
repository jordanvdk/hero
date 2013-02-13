using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Creates sprite texture templates.
/// </summary>
public class SpriteTextureTemplateGenerator : ScriptableWizard
{
	public string textureName = "sprite_texture_template";
	public int frameWidth = 32;
	public int frameHeight = 32;
	public int frames = 4;
	public Color backColor = Color.white;
	public Color gridColor = Color.gray;

	void OnWizardUpdate ()
	{
		helpString = "Sprite texture template will be created at:\n" +
			AssetDatabase.GetAssetPath(Selection.activeObject) + "/" + textureName + "\n";

		string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
		bool isFolder = assetPath.Length != 0 && !assetPath.Contains(".");

		if (frameWidth <= 0 || frameHeight <= 0)
			errorString = "Frame dimensions must be non-zero.";
		else if (frames <= 0)
			errorString = "Sprite must have at least one frame.";
		else if (!isFolder)
			errorString = "Target folder must be selected.";
		else errorString = "";

		isValid = errorString == "";
	}

	void OnWizardCreate ()
	{
		int potWidth = 1;

		while (potWidth < frameWidth * frames)
			potWidth *= 2;

		int potHeight = 1;

		while (potHeight < frameHeight)
			potHeight *= 2;

		Texture2D texture = new Texture2D(potWidth, potHeight);

		for (int i = 0; i < frameWidth * frames; i++)
		{
			for (int j = 0; j < frameHeight; j++)
			{
				if (i % frameWidth == 0 || i % frameWidth == frameWidth - 1 ||
					j == 0 || j == frameHeight - 1)
					texture.SetPixel(i, j, gridColor);
				else texture.SetPixel(i, j, backColor);
			}
		}

		File.WriteAllBytes(AssetDatabase.GetAssetPath(Selection.activeObject) +
			"/" + textureName + ".png", texture.EncodeToPNG());

		AssetDatabase.Refresh();
	}
}
