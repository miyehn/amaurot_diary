using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// use a 2nd degree bezier to represent the stack movement;
// see: https://www.cg.tuwien.ac.at/research/publications/2017/JAHRMANN-2017-RRTG/JAHRMANN-2017-RRTG-draft.pdf
// (section 5.2)
public class WavingStack : MonoBehaviour
{
	public float rootX;
	public float v1Y;
	public float length = 4.0f;

	public GameObject v2Control;

	public GameObject v0Sprite;
	public GameObject v1Sprite;
	public GameObject v2Sprite;
	public float freeT;
	public GameObject vFreePt;

	Vector2 v0;
	Vector2 v1;
	Vector2 v2;

	void Awake()
	{
		v0 = new Vector2(0, 0);
		v1 = new Vector2(0, 0);
	}

	Vector2 getPoint(float t)
	{
		Vector2 v01 = Vector2.Lerp(v0, v1, t);
		Vector2 v12 = Vector2.Lerp(v1, v2, t);
		return Vector2.Lerp(v01, v12, t);
	}

	// Update is called once per frame
	void Update()
	{
		v0.x = rootX;
		v1.x = rootX; v1.y = v1Y;
		v2 = new Vector2(v2Control.transform.position.x, v2Control.transform.position.y);

		// constrain to specified length
		float L0 = (v2 - v0).magnitude;
		float L1 = (v2 - v1).magnitude + (v1 - v0).magnitude;
		float L = (2 * L0 + L1) / 3.0f;
		float ratio = length / L;
		v1.y *= ratio;
		v2 = v0 + (v2 - v0) * ratio;

		v0Sprite.transform.position = v0;
		v1Sprite.transform.position = v1;
		v2Sprite.transform.position = v2;

		vFreePt.transform.position = getPoint(freeT);
	}
}
