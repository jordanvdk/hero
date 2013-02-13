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