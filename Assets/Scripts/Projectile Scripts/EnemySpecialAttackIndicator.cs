using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialAttackIndicator : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        transform.position = player.transform.position;
    }

    public IEnumerator SpawnProjectileAfterDelay(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 targetPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Instantiate(projectile, targetPos, Quaternion.identity);
        Destroy(gameObject);
    }
}
