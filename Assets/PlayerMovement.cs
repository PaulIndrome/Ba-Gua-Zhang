using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public TargetDebug currentTarget;

	public float radius, degreeSpeed, smoothTime;
	float horizontal, vertical;
	float targetAngle, currentAngle, angleDifference, nextAngle, currentCircleVelocity;
	Quaternion rotationDifference;
	Vector3 targetPositionOnCircle, nextPos, yRaise;
	CharacterController characterController;
	Coroutine lerpToAngleCor;

	LineRenderer lineRenderer;

	public Animator playerAnimator;

	// Use this for initialization
	void Start () {

		if(currentTarget == null){
			GameObject newTarget = GameObject.CreatePrimitive(PrimitiveType.Cube);
			currentTarget = newTarget.AddComponent<TargetDebug>();
			currentTarget.RepositionCurrentTargetTo(Vector3.zero);
		}

		currentTarget.posChangeEvent += TargetPositionUpdate;

		yRaise = new Vector3(0,1,0);

		characterController = GetComponent<CharacterController>();
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetPosition(0, currentTarget.transform.position + yRaise);
		currentAngle = Vector3.SignedAngle(Vector3.forward, transform.position, Vector3.up);
		targetAngle = currentAngle;
	}
	
	// Update is called once per frame
	void Update () {
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		if(new Vector2(horizontal, vertical).magnitude > 0.15f){
			//targetPositionOnCircle = new Vector3(horizontal, 0, vertical);
			//targetAngle = Vector3.SignedAngle(Vector3.forward, targetPositionOnCircle, Vector3.up);
			//targetPositionOnCircle = targetPositionOnCircle.normalized * radius;
			
			//angleDifference = Vector3.SignedAngle(transform.position, targetPositionOnCircle, Vector3.up);
			//rotationDifference = Quaternion.AngleAxis(angleDifference, Vector3.up);
			targetAngle -= 2 * (degreeSpeed * Mathf.Abs(horizontal)) * Time.deltaTime * Mathf.Sign(horizontal);
			//lineRenderer.SetPosition(2, currentTarget.transform.position + targetPositionOnCircle + yRaise);
			lineRenderer.SetPosition(2, radius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * targetAngle), 0, Mathf.Cos(Mathf.Deg2Rad * targetAngle)) + yRaise);

		}

		LerpToAnglePosition(); 

		//nextAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, Time.deltaTime * degreeSpeed);
		//nextAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref currentCircleVelocity, smoothTime, degreeSpeed, Time.deltaTime * 2);
		//nextPos = (radius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * nextAngle), 0, Mathf.Cos(Mathf.Deg2Rad * nextAngle)));
		//lineRenderer.SetPosition(1, currentTarget.transform.position + nextPos);
		//currentAngle = Vector3.SignedAngle(Vector3.forward, nextPos, Vector3.up);
		//characterController.Move(currentTarget.transform.position + nextPos - transform.position);
	}

	void TargetPositionUpdate(Vector3 newPosition){
		lineRenderer.SetPosition(0, newPosition + yRaise);
	}

	public void LerpToAnglePosition(){
		nextAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref currentCircleVelocity, smoothTime, degreeSpeed, Time.deltaTime * 2);
		playerAnimator.SetFloat("speed", Mathf.Sqrt(Mathf.Abs(currentCircleVelocity)) * -Mathf.Sign(currentCircleVelocity));
		
		nextPos = (radius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * nextAngle), 0, Mathf.Cos(Mathf.Deg2Rad * nextAngle)));

		lineRenderer.SetPosition(1, currentTarget.transform.position + nextPos + yRaise);

		currentAngle = Vector3.SignedAngle(Vector3.forward, nextPos, Vector3.up);

		characterController.Move((currentTarget.transform.position + nextPos) - transform.position);
		transform.LookAt(currentTarget.transform.position);

		//playerAnimator.SetFloat("speed", 0);

		//lineRenderer.SetPosition(1, currentTarget.transform.position + targetPositionOnCircle + yRaise);

		//characterController.Move((currentTarget.transform.position + targetPositionOnCircle) - transform.position);
	}
}
