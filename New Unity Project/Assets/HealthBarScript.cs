/*
 #pragma strict
var hp:float;
var maxhp:float;
var healthBarWidth:int;
var myhp:GameObject;
var myHealthBar:GameObject;

function Start () {
healthBarWidth = 50;
myhp = Instantiate(myHealthBar, transform.position, transform.rotation);

}

function Update () {
	myhp.transform.position=Camera.main.WorldToViewportPoint(transform.position);
	myhp.transform.position.x-=.05;
	myhp.transform.position.y+=.06;
	myhp.transform.localScale=Vector3.zero;
	var healthpercent:float=hp/maxhp;
	if(healthpercent<0){healthpercent=0;}
	if(healthpercent>100){healthpercent=100;}
	healthBarWidth=healthpercent*50;
	myhp.guiTexture.pixelInset=Rect(10,10,healthBarWidth,5);
}
*/



using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {
	
	public float hp;
	public float maxhp;
	public int healthBarWidth;
	public Transform myhp;
	public GameObject myHealthBar;
	public Vector3 position;
	
	
	// Use this for initialization
	void Start () {
		healthBarWidth = 50;
		myhp = (Transform)Instantiate(myHealthBar, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		position = myhp.transform.position;
		myhp.transform.position=Camera.main.WorldToViewportPoint(transform.position);
		//myhp.transform.position.x-=.05;
		//myhp.transform.position.y+=.06;
		myhp.transform.position.Set(myhp.transform.position.x - 6.0f, myhp.transform.position.y + 6.0f, myhp.transform.position.z);
		myhp.transform.localScale=Vector3.zero;
		
		float healthpercent = hp/maxhp;
		if(healthpercent<0){healthpercent=0;}
		if(healthpercent>100){healthpercent=100;}
		healthBarWidth=(int) healthpercent*50;
		myhp.guiTexture.pixelInset= new Rect(10,10,healthBarWidth,5);
			//(10,10,healthBarWidth,5);
		
		
	}
}
