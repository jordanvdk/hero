using UnityEngine;
using UnityEditor;

/// <summary>
/// Contains the different menu items available for Tuizi.
/// </summary>
public class TuiziMenu : MonoBehaviour
{
	/// <summary>
	/// Creates a sprite in the scene view.
	/// </summary>
	[MenuItem("GameObject/Create Other/Sprite")]
	static void CreateSprite ()
	{
		GameObject go = new GameObject("Sprite");

		Undo.RegisterCreatedObjectUndo(go, "Create Sprite");

		// Adding a sprite component first will automatically add the other required components.
		Sprite sprite = go.AddComponent<Sprite>();

		// Use the awesome unit square mesh.
		go.GetComponent<MeshFilter>().sharedMesh =
			(Mesh)AssetDatabase.LoadAssetAtPath("Assets/Tuizi/Meshes/unit_square.asset", typeof(Mesh));

		SpriteClip clip = new SpriteClip();
		sprite.AddClip(clip);

		// Set the material for the sprite to a default, so it's not the nasty magenta.
		clip.Material = go.renderer.sharedMaterial =
			(Material)AssetDatabase.LoadAssetAtPath(
			"Assets/Tuizi/Materials/default_sprite_material.mat", typeof(Material));

		// Highlight the newly created sprite.
		Selection.activeGameObject = go;
	}

	/// <summary>
	/// Add a 2D Rigidbody component.
	/// </summary>
	[MenuItem("Component/Physics/Tuizi 2D Rigidbody")]
	static void Add2DRigidbody ()
	{
		// Just a regular rigidbody with some typical 2D constraints.
		Selection.activeGameObject.AddComponent<Rigidbody>();
		Selection.activeGameObject.rigidbody.constraints =
			RigidbodyConstraints.FreezePositionZ |
			RigidbodyConstraints.FreezeRotationX |
			RigidbodyConstraints.FreezeRotationY;
	}

	/// <summary>
	/// Whether or not a 2D Rigidbody component can be added.
	/// </summary>
	/// <returns>Whether or not a 2D Rigidbody component can be added.</returns>
	[MenuItem("Component/Physics/2D Rigidbody", true)]
	static bool CanAdd2DRigidbody ()
	{
		// We can add one if a GameObject is selected, and it doesn't have a rigidbody.
		return Selection.activeGameObject != null &&
			Selection.activeGameObject.GetComponent<Rigidbody>() == null;
	}

	/// <summary>
	/// Create a sprite texture template.
	/// </summary>
	[MenuItem("Assets/Create Sprite Texture Template")]
	static void CreateSpriteTextureTemplate ()
	{
		string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

		if (assetPath.Length != 0 && !assetPath.Contains("."))
		{
			ScriptableWizard.DisplayWizard<SpriteTextureTemplateGenerator>(
				"Create Sprite Texture Template", "Create");
		}
		else EditorUtility.DisplayDialog("Tuizi Error", "Please select a texture folder first!", "OK");
	}
}
