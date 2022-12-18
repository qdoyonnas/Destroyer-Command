using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Projectile projectile = other.GetComponent<Projectile>();
        if( projectile != null && projectile.alive) {
            projectile.Explode();
            Destroy(projectile.gameObject);
            return;
        }
        Ship ship = other.GetComponent<Ship>();
        if( ship != null && ship.alive ) {
            ship.Explode();
            PlayerControls player = other.GetComponent<PlayerControls>();
            if( player != null ) {
                GameManager.instance.PlayerDeath(player);
            }
            Destroy(ship.GetAimer().gameObject);
            Destroy(ship.gameObject);
            return;
        }
	}
}
