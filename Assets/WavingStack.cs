//#define SHOW_CTRL_PTS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// use a 2nd degree bezier to represent the stack movement;
// see: https://www.cg.tuwien.ac.at/research/publications/2017/JAHRMANN-2017-RRTG/JAHRMANN-2017-RRTG-draft.pdf
// (section 5.2)
public class WavingStack : MonoBehaviour
{
	//public float v1Y;
	float v1Y;
	//public float offset;
	float offset;

	public float waveSpeed;
	public float waveMagnitude;

	[Space(10)]
	public SpriteRenderer stackTop;

	[Header("Debug")]
	public GameObject v0Sprite;
	public GameObject v1Sprite;
	public GameObject v2Sprite;
	public float freeT;
	public GameObject vFreePt;

	float length;

	Vector2 v0;
	Vector2 v1;
	Vector2 v2;

	List<BookContainer> books;

	void Awake()
	{
		v0 = new Vector2(0, 0);
		v1 = new Vector2(0, 0);

		length = stackTop.gameObject.transform.position.y;
		v1Y = length / 2.0f;
		
		books = new List<BookContainer>();
		BookContainer[] containers = GetComponentsInChildren<BookContainer>();
		foreach (BookContainer container in containers)
		{
			container.t = container.gameObject.transform.position.y / length;
			books.Add(container);
		}

#if SHOW_CTRL_PTS
		v0Sprite.SetActive(true);
		v1Sprite.SetActive(true);
		v2Sprite.SetActive(true);
		vFreePt.SetActive(true);
#else
		stackTop.enabled = false;
#endif

		Debug.Log("initialized a stack of " + books.Count + " books");
	}

	Vector2 getPoint(float t)
	{
		Vector2 v01 = Vector2.Lerp(v0, v1, t);
		Vector2 v12 = Vector2.Lerp(v1, v2, t);
		return Vector2.Lerp(v01, v12, t);
	}

	// returns tangent vector pointing toward v2 (top)
	Vector2 getTangent(float t)
	{
		Vector2 v01 = Vector2.Lerp(v0, v1, t);
		Vector2 v12 = Vector2.Lerp(v1, v2, t);
		return (v12 - v01).normalized;
	}

	void Update()
	{
		offset = Mathf.Sin(waveSpeed * Time.realtimeSinceStartup) * waveMagnitude;

		v0.x = transform.position.x;
		v1.x = transform.position.x; v1.y = v1Y;
		v2 = new Vector2(transform.position.x + offset, length);

		// constrain curve to specified length
		float L0 = (v2 - v0).magnitude;
		float L1 = (v2 - v1).magnitude + (v1 - v0).magnitude;
		float L = (2 * L0 + L1) / 3.0f;
		float ratio = length / L;
		v1.y *= ratio;
		v2 = v0 + (v2 - v0) * ratio;

#if SHOW_CTRL_PTS
		v0Sprite.transform.position = v0;
		v1Sprite.transform.position = v1;
		v2Sprite.transform.position = v2;
		vFreePt.transform.position = getPoint(freeT);
#endif

		foreach (BookContainer b in books)
		{
			b.transform.position = getPoint(b.t);
			Vector2 tangent = getTangent(b.t);
			float angle = Vector2.SignedAngle(Vector2.up, tangent);
			b.transform.eulerAngles = new Vector3(0, 0, angle);
		}
	}
}
