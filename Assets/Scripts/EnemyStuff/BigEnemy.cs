using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : Enemie
{
	[Header("Big Enemy Logic")]
	public GameObject prefabOfLesserEnemies;



	protected override void Die()
	{
		base.Die();

		SceneManager.Instance.SpawnEnemy(prefabOfLesserEnemies, transform.position + transform.right);
		SceneManager.Instance.SpawnEnemy(prefabOfLesserEnemies, transform.position - transform.right);

	}
}
