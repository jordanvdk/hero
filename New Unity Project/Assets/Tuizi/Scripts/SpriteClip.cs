using UnityEngine;
using System;

/// <summary>
/// A sprite clip.
/// </summary>
[Serializable]
public class SpriteClip
{
	/// <summary>
	/// Name of the clip. Set to be the name of the material.
	/// </summary>
	/// <remarks>Note that this should not ALWAYS reflect the name of the material
	/// since during run-time, the material is instantiated with a different name.</remarks>
	public string Name = null;
	/// <summary>
	/// The material to use for this clip.
	/// </summary>
	public Material Material = null;
	/// <summary>
	/// The number of frames in this clip. Do NOT set this < 0.
	/// </summary>
	public int Frames = 1;
	/// <summary>
	/// The width of each frame in this clip in pixels. Do NOT set this < 0.
	/// </summary>
	public int FrameWidth = 1;
	/// <summary>
	/// The height of each frame in this clip in pixels. Do NOT set this < 0.
	/// </summary>
	public int FrameHeight = 1;
	/// <summary>
	/// The default playback speed of this clip.
	/// </summary>
	public float Speed = 0;

	/// <summary>
	/// Adjust the material's texture scale based on frame dimensions.
	/// </summary>
	public void AdjustTextureScale ()
	{
		if (Material != null && Material.mainTexture != null)
		{
			Material.mainTextureScale = new Vector2(
				(float)FrameWidth / Material.mainTexture.width,
				(float)FrameHeight / Material.mainTexture.height);
		}
	}
}