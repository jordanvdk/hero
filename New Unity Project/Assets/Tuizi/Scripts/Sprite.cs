using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A simple sprite component. Supports multiple animations and auto-scaling in editor.
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Sprite : MonoBehaviour
{
	/// <summary>
	/// The clip that the sprite is currently set to.
	/// </summary>
	public SpriteClip ActiveClip
	{
		get { return this.activeClip; }
	}

	/// <summary>
	/// The number of clips available for this sprite.
	/// </summary>
	public int NumClips
	{
		get { return this.clips.Count; }
	}

	/// <summary>
	/// All available clips for this sprite.
	/// </summary>
	public SpriteClip[] Clips
	{
		get { return this.clips.ToArray(); }
	}

	/// <summary>
	/// Whether or not to fix the aspect ratio according to the texture while in the editor.
	/// This of course takes the frame count into account.
	/// </summary>
	public bool FixedAspect = true;
	/// <summary>
	/// Whether or not the first clip in the clip list is played automatically on start.
	/// </summary>
	public bool AutoPlay = true;
	/// <summary>
	/// Whether or not to restart the active clip when it is the same clip
	/// as the clip that is being requested through Play(...).
	/// </summary>
	public bool RestartSameClip = false;
	/// <summary>
	/// Whether or not to start clips from their second frame. Useful for animations that can
	/// start and stop frequently before reaching the second frame, and the first frame
	/// looks just like the preceding clip.
	/// </summary>
	public bool PlayFromSecondFrame = false;
	/// <summary>
	/// The speed at which to play the active clip. Although each clip has its own default
	/// speed which is assigned to this field when Play(...) is called, the playback speed
	/// can be adjusted in real-time. For example, set to 0 to pause, or set to .5f for
	/// slow-motion. Speed is specified in frames-per-second.
	/// </summary>
	public float Speed = 0f;

	[SerializeField]
	List<SpriteClip> clips = new List<SpriteClip>();

	int frame;
	float frameBuffer;
	SpriteClip activeClip;
	Vector3 prevScale;

	/// <summary>
	/// Play the clip with this name.
	/// </summary>
	/// <param name="clipName">The clip name. Same as the name of the clip material.</param>
	public void PlayClip (string clipName)
	{
		SpriteClip clip = GetClip(clipName);

		if (clip != null)
		{
			if ((clip != activeClip) || RestartSameClip || Speed == 0f)
			{
				activeClip = clip;

				// Assigning to renderer.material from clip.Material instantiates the material.
				// We save the instance it back to clip.Material so we don't keep instantiating.
				renderer.material = clip.Material;
				clip.Material = renderer.material;

				frame = (PlayFromSecondFrame && clip.Frames > 1) ? 1 : 0;
				frameBuffer = 0f;
			}

			Speed = clip.Speed;
		}
	}

	/// <summary>
	/// Stop a clip. Rewinds to first frame. To pause instead, just set Speed to 0.
	/// </summary>
	public void StopClip ()
	{
		Speed = 0f;
		frame = 0;
		frameBuffer = 0f;
	}

	/// <summary>
	/// Rewind a clip without stopping it necessarily.
	/// </summary>
	public void RewindClip ()
	{
		frame = 0;
		frameBuffer = 0f;
	}

	/// <summary>
	/// Get the clip with this name.
	/// </summary>
	/// <param name="clipName">The name of the clip to retrieve.</param>
	/// <returns>The requested clip, or null if not found.</returns>
	public SpriteClip GetClip (string clipName)
	{
		foreach (SpriteClip clip in this.clips)
		{
			if (clip.Name == clipName)
				return clip;
		}

		return null;
	}

	/// <summary>
	/// Get the clip at this index.
	/// </summary>
	/// <param name="index">The index of the clip to retrieve.</param>
	/// <returns>The requested clip, or null if not found.</returns>
	public SpriteClip GetClip (int index)
	{
		if (index < 0 || index >= clips.Count)
			return null;

		return clips[index];
	}

	/// <summary>
	/// Add a clip to the list. No effect if clip is null.
	/// </summary>
	/// <param name="clip">The clip to add.</param>
	public void AddClip (SpriteClip clip)
	{
		if (clip != null)
			this.clips.Add(clip);
	}

	/// <summary>
	/// Insert a clip to the list at a certain point. No effect if clip is null.
	/// </summary>
	/// <param name="index">The index at which to insert the clip.</param>
	/// <param name="clip">The clip to insert.</param>
	public void InsertClip (int index, SpriteClip clip)
	{
		if (index <= this.clips.Count)
			this.clips.Insert(index, clip);
	}

	/// <summary>
	/// Remove a clip from the list. No effect if clip is null.
	/// </summary>
	/// <param name="clip">The clip to remove.</param>
	public void RemoveClip (SpriteClip clip)
	{
		this.clips.Remove(clip);
	}

	/// <summary>
	/// Remove a clip from a point in the list. No effect if index is out of bounds.
	/// </summary>
	/// <param name="index">The index of the clip to remove.</param>
	public void RemoveClipAt (int index)
	{
		SpriteClip clip = GetClip(index);

		if (clip != null)
			clips.Remove(clip);
	}

	/// <summary>
	/// Empty the clip list.
	/// </summary>
	public void ClearClips ()
	{
		this.clips.Clear();
	}

	/// <summary>
	/// Adjust the aspect to match the first sprite clip.
	/// </summary>
	public void AdjustAspect ()
	{
		if (FixedAspect)
		{
			// Calculate the desired aspect from the first sprite clip.
			float desiredAspect = (float)this.clips[0].FrameWidth / this.clips[0].FrameHeight;

			// This is the aspect-corrected size we would get if we use the current width.
			Vector3 widthBasedNewSize = new Vector3(transform.localScale.x,
				transform.localScale.x / desiredAspect, transform.localScale.z);

			// This is the aspect-corrected size we would get if we use the current height.
			Vector3 heightBasedNewSize = new Vector3(transform.localScale.y * desiredAspect,
				transform.localScale.y, transform.localScale.z);

			// This is the bigger size of the two.
			Vector3 biggerSize = widthBasedNewSize.sqrMagnitude > heightBasedNewSize.sqrMagnitude ?
				widthBasedNewSize : heightBasedNewSize;

			// This is the smaller size of the two.
			Vector3 smallerSize = biggerSize == widthBasedNewSize ?
				heightBasedNewSize : widthBasedNewSize;

			// If the user's intent was to scale up, use the bigger size.
			if (transform.localScale.sqrMagnitude > prevScale.sqrMagnitude)
			{
				prevScale = transform.localScale;
				transform.localScale = biggerSize;
			}
			// If the user's intent was to scale down, use the smaller size.
			else if (transform.localScale.sqrMagnitude < prevScale.sqrMagnitude)
			{
				prevScale = transform.localScale;
				transform.localScale = smallerSize;
			}
			else // Otherwise, use the height-based size.
			{
				prevScale = transform.localEulerAngles;
				transform.localScale = heightBasedNewSize;
			}
		}
	}

	/// <summary>
	/// Called when the game starts, or when the editor scene is loaded,
	/// since we are using [ExecuteInEditMode]
	/// </summary>
	void Start ()
	{
		// If this is the game starting...
		if (Application.isPlaying)
		{
			for (int i = 0; i < this.clips.Count; i++)
			{
				// Remove invalid clips from the clip list.
				if (this.clips[i].Material == null)
				{
					this.clips.Remove(clips[i]);
					i--;
					continue;
				}

				if (i == 0)
				{
					// At game start, the material asset that is referenced in the renderer
					// (the material of the first clip) is instantiated. We would like to
					// keep using this instance, since we have no more use for the orignal
					// material asset, so we store it as a reference in the clip. We want
					// the material instance and not the asset since different instances
					// can have different states. For example, one enemy might be on a
					// different frame of an animation from another, even though they were
					// created from the same material asset.
					this.clips[i].Material = this.renderer.material;

					if (AutoPlay)
						PlayClip(this.clips[i].Name);
				}
			}
		}
	}

	/// <summary>
	/// Called when the game loops, or when something in the editor scene has changed,
	/// since we are using [ExecuteInEditMode]
	/// </summary>
	void Update ()
	{
		// If this is the game looping...
		if (Application.isPlaying)
		{
			if (activeClip != null)
			{
				if (Speed != 0)
				{
					// Increment the frame buffer by the playback speed.
					frameBuffer += Time.deltaTime * Speed;

					// When frame buffer reaches 1, it's time to increment the frame.
					while (frameBuffer >= 1)
					{
						frameBuffer -= 1f;
						frame = (frame + 1) % activeClip.Frames;
					}
				}

				// Set the material's texture offset to display the proper frame.
				if (renderer.material.mainTexture != null)
					renderer.material.mainTextureOffset = new Vector2(
						(float)(frame * activeClip.FrameWidth) /
						activeClip.Material.mainTexture.width, 0);
			}
		}
		else // If this is the editor scene changing...
		{
			if (this.clips.Count != 0)
			{
				// If the scale of our sprite has changed and we want to maintain proper aspect
				// and we have a material with a texture with which to calculate the proper aspect...
				if (FixedAspect && prevScale != transform.localScale)
					AdjustAspect();
			}
		}
	}
}
