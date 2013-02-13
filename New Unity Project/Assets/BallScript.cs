using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BallScript : MonoBehaviour {
	
	public Vector3 currPos;
	public Vector3 targetPos;
	public bool moveFlag = false;
	public float offset;
	public float speed; 
	//public Transform grid;
	public int gridx;
	public int gridy;
	public int movecount;
	public string name;
	public bool selected = false;
	public int health = 100;
	public int attack = 20;
	public Vector2 currCell;
	public int team;
	public int jersey;
	public string texturePath;
	public int range;
	public Transform Text;
	
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			Destroy(this.gameObject);
		}
		if (moveFlag == true){
			currPos = transform.position;
			if (!(currPos.x >= targetPos.x - offset && currPos.x <= targetPos.x + offset)){
				float deltaX = targetPos.x - currPos.x;
				if (deltaX > 0){
					transform.Translate(Vector2.right * Time.deltaTime * speed);		
				}
				else{
					transform.Translate(Vector2.right * -Time.deltaTime * speed);
				}
			}
			else if (!(currPos.y >= targetPos.y - offset && currPos.y <= targetPos.y + offset)){
				float deltaY = targetPos.y - currPos.y;
				if (deltaY > 0){
					transform.Translate(Vector2.up * Time.deltaTime * speed);		
				}
				else{
					transform.Translate(Vector2.up * -Time.deltaTime * speed);
				}
			}
			else{
				moveFlag = false;	
				currCell.x = Mathf.RoundToInt(currPos.x);
				currCell.y = Mathf.RoundToInt(currPos.y);
				transform.position = new Vector3(currCell.x, currCell.y, transform.position.z);
				
			}	
		}
		
		Transform[,] cells = GameObject.Find("Grid").GetComponent<GridInit>().Grid;
		
		if (selected){
			this.renderer.material.mainTexture = Resources.Load("ballSelected") as Texture;
			foreach (Transform cell in cells) {
				cellscript cellObject = cell.GetComponent<cellscript>();
				if (Mathf.Abs(cellObject.position.x - transform.position.x) + Mathf.Abs(cellObject.position.y - transform.position.y) <= range) {
					cellObject.renderer.material.mainTexture = Resources.Load ("terrain") as Texture;
				}
				else {
					cellObject.renderer.material.mainTexture = Resources.Load ("cell") as Texture;
				}
			}
		}
		else{
			this.renderer.material.mainTexture = Resources.Load(texturePath) as Texture;
		}
	}
	void OnMouseOver() {
		CameraScript cameraObject = Camera.main.GetComponent<CameraScript>();
		cameraObject.targetDescription = "Status" + System.Environment.NewLine +
									"Team = " + team + System.Environment.NewLine +
									"Health = " + health + System.Environment.NewLine +
									"Attack Damage = " + attack + System.Environment.NewLine;
	}
	void OnMouseExit() {
		CameraScript cameraObject = Camera.main.GetComponent<CameraScript>();
		cameraObject.targetDescription = "";
	}
	void OnMouseDown () {
		currPos = transform.position;
		/*
		
		Camera.main.transform.position = new Vector3(currPos.x, currPos.y, Camera.main.transform.position.z);
		*/
		
		GameObject Grid = GameObject.Find("Grid");
		GridInit grid = Grid.GetComponent<GridInit>();
		
		if (grid.currTeam == this.team) {
			//select a unit
			CameraScript cameraObject = Camera.main.GetComponent<CameraScript>();
			cameraObject.description = "Status" + System.Environment.NewLine +
										"Team = " + team + System.Environment.NewLine +
										"Health = " + health + System.Environment.NewLine +
										"Attack Damage = " + attack + System.Environment.NewLine;
			
			foreach(Transform ball in grid.team0){
				BallScript ballObject = ball.GetComponent<BallScript>();
				ballObject.selected = false;
			}
			this.selected = true;
		}
		else {
			foreach(Transform ball in grid.team0){
				BallScript ballObject = ball.GetComponent<BallScript>();
				if (ballObject.selected == true) {
					this.health = this.health - ballObject.attack;
					
					GameObject announcementObject = GameObject.Find("Smack");
					TextMesh announcement = GameObject.Find("Smack").GetComponent<TextMesh>();
					announcementObject.transform.position = transform.position;
					announcement.text= "Smack!";

					//yield WaitForSeconds (5);
					
					announcement.text= "";
					
					//new WaitForSeconds(1);
					//GameObject.Find("Smack").gameObject.guiText.text="";
					break;
				}
			}
			
		}
	}
}
