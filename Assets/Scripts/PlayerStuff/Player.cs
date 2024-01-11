using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("Health")]
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;

	public float movementSpeed = 5;

    private float lastAttackTime = 0;
    private bool isDead = false;
    public Animator AnimatorController;

	private InputController _input;
	private Rigidbody _rb;

	

	private void Start()
	{
		// Initialize stuff
		_input = GetComponent<InputController>();
		_rb = GetComponent<Rigidbody>();
	}



	private void Update()
    {
		if (DeathCheck()) return;

	//	EnemyLogic();
        MovementCheck();





        /**/
    }


	//(transform.forward * Mathf.Clamp(_input.MovementVector.magnitude, 0, 1))


	// For optimization purposes, to not allocate it each tick and avoid memory fragmentation
	private Vector3 moveVector = Vector3.zero; 
	private void MovementCheck()
	{
		//Debug.Log("Movement vector = " + _input.MovementVector.ToString());
		if (_input.MovementVector.magnitude > 0)
		{
			moveVector.x = _input.MovementVector.x;
			moveVector.z = _input.MovementVector.y;

			_rb.MovePosition(transform.position + moveVector * movementSpeed * Time.fixedDeltaTime);

			LookAtDirection(moveVector);
		}

	}

	private void LookAtDirection(Vector3 lookDir)
	{
		var skewedInput = lookDir;
		var relative = (transform.position + skewedInput) - transform.position;
		var rot = Quaternion.LookRotation(relative, Vector3.up);

		transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 300 * Time.deltaTime);
	}





	private void EnemyLogic()
	{
		var enemies = SceneManager.Instance.Enemies;
		Enemie closestEnemie = null;

		// Find closest Enemie among all other enemies
		for (int i = 0; i < enemies.Count; i++)
		{
			var enemie = enemies[i];
			if (enemie == null)
			{
				continue;
			}

			if (closestEnemie == null)
			{
				closestEnemie = enemie;
				continue;
			}

			var distance = Vector3.Distance(transform.position, enemie.transform.position);
			var closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

			if (distance < closestDistance)
			{
				closestEnemie = enemie;
			}

		}


		// Auto attack closest enemie if it close enough
		if (closestEnemie != null)
		{
			var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
			if (distance <= AttackRange)
			{
				if (Time.time - lastAttackTime > AtackSpeed)
				{
					//transform.LookAt(closestEnemie.transform);
					transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

					lastAttackTime = Time.time;
					closestEnemie.Hp -= Damage;
					AnimatorController.SetTrigger("Attack");
				}
			}
		}
	}

	#region Death Logic

	private bool DeathCheck()
	{
		if (isDead)
			return true;

		if (Hp <= 0)
		{
			Die();
			return true;
		}

		return false;
	}
    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }

	#endregion



}
