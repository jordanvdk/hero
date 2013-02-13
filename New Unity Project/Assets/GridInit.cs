using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridInit : MonoBehaviour {
	public Transform CellPrefab;
	public Transform BallPrefab;
	public Vector3 Size;
	public Transform[,] Grid;
	public Transform ball;
	public int numbBalls;
	public List<Transform> team0;
	public List<Transform> team1;
	public int currTeam;
	
	void Update() {
		
		if (noMovesLeft()) {
			currTeam = currTeam+1 % 2;
		}
		
			
	}
	
	bool noMovesLeft() {
		return false;
	}
	
	// Use this for initialization
	void Start () {
		createGrid();
		setAdjacent();
		
		
		team0.Add(addBall(0,1,new Vector2(0,0)));
		team0.Add(addBall(0,2,new Vector2(0,4)));
		team1.Add(addBall(1,1,new Vector2(4,4)));
		team1.Add(addBall(1,2,new Vector2(4,0)));
		
		currTeam=0;
		
		
		
	}
	
	Transform addBall(int team, int jersey, Vector2 position){
		Transform newBall;
		newBall = (Transform)Instantiate(BallPrefab, new Vector3(position.x,position.y,-0.1f), Quaternion.identity);
		newBall.GetComponent<BallScript>().team = team;
		newBall.GetComponent<BallScript>().jersey = jersey;
		if (team==0) {
			newBall.GetComponent<BallScript>().texturePath = "redBall";
		}
		else {
			newBall.GetComponent<BallScript>().texturePath = "blueBall";
		}
		
		return newBall;
	}

	void createGrid(){
		Grid = new Transform[(int)Size.x,(int)Size.y];
		for(int x = 0; x < Size.x ; x++){
			for(int y = 0; y <Size.y ; y++){
				Transform newCell;
				newCell = (Transform)Instantiate(CellPrefab, new Vector3(x,y,0), Quaternion.identity);
				newCell.name = "{"+x+","+y+"}";
				newCell.parent = transform;
				newCell.GetComponent<cellscript>().position = new Vector3(x,y,0);
				//newCell.GetComponent<cellscript>().ball = ball;
				Grid[x,y] = newCell;		
			}
		}
	}
	
	void setAdjacent(){
		for(int x = 0; x < Size.x ; x++){
			for(int y = 0; y <Size.y ; y++){
				Transform cell;
				cell = Grid[x,y];
				cellscript cScript = cell.GetComponent<cellscript>();
				if (x > 0){
					cScript.adjacents.Add(Grid[x-1,y]);
				}
				if (y > 0){
					cScript.adjacents.Add(Grid[x,y-1]);
				}
				if (x < Size.x -1){
					cScript.adjacents.Add(Grid[x+1,y]);
				}
				if (y < Size.y - 1){
					cScript.adjacents.Add(Grid[x,y+1]);
				}
			}
		}
	}
}
