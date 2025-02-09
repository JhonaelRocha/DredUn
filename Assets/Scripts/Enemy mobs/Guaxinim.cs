using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Guaxinim : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject mao;
    public bool isHoldingItem;
    Animator anim;
    GameObject[] players;

    GameObject heldItem;
    public float distanceToRunAway = 5f; // Distância para começar a fugir
    public float runAwayDistance = 10f; // Distância que ele vai percorrer ao fugir




    private Vector3 direction; // Direção do movimento

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        anim.SetBool("isWalk", agent.velocity.magnitude > 0.1f);
        if (agent == null) return;

        // Se não estiver segurando um item, procurar um
        if (heldItem == null)
        {
            GameObject[] _items = GameObject.FindGameObjectsWithTag("Item");
            List<GameObject> items = _items.Where(item => !item.GetComponent<ItemType>().isBeenHold).ToList();

            if (items.Count == 0)
            {
                // Se não houver mais itens, o guaxinim para de se mover
                agent.ResetPath();
                agent.velocity = Vector3.zero;


                //ERA PARA FICAR ZANZANDO POR AI: ARRUMAR
                if (!agent.hasPath || agent.remainingDistance < 0.5f)
                {
                    SetRandomDirection();
                }
                //----

                
                return; // Sai do Update() para evitar chamadas desnecessárias
            }

            GameObject nearestItem = items
                .OrderBy(item => Vector3.Distance(transform.position, item.transform.position))
                .FirstOrDefault();

            if (nearestItem != null)
            {
                agent.SetDestination(nearestItem.transform.position);

                float distanceToItem = Vector3.Distance(transform.position, nearestItem.transform.position);
                if (distanceToItem < 0.75f)
                {
                    heldItem = nearestItem;
                    nearestItem.transform.parent = mao.transform;
                    nearestItem.gameObject.transform.localScale = Vector3.one * 0.5f;
                    nearestItem.GetComponent<ItemType>().isBeenHold = true;
                    nearestItem.transform.localPosition = Vector3.zero;
                }
            }
        }

        // Se estiver segurando um item, fugir dos jogadores
        if (heldItem != null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            if (players.Length > 0)
            {
                GameObject nearestPlayer = players
                    .OrderBy(player => Vector3.Distance(transform.position, player.transform.position))
                    .FirstOrDefault();

                if (nearestPlayer != null)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, nearestPlayer.transform.position);

                    if (distanceToPlayer < distanceToRunAway)
                    {
                        Vector3 directionAwayFromPlayer = (transform.position - nearestPlayer.transform.position).normalized;
                        Vector3 runToPosition = transform.position + directionAwayFromPlayer * runAwayDistance;

                        agent.SetDestination(runToPosition);
                    }
                }
            }
        }
    }




    private void SetRandomDirection()
    {
        // Define uma direção aleatória no plano XZ
        direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        // Ajusta o destino do NavMeshAgent baseado na nova direção
        Vector3 newDestination = transform.position + direction * 10f; // Definir uma distância para o novo destino
        agent.SetDestination(newDestination);
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Quando colidir com uma parede (bordas), mudar a direção
        if (collision.gameObject.CompareTag("Wall")) // Certifique-se de que as bordas têm a tag "Wall"
        {
            ContactPoint contact = collision.contacts[0];

            if (Mathf.Abs(contact.normal.x) > 0.5f) // Colisão no eixo X
            {
                direction.x = -direction.x; // Inverte a direção no eixo X
            }

            if (Mathf.Abs(contact.normal.z) > 0.5f) // Colisão no eixo Z
            {
                direction.z = -direction.z; // Inverte a direção no eixo Z
            }

            // Atualiza o destino do NavMeshAgent com a nova direção
            Vector3 newDestination = transform.position + direction * 10f;

            agent.SetDestination(newDestination);
        }

    }
}
