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

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        anim.SetBool("isWalk", agent.velocity.magnitude > 0.1f);
        if (agent == null) return;

        // Verifique se o guaxinim já está segurando um item
        if (heldItem == null)
        {
            GameObject[] _items = GameObject.FindGameObjectsWithTag("Item");
            List<GameObject> items = _items.Where(item => !item.GetComponent<ItemType>().isBeenHold).ToList();

            if (items.Count == 0) return;

            GameObject nearestItem = items
                .OrderBy(item => Vector3.Distance(transform.position, item.transform.position))
                .FirstOrDefault();

            if (nearestItem == null) return;
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

        // Se o guaxinim estiver segurando um item, ele deve fugir
        if (heldItem != null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            
            GameObject nearestPlayer = null;

            try
            {
                nearestPlayer = players
                    .OrderBy(player => Vector3.Distance(transform.position, player.transform.position))
                    .FirstOrDefault();

                float distanceToPlayer = Vector3.Distance(transform.position, nearestPlayer.transform.position);

                // Se o jogador estiver dentro da distância de fuga, calcular a direção oposta
                if (distanceToPlayer < distanceToRunAway)
                {
                    // Direção oposta ao jogador
                    Vector3 directionAwayFromPlayer = (transform.position - nearestPlayer.transform.position).normalized;
                    Vector3 runToPosition = transform.position + directionAwayFromPlayer * runAwayDistance;

                    // Definir o destino para a posição de fuga
                    agent.SetDestination(runToPosition);
                }
            }
            catch
            {
                return;
            }
        }
    }
}
