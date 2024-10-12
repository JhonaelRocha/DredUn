using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Cobra : MonoBehaviour
{
    public float speed = 5f; // Velocidade do inimigo
    private Vector3 direction; // Direção do movimento
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        // Inicialize o inimigo com uma direção aleatória
        SetRandomDirection();
    }

    void Update()
    {
        // Se o inimigo estiver perto de seu destino ou parou, procurar uma nova direção
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            SetRandomDirection();
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
