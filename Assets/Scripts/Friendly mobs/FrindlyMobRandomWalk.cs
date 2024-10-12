using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyMobRandomWalk : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;

    [Header("Propriedades")]
    public float speed = 2f;
    public float rotationSpeed = 500f;
    public float walkRadius = 5f;
    public float minTimeToWalk = 1f, maxTimeToWalk = 3f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed; 
        agent.angularSpeed = rotationSpeed;

        anim = GetComponent<Animator>();
        StartCoroutine(WalkCoroutine());
    }

    void Update()
    {
        anim.SetBool("isWalk", agent.velocity.magnitude > 0.1f);
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

            agent.ResetPath(); //Remove o destino do agente fazendo ele parar.
            yield return new WaitForSeconds(timeToWalk);
        }
    }
}