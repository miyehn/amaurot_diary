using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Azem : MonoBehaviour
{
	public float horizontalForce;
	public float jumpForce;

	SpriteRenderer sr;
	Rigidbody2D rb;

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		Assert.IsTrue(sr != null);
		rb = GetComponent<Rigidbody2D>();
		Assert.IsTrue(rb != null);
	}

	void handleMovement()
	{
		// jump
		if (Input.GetKeyDown(KeyCode.Space))
		{
			rb.AddForce(new Vector2(0, jumpForce));
		}

		// horizontal movement
		if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
		{
			rb.velocity = Vector2.zero;
			return;
		}

		Vector2 horizontalForceVec = Vector2.zero;
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			horizontalForceVec += new Vector2(-horizontalForce, 0);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			horizontalForceVec += new Vector2(horizontalForce, 0);
		}
		rb.AddForce(horizontalForceVec);
	}

	// Update is called once per frame
	void Update()
	{
		handleMovement();
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log("Collision with " + col.gameObject.name);
	}

	void OnCollisionExit2D(Collision2D col)
	{
		Debug.Log("Left contact with " + col.gameObject.name);
	}
}
