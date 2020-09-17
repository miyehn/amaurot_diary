using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Azem : MonoBehaviour
{
	public float horizontalSpeed;
	public float jumpForce;

	public Camera cam;

	SpriteRenderer sr;
	Rigidbody2D rb;
	CircleCollider2D cc;

	bool onGround;
	float jumpForceAccumulated;

	void Awake()
	{
		Assert.IsTrue(cam != null);
		sr = GetComponent<SpriteRenderer>();
		Assert.IsTrue(sr != null);
		rb = GetComponent<Rigidbody2D>();
		Assert.IsTrue(rb != null);
		cc = GetComponent<CircleCollider2D>();
		Assert.IsTrue(cc != null);

		onGround = true;
		jumpForceAccumulated = 1.0f;

		MoveCamera(true);
	}

	void FixedUpdate()
	{
		// horizontal movement (update by setting velocity directly)
		Vector2 velocity = rb.velocity;

		if (onGround)
		{
			bool left = Input.GetKey(KeyCode.LeftArrow);
			bool right = Input.GetKey(KeyCode.RightArrow);
			if (left && right)
			{
				velocity.x = 0.0f;
			}
			else if (left)
			{
				velocity.x = -horizontalSpeed;
			}
			if (right)
			{
				velocity.x = horizontalSpeed;
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				velocity.x -= horizontalSpeed * 0.05f;
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				velocity.x += horizontalSpeed * 0.05f;
			}
			if (velocity.x > horizontalSpeed * 1.25f) velocity.x = horizontalSpeed * 1.25f;
			else if (velocity.x < -horizontalSpeed * 1.25f) velocity.x = -horizontalSpeed * 1.25f;
		}

		rb.velocity = velocity;
	}

	// Update is called once per frame
	void Update()
	{
		/*
		if (Physics2D.Raycast(
					transform.position + new Vector3(0, -cc.radius-0.001f, 0), 
					Vector2.down, 
					0.1f))
		{
			onGround = true;
		}
		else
		{
			onGround = false;
		}
		*/

		// jump
		if (onGround)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				jumpForceAccumulated += Time.deltaTime * 0.5f;
				if (jumpForceAccumulated > 1.5f) jumpForceAccumulated = 1.5f;
			}
			if (Input.GetKeyUp(KeyCode.Space))
			{
				float velocityX = rb.velocity.x;
				rb.AddForce(new Vector2(velocityX, jumpForce * jumpForceAccumulated));
				jumpForceAccumulated = 1.0f;
			}
		}

		MoveCamera();
	}

	void MoveCamera(bool immediate = false)
	{
		float cameraTargetY = rb.position.y + 2.0f;
		float cameraNextY = 
			immediate ? cameraTargetY : Mathf.Lerp(cam.transform.position.y, cameraTargetY, Time.deltaTime * 3);

		Vector3 position = cam.transform.position;
		position.y = cameraNextY;
		cam.transform.position = position;
	}

	// TODO: use FixedJoint to constrain character to contact point
	void OnCollisionEnter2D(Collision2D col)
	{
		onGround = true;
	}

	void OnCollisionExit2D(Collision2D col)
	{
		onGround = false;
	}
}
