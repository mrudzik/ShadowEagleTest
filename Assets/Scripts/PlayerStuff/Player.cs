using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
	[Header("Stats")]
    public float maxHP;
	private float currentHP;
	public float CurrentHP
	{
		get { return currentHP; }
		set { currentHP = value; healthDisplay.RefreshHP(currentHP, maxHP); }
	}


	public float movementSpeed = 5;
	public float AttackRange = 2;

	[Header("Fast Attack")]
	public float FastDamage = 1;
	public float FastAttackSpeed = 0.6f;
	[Header("Strong Attack")]
	public float StrongDamage = 2;
	public float StrongAttackSpeed = 2f;

	[Header("Animation")]
	public Animator AnimatorController;
	[Header("UI Stuff")]
	public Image strongAttackBlocker;
	public UI_HpDisplay healthDisplay;

	private float lastAttackTime = 0;
    private bool isDead = false;

	private InputController _input;
	private Rigidbody _rb;

	

	private void Start()
	{
		// Initialize stuff
		_input = GetComponent<InputController>();
		_rb = GetComponent<Rigidbody>();

		CurrentHP = maxHP;
	}



	private void Update()
    {
		if (DeathCheck()) return;

		EnemyLogic();
        MovementCheck();

    }



	#region Movement

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

	#endregion


	private void EnemyLogic()
	{
		var enemy = FindClosestEnemy();
		if (enemy == null)
		{
			canDoStrongAttack = false;
			strongAttackBlocker.enabled = true;
			return;
		}
			


		var distance = Vector3.Distance(transform.position, enemy.transform.position);
		canDoStrongAttack = (distance <= AttackRange);

		// UI button block
		if (strongAttackBlocker.enabled == canDoStrongAttack)
			strongAttackBlocker.enabled = !canDoStrongAttack;
	}








	public void FastAttack()
	{
		FastAttackEnemy(FindClosestEnemy());
	}

	private bool canDoStrongAttack = false;
	public bool CanDoStrongAttack { get { return canDoStrongAttack; } }
	public void StrongAttack()
	{
		if (!canDoStrongAttack)
			return;
		StrongAttackEnemy(FindClosestEnemy());
	}











	private Enemie FindClosestEnemy()
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

		return closestEnemie;
	}


	private void FastAttackEnemy(Enemie closestEnemie)
	{
		// Auto attack closest enemie if it close enough
		if (closestEnemie != null)
		{
		//	var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);


			transform.LookAt(new Vector3(closestEnemie.transform.position.x, transform.position.y, closestEnemie.transform.position.z));
			lastAttackTime = Time.time;
			AnimatorController.SetTrigger("Attack");
			StartCoroutine(AttemptToDamage(closestEnemie, FastDamage));
			
		}
	}

	private void StrongAttackEnemy(Enemie closestEnemie)
	{
		if (closestEnemie != null)
		{
			var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
			if (distance <= AttackRange)
			{
				transform.LookAt(new Vector3(closestEnemie.transform.position.x, transform.position.y, closestEnemie.transform.position.z));
				lastAttackTime = Time.time;
				
				AnimatorController.SetTrigger("Attack2");

				StartCoroutine(AttemptToDamage(closestEnemie, StrongDamage));
			}
		}
	}

	private IEnumerator AttemptToDamage(Enemie enemy, float damage)
	{
		yield return new WaitForSeconds(0.4f);
		// Need to wait till animation complete the move, to avoid annoying immediate damage before axe hit

		var distance = Vector3.Distance(transform.position, enemy.transform.position);
		if (distance > AttackRange)
			yield break; // Target is out of reach
		if (isDead)
			yield break; // We are dead

		enemy.CurrentHP -= damage;
		RegenAfterKill(enemy);

	}

		private void RegenAfterKill(Enemie enemy)
	{
		if (enemy.CurrentHP > 0)
			return;

		Debug.Log("Regen");
		CurrentHP += 1;
	}






	
	



	#region Death Logic

	private bool DeathCheck()
	{
		if (isDead)
			return true;

		if (currentHP <= 0)
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
