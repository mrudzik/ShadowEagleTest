using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

	private Vector2 movementVector = Vector2.zero;
	public Vector2 MovementVector { get { return movementVector.normalized; } }



	private Player playerRef;
    // Start is called before the first frame update
    void Start()
    {
		playerRef = SceneManager.Instance.Player;

	}

    // Update is called once per frame
    void Update()
    {
		MovementInput();
		AttackInput();
    }




	void MovementInput()
	{
		if (Input.GetKeyDown(KeyCode.W))	movementVector.y += 1;
		if (Input.GetKeyUp(KeyCode.W))		movementVector.y -= 1;

		if (Input.GetKeyDown(KeyCode.S))	movementVector.y -= 1;
		if (Input.GetKeyUp(KeyCode.S))		movementVector.y += 1;

		if (Input.GetKeyDown(KeyCode.D))	movementVector.x += 1;
		if (Input.GetKeyUp(KeyCode.D))		movementVector.x -= 1;

		if (Input.GetKeyDown(KeyCode.A))	movementVector.x -= 1;
		if (Input.GetKeyUp(KeyCode.A))		movementVector.x += 1;

	}







	public UI_CooldownDisplay fastAttackCooldown;
	public UI_CooldownDisplay strongAttackCooldown;

	void AttackInput()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0)	&& !fastAttackCooldown.StillCool)
		{// Fast attack
			fastAttackCooldown.StartCooldown(playerRef.FastAttackSpeed);
			playerRef.FastAttack();
		}
		if (Input.GetKeyDown(KeyCode.Mouse1)    && !strongAttackCooldown.StillCool)
		{// Strong attack
			if (playerRef.CanDoStrongAttack)
			{
				strongAttackCooldown.StartCooldown(playerRef.StrongAttackSpeed);
				playerRef.StrongAttack();
			}
		}
	}



}
