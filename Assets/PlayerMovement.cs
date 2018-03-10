using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public TargetDebug currentTarget;

	public float radius, degreeSpeed, smoothTime;
	float targetAngle, currentAngle, angleDifference, nextAngle, currentCircleVelocity;
	Quaternion rotationDifference;
	Vector3 targetPositionOnCircle, nextPos;
	CharacterController characterController;
	Coroutine lerpToAngleCor;

	LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {

		if(currentTarget == null){
			GameObject newTarget = GameObject.CreatePrimitive(PrimitiveType.Cube);
			currentTarget = newTarget.AddComponent<TargetDebug>();
			currentTarget.RepositionCurrentTargetTo(Vector3.zero);
		}

		currentTarget.posChangeEvent += TargetPositionUpdate;

		characterController = GetComponent<CharacterController>();
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetPosition(0, currentTarget.transform.position);
		currentAngle = Vector3.SignedAngle(Vector3.forward, transform.position, Vector3.up);
		
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		if(new Vector2(horizontal, vertical).magnitude > 0.8f){
			targetPositionOnCircle = new Vector3(horizontal, 0, vertical);
			targetAngle = Vector3.SignedAngle(Vector3.forward, targetPositionOnCircle, Vector3.up);
			targetPositionOnCircle = targetPositionOnCircle.normalized * radius;
			lineRenderer.SetPosition(2, currentTarget.transform.position + targetPositionOnCircle);
			//angleDifference = Vector3.SignedAngle(transform.position, targetPositionOnCircle, Vector3.up);
			//rotationDifference = Quaternion.AngleAxis(angleDifference, Vector3.up);
			if(lerpToAngleCor == null)
				lerpToAngleCor = StartCoroutine(LerpToAnglePosition());
		}

		//nextAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, Time.deltaTime * degreeSpeed);
		//nextAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref currentCircleVelocity, smoothTime, degreeSpeed, Time.deltaTime * 2);
		//nextPos = (radius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * nextAngle), 0, Mathf.Cos(Mathf.Deg2Rad * nextAngle)));
		//lineRenderer.SetPosition(1, currentTarget.transform.position + nextPos);
		//currentAngle = Vector3.SignedAngle(Vector3.forward, nextPos, Vector3.up);
		//characterController.Move(currentTarget.transform.position + nextPos - transform.position);
	}

	void TargetPositionUpdate(Vector3 newPosition){
		lineRenderer.SetPosition(0, newPosition);
	}

	IEnumerator LerpToAnglePosition(){
		while(Mathf.Abs(targetAngle - currentAngle) > 0.25f){
			nextAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref currentCircleVelocity, smoothTime, degreeSpeed, Time.deltaTime * 2);
			
			nextPos = (radius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * nextAngle), 0, Mathf.Cos(Mathf.Deg2Rad * nextAngle)));

			lineRenderer.SetPosition(1, currentTarget.transform.position + nextPos);

			currentAngle = Vector3.SignedAngle(Vector3.forward, nextPos, Vector3.up);

			characterController.Move(currentTarget.transform.position + nextPos - transform.position);
			yield return null;
		}

		lineRenderer.SetPosition(1, currentTarget.transform.position + targetPositionOnCircle);

		characterController.Move(currentTarget.transform.position + (targetPositionOnCircle - transform.position));

		lerpToAngleCor = null;

		Debug.Log("Lerping stopped");

		yield return null;
		
	}
}
