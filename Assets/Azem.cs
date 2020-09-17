using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Azem : MonoBehaviour
{
	public float horizontalSpeed;
	public float jumpForce;

	SpriteRenderer sr;
	Rigidbody2D rb;

	bool onGround;
	float jumpForceAccumulated;

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		Assert.IsTrue(sr != null);
		rb = GetComponent<Rigidbody2D>();
		Assert.IsTrue(rb != null);

		onGround = true;
		jumpForceAccumulated = 1.0f;
	}

	void FixedUpdate()
	{
		// horizontal movement (update by setting velocity directly)
		Vector2 velocity = rb.velocity;

		if (onGround)
		{
			velocity.x = 0.0f;
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				velocity.x -= horizontalSpeed;
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				velocity.x += horizontalSpeed;
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
		}

		if (velocity.x > horizontalSpeed * 1.25f) velocity.x = horizontalSpeed * 1.25f;
		else if (velocity.x < -horizontalSpeed * 1.25f) velocity.x = -horizontalSpeed * 1.25f;

		rb.velocity = velocity;
	}

	// Update is called once per frame
	void Update()
	{
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
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		onGround = true;
	}

	void OnCollisionExit2D(Collision2D col)
	{
		onGround = false;
	}
}
