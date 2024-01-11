using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] public GameObject toFollow;

	private void Update()
	{
		if (toFollow != null)
			transform.position = new Vector3(toFollow.transform.position.x, transform.position.y, toFollow.transform.position.z);
	}
}
