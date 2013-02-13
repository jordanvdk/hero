using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cellscript : MonoBehaviour {

	// Use this for initialization
	//public Transform ball;
	public List<Transform> adjacents;
	public Vector3 position;

	//public bool xmoveflag = false;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*
		ball = GameObject.Find("Ball");
		if (xmoveflag = true){
			ball.transform.Translate((position.x - ball.transform.position.x) * Time.deltaTime,0,0);	
			if (position.x == ball.transform.position.x){
				xmoveflag = false;
			}
		}
		*/
	}
	void OnMouseDown () {
		//Vector2 squareCoordinates = new Vector2(transform.position.x,transform.position.y);
		//Vector2 ballCoordinates	= new Vector2 (ball.transform.position.x, ball.transform.position.y);
		//while (!squareCoordinates.Equals(ballCoordinates)) {
		//Debug.Log(squareCoordinates.x);
		//Debug.Log(squareCoordinates.y);
		//Debug.Log(ball.transform.position.z);
		
		//BallScript ballObject = ball.GetComponent<BallScript>();
		//if (position != ballObject.targetPos){	
			//ball.transform.Translate((squareCoordinates.x - ball.transform.position.x) * Time.deltaTime,0,0);
			//xmoveflag = true;	
			//ballObject.targetPos.x = position.x;
			//ballObject.targetPos.y = position.y;
			//ballObject.moveFlag = true;
			//ballObject.gridx = (int)Math.Ceiling(postion.x);
			//ballObject.gridy = (int)Math.Ceiling(postion.y);
			
		Debug.Log("MouseDown on Cell");
			GameObject Grid = GameObject.Find("Grid");
			GridInit grid = Grid.GetComponent<GridInit>();
			foreach(Transform ball in grid.team0){
			BallScript ballObject = ball.GetComponent<BallScript>();
				if (ballObject.selected == true){
					if (position != ballObject.targetPos){
						ballObject.targetPos.x = position.x;
						ballObject.targetPos.y = position.y;
						ballObject.moveFlag = true;
					}
				}
				
			}
			
			Transform[,] cells = GameObject.Find("Grid").GetComponent<GridInit>().Grid;
			foreach (Transform cell in cells) {
				cellscript cellObject = cell.GetComponent<cellscript>();
				//cellObject.renderer.material.mainTexture = Resources.Load ("cell") as Texture;
			}
		
		
		
		//ball.transform.position = new Vector3(squareCoordinates.x, squareCoordinates.y, ball.transform.position.z);
		//}
	}
}
 