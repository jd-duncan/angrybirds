using UnityEngine;
using System.Collections;

//What you are going to do here is working on the line rendered that are supposed to throw 
//the projectile and working on the asteroid itself
public class Projectile_Dragging_Darius : MonoBehaviour {
	//maxstretch determines how far do we want ot stretch it back (limit to how far we stretch)
	//Here it is defaulted at 3 units
	public float maxStretch = 3f;

	//These two lines of blocks are use to 
	//reference the two lines to the catapult to allow the user to manipulate it
	public LineRenderer catapultLineFront; //Remember that the catapult is divider into two objects
	public LineRenderer catapultLineBack;

	//Declare variable "spring" that will define the spring state on/off
	private SpringJoint2D spring;
	private Transform catapult;
	//Make sure when dragging, everything stays in line with the mouse
	private Ray rayToMouse;
	private Ray leftCatapultToProjectile;
	private float maxStretchSqr;
	private float circleRadius;
	private bool clickedOn;
	private bool isReleased;
	private Vector2 prevVelocity;

	void Awake () {
		spring = GetComponent <SpringJoint2D> ();
		catapult = spring.connectedBody.transform;
	}
	void Start () {
		//Call this function at start the function to initialize the rubber band 
		LineRendererSetup ();
		//
		rayToMouse = new Ray(catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
		//Easier to define limit and more precise if we take sqr
		maxStretchSqr = maxStretch * maxStretch;
		CircleCollider2D circle = collider2D as CircleCollider2D;
		circleRadius = circle.radius;
	}
	

	void Update () {
		if (clickedOn)
			Dragging ();
		//Defining what happen if our asteroid go beyond the catapult: no more kinetic, no more spring
		if (spring != null) {
		 if(!rigidbody2D.isKinematic && prevVelocity.sqrMagnitude > rigidbody2D.velocity.sqrMagnitude){
				Destroy (spring);
				rigidbody2D.velocity = prevVelocity;
			}
			if(!clickedOn)
				prevVelocity = rigidbody2D.velocity;
			LineRenderUpdate();
		} else {
			catapultLineFront.enabled = false;
			catapultLineBack.enabled = false;
		
		}
	
	}
	//Here we declare the function to set up rubber band at its start (here the 0's are set to the catapult
	void LineRendererSetup (){
		catapultLineFront.SetPosition (0, catapultLineFront.transform.position);
		catapultLineBack.SetPosition (0, catapultLineBack.transform.position);

		//Set the order in layer 
		catapultLineFront.sortingLayerName = "ForeGround";
		catapultLineBack.sortingLayerName = "ForeGround";

		//Order in layer
		catapultLineFront.sortingOrder = 3;
		catapultLineBack.sortingOrder = 1;

		}

	//What happens when we hold and release the projectile ?
	//Here: Determine what happen when we click = the spring joint doesn't happen
	void OnMouseDown () {
		spring.enabled = false;
		clickedOn = true;
	}
	//Here: Determine what happen when we release = the spring joint is activated
	void OnMouseUp () {
		spring.enabled = true;
		//Once it's released it works under the law of physics
		rigidbody2D.isKinematic = false;
		clickedOn = false;
	}
	//Create function for dragging
	void Dragging (){
	//When dragging the asteroid determines the behavior of the asteroid
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//Defining vector between asteroid and catapult
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
		//Determine limit for dragging, if go beyond 3, stays = to limit
		if (catapultToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (maxStretch);
		}
		//ignore position of z because 2D game
		mouseWorldPoint.z = 0f;
		//Position of the asteroid become position of mouse
		transform.position = mouseWorldPoint;
	}
	//Each frame we need to update our line render
	void LineRenderUpdate (){
		Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
		catapultLineFront.SetPosition (1, holdPoint);
		catapultLineBack.SetPosition (1, holdPoint);
	}
}
