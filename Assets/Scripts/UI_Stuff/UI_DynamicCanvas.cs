using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DynamicCanvas : MonoBehaviour
{
	private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
		cam = SceneManager.Instance.playerCam;
    }

    // Update is called once per frame
    void Update()
    {
		//    transform.LookAt(cam.transform.position);
		transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
