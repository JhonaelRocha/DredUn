using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Porcao : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    GameObject[] players;
    public float walkRadius = 3f;
    public float minTimeToWalk = 1f;
    public float maxTimeToWalk = 3f;
    public float distanceToFollow = 5f;
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(WalkCoroutine());
    }
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        anim.SetBool("isWalk", agent.velocity.magnitude > 0.1f);
        GameObject nearestPlayer = null;

        try
        {
            nearestPlayer = players
            .OrderBy(player => Vector3.Distance(transform.position, player.transform.position))
            .FirstOrDefault();
            
            if (Vector3.Distance(nearestPlayer.transform.position, transform.position) < distanceToFollow)
            {
                StopCoroutine("WalkCoroutine");
                agent.SetDestination(nearestPlayer.transform.position);
            }
        }
        catch
        {
            return;
        }

    }

    IEnumerator WalkCoroutine()
    {
        while (true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            float timeToWalk = Random.Range(minTimeToWalk, maxTimeToWalk);
            yield return new WaitForSeconds(timeToWalk);
            agent.ResetPath();
            yield return new WaitForSeconds(timeToWalk);
        }
    }
}
