using UnityEngine;
using System.Collections;

public class EnemyTurret : MonoBehaviour {
	public bool isObjective = false;

	public Collider2D hitBox;
	//public GameObject shootPoint;
	public GameObject turretExplosion;
	//public GameObject projectile;

	private Transform player;

	//public float projectileSpeed;
	public float timeBetweenSteps;
	private float lastStep;
	public float health;
	public float damage;

	void Start() {
		player = GameObject.FindGameObjectWithTag("player").transform;
	}


	void Update() {
		if (player.GetComponent<Collider2D>().IsTouching(hitBox) && !player.GetComponent<PrototypePlayer>().getDeathState()) {
			LookAtPlayer();
			FireTurret();
		}

	}

	void HitDamage(float damage) {
		
		if ((health -= damage) < 0) {
			DestroySelf();
		}

	}

	void DestroySelf() {
		Instantiate(turretExplosion, transform.position, transform.rotation);
		Destroy(gameObject);
		if (isObjective == true) {
			GameObject.Find ("ObjectiveSystem").GetComponent<LevelEndScript>().IncrementTargetsDestroyed();
		}
	}

	void FireTurret() {
		if (Time.time - lastStep > timeBetweenSteps) {
			GetComponent<ProjectileController> ().shootNormalProjectile ();
			lastStep = Time.time;
		}
	}

	void LookAtPlayer() {
		Vector3 dir = player.position - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
	}
}
