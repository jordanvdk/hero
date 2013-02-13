using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	public Vector3 currPos;
	public Vector3 targetPos;
	public float speed;
	public string description;
	public string targetDescription;
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(transform.position);
		//Debug.Log(Screen.width);
		//Debug.Log(Screen.height);
		if (Input.mousePosition.x > 0.90*Screen.width){
			transform.Translate(Vector2.right * Time.deltaTime * speed);
		}
		if (Input.mousePosition.x < 0.10*Screen.width){
			transform.Translate(Vector2.right * -Time.deltaTime * speed);
		}
		if (Input.mousePosition.y > 0.90*Screen.height){
			transform.Translate(Vector2.up * Time.deltaTime * speed);
		}
		if (Input.mousePosition.y < 0.10*Screen.height){
			transform.Translate(Vector2.up * -Time.deltaTime * speed);
		}
	}
	
	void OnGUI () {
		float boxOriginX = 1F;
		float boxOriginY = Screen.height * 0.8F;
		float boxWidth = Screen.width * 0.2F;
		float boxHeight = Screen.height *0.15F;
		
		
		//description += " hi";
		
		GUI.Box(new Rect(boxOriginX, boxOriginY, boxWidth, boxHeight), "Skills");
		
		
		if (GUI.Button(new Rect(boxOriginX * 5F, boxOriginY + 35F, boxWidth * 0.9F, boxHeight * 0.2F), "Fire Ball")){
			fireball();
		}
		if (GUI.Button(new Rect(boxOriginX * 5F, boxOriginY + 55F, boxWidth * 0.9F, boxHeight * 0.2F), "lightning bolt")){
			lightningbolt();
		}
		
		GUI.Box(new Rect(boxOriginX + boxWidth + 5, boxOriginY, boxWidth, boxHeight), description);
		GUI.Box(new Rect(boxOriginX + 2* (boxWidth + 5), boxOriginY, boxWidth, boxHeight), targetDescription);
		
		
	}
	void fireball() {
	}
	void lightningbolt() {
	}
}
