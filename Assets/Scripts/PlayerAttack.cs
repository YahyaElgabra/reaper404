using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform opponentTransform;
    private GameObject _opponent;
    private GameObject _opponentBoss;
    
    private const float AttackRange = 3f;
    private const float AttackBossRange = 5f;
    private float _distanceToEnemy;
    private const float AttackAnimationDuration = 1.533f; // Duration of the attack animation in seconds

    public SpawnBoss spawnBoss;
    public UpdatePlayerInfo updatePlayerInfo;
    
    void Start()
    {
        spawnBoss = FindObjectOfType<SpawnBoss>();
    }
    void Update()
    {   
        _opponent = GameObject.FindGameObjectWithTag("Enemy");
        _opponentBoss = GameObject.FindGameObjectWithTag("Boss");

        if (_opponent != null)
        {
            Attack(_opponent, AttackRange);
        }
        if (_opponentBoss != null)
        {
            Attack(_opponentBoss, AttackBossRange);
        }
        
    }

    private void Attack(GameObject opp, float range)
    {
        opponentTransform = opp.transform;
        
        _distanceToEnemy = Vector3.Distance(transform.position, opponentTransform.position);

        if (_distanceToEnemy <= range && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(DestroyEnemyAfterAnimation(opp));
        }
    }

    private IEnumerator DestroyEnemyAfterAnimation(GameObject opp)
    {
        yield return new WaitForSeconds(AttackAnimationDuration);
        
        if (opp.CompareTag("Enemy"))
        {
            Destroy(opp.gameObject);
        }
        if (opp.CompareTag("Boss"))
        {
            spawnBoss.bossLivesLeft -= 1;
            updatePlayerInfo = FindObjectOfType<UpdatePlayerInfo>();
            updatePlayerInfo.UpdateBossLivesText(spawnBoss.bossLivesLeft);
        }
    }
}
