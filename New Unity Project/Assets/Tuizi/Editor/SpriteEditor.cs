using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom inspector for the Sprite component.
/// </summary>
[CustomEditor(typeof(Sprite))]
public class SpriteEditor : Editor
{
	/// <summary>
	/// Gets called when the inspector GUI updates.
	/// </summary>
	public override void OnInspectorGUI ()
	{
		Sprite sprite = (Sprite)target;

		GUILayout.Space(10);

		// Basic properties for the sprite.

		sprite.AutoPlay = GUILayout.Toggle(
			sprite.AutoPlay, "Auto Play");

		sprite.FixedAspect = GUILayout.Toggle(
			sprite.FixedAspect, "Fixed Aspect");

		sprite.RestartSameClip = GUILayout.Toggle(
			sprite.RestartSameClip, "Restart Same Clip");

		sprite.PlayFromSecondFrame = GUILayout.Toggle(
			sprite.PlayFromSecondFrame, "Play From Second Frame");

		// Begin section for clips.
		GUILayout.Label("Clips (" + sprite.Clips.Length + ")");

		GUILayout.BeginHorizontal();

		GUILayout.Space(15);

		GUILayout.BeginVertical();

		GUILayout.Space(8);

		SpriteClip[] clips = sprite.Clips;

		for (int i = 0; i < clips.Length; i++)
		{
			SpriteClip clip = clips[i];

			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();

			// Field for assigning the material.
			Material newMaterial = (Material)EditorGUILayout.ObjectField(
				clip.Material, typeof(Material), false);

			if (newMaterial != clip.Material)
			{
				Undo.RegisterUndo(sprite, "Set Sprite Clip Material");
				clip.Material = newMaterial;

				// Set the clip name to be the material name.
				if (clip.Material != null)
				{
					clip.Name = clip.Material.name;

					if (clip.Material.mainTexture != null)
					{
						clip.FrameWidth = clip.Material.mainTexture.width;
						clip.FrameHeight = clip.Material.mainTexture.height;
					}
					else
					{
						clip.FrameWidth = 1;
						clip.FrameHeight = 1;
					}
				}
				else clip.Name = null;
			}

			// Can't move first clip up.
			GUI.enabled = i != 0;

			// Button to move this clip up in the list.
			if (GUILayout.Button("Up", GUILayout.Width(28)))
			{
				Undo.RegisterUndo(sprite, "Rearrange Sprite Clips");
				sprite.RemoveClip(clip);
				sprite.InsertClip(i - 1, clip);
			}

			// Can't move last clip down.
			GUI.enabled = i != clips.Length - 1;

			// Button to move this clip down in the list.
			if (GUILayout.Button("Dn", GUILayout.Width(28)))
			{
				Undo.RegisterUndo(sprite, "Rearrange Sprite Clips");
				sprite.RemoveClip(clip);
				sprite.InsertClip(i + 1, clip);
			}

			GUI.enabled = true;
			
			// Button to remove this clip.
			if (GUILayout.Button("X", GUILayout.Width(20)))
			{
				Undo.RegisterUndo(sprite, "Remove Sprite Clip");
				sprite.RemoveClip(clip);
			}

			GUI.enabled = true;

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();

			// Field for frame count. Lower limit of 1, to prevent doodoos.
			GUILayout.Label("Frames", GUILayout.Width(47));
			int newFrames = Mathf.Max(1, EditorGUILayout.IntField(clip.Frames, GUILayout.Width(40)));

			if (newFrames != clip.Frames)
			{
				Undo.RegisterUndo(sprite, "Set Sprite Clip Frames");
				clip.Frames = newFrames;
			}

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			// Field for speed, in frames per second.
			GUILayout.Label("Speed", GUILayout.Width(47));
			float newSpeed = EditorGUILayout.FloatField(clip.Speed, GUILayout.MaxWidth(40));

			if (newSpeed != clip.Speed)
			{
				Undo.RegisterUndo(sprite, "Set Sprite Clip Speed");
				clip.Speed = newSpeed;
			}

			GUILayout.EndHorizontal();

			GUILayout.EndVertical();

			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();

			// Field for frame width. Lower limit of 1, to prevent doodoos.
			GUILayout.Label("Frame Width", GUILayout.Width(82));
			int newFrameWidth = Mathf.Max(1, EditorGUILayout.IntField(clip.FrameWidth, GUILayout.Width(40)));

			if (newFrameWidth != clip.FrameWidth)
			{
				Undo.RegisterUndo(sprite, "Set Sprite Clip Frame Width");
				clip.FrameWidth = newFrameWidth;

				clip.AdjustTextureScale();
			}

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			// Field for frame height. Lower limit of 1, to prevent doodoos.
			GUILayout.Label("Frame Height", GUILayout.Width(82));
			int newFrameHeight = Mathf.Max(1, EditorGUILayout.IntField(clip.FrameHeight, GUILayout.Width(40)));

			if (newFrameWidth != clip.FrameHeight)
			{
				Undo.RegisterUndo(sprite, "Set Sprite Clip Frame Height");
				clip.FrameHeight = newFrameHeight;

				clip.AdjustTextureScale();
			}

			GUILayout.EndHorizontal();

			GUILayout.EndVertical();

			GUILayout.EndHorizontal();

			GUILayout.Space(5);

			GUILayout.EndVertical();
		}

		GUILayout.BeginHorizontal();

		// Button to add a new clip.
		if (GUILayout.Button("New", GUILayout.MaxWidth(50)))
		{
			Undo.RegisterUndo(sprite, "Add Sprite Clip");
			sprite.AddClip(new SpriteClip());
		}

		GUI.enabled = clips.Length != 0;
		
		// Button to remove all clips.
		if (GUILayout.Button("Clear", GUILayout.MaxWidth(55)))
		{
			// Confirm removal of all clips.
			if (EditorUtility.DisplayDialog("Clear Sprite Clips",
				"Are you sure you would like to clear all sprite clips from this sprite?",
				"Yes", "No"))
			{
				Undo.RegisterUndo(sprite, "Clear Sprite Clips");
				sprite.ClearClips();
			}
		}

		GUI.enabled = true;

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		if (GUI.changed)
		{
			SpriteClip firstClip = sprite.GetClip(0);

			// If this is the first clip, assign it to the sprite's renderer right away
			// so we can preview the sprite's apperance in edit mode.
			if (firstClip.Material != null && firstClip.Material != sprite.renderer.sharedMaterial)
				sprite.renderer.sharedMaterial = firstClip.Material;

			sprite.AdjustAspect();

			// If no material available, use default.
			if (sprite.Clips.Length == 0)
				sprite.renderer.sharedMaterial =
					(Material)AssetDatabase.LoadAssetAtPath(
					"Assets/Tuizi/Materials/default_sprite_material.mat", typeof(Material));

			EditorUtility.SetDirty(target);
		}
	}
}