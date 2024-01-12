using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    public float maxHP;
	private float currentHP;
	public float CurrentHP 
	{
		get { return currentHP; }
		set { currentHP = value; healthDisplay.RefreshHP(currentHP, maxHP); }
	}


    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;


    public Animator AnimatorController;
    public NavMeshAgent Agent;

    private float lastAttackTime = 0;
    private bool isDead = false;

	[Header("UI Stuff")]
	public UI_HpDisplay healthDisplay;

    private void Start()
    {
        SceneManager.Instance.AddEnemie(this);
        Agent.SetDestination(SceneManager.Instance.Player.transform.position);

		CurrentHP = maxHP;

    }

    private void Update()
    {
        if(isDead)
        {
            return;
        }

        if (CurrentHP <= 0)
        {
            Die();
            Agent.isStopped = true;
            return;
        }

		var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);
     
        if (distance <= AttackRange)
        {
            Agent.isStopped = true;
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
                AnimatorController.SetTrigger("Attack");

				StartCoroutine(AttemptToAttack());
            }
        }
        else
        {
			if (Time.time - lastAttackTime > AtackSpeed)
			{
				Agent.isStopped = false;
				Agent.SetDestination(SceneManager.Instance.Player.transform.position);
			}
        }
        AnimatorController.SetFloat("Speed", Agent.speed); 
    }

	private IEnumerator AttemptToAttack()
	{
		yield return new WaitForSeconds(0.4f);
		// Need to wait till animation complete the move, to avoid annoying immediate damage before axe hit
		
		var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);
		if (distance > AttackRange)
			yield break; // Target is out of reach
		if (isDead)
			yield break; // We are dead

		SceneManager.Instance.Player.CurrentHP -= Damage;

	}



    protected virtual void Die()
    {
        SceneManager.Instance.RemoveEnemie(this);
        isDead = true;
        AnimatorController.SetTrigger("Die");
    }

}
