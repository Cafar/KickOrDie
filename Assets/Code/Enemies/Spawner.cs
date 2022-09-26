using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public string path;

	public void InstanceEnemy(string _nameEnemy)
	{
		Instantiate(Resources.Load<EnemyController>(path + _nameEnemy), transform.localPosition, transform.localRotation);
	}

	public void InstanceBoss(string _nameEnemy)
	{
		Instantiate(Resources.Load(path + _nameEnemy));
	}
}
