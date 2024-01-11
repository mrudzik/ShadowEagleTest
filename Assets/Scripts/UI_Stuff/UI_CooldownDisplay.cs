using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_CooldownDisplay : MonoBehaviour
{
	private float timeToWait = 0;
	private float timer = 0;
	private bool counting = false;
	public bool StillCool { get { return counting; } }



	public Image fillImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
		{
			timer += Time.deltaTime;
			if (timer > timeToWait)
			{
				counting = false;
			}
			fillImage.fillAmount = 1 - (timer / timeToWait);

		}
    }


	public void StartCooldown(float timeToDisplay)
	{
		Debug.Log("Start cooldown");
		timeToWait = timeToDisplay;
		timer = 0;
		counting = true;
	}
}
