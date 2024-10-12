using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Propriedades do Player")]
    public float speed = 5f;
    private Vector2 moveInput;
    private CharacterController characterController;
    private PlayerInput playerInput;
    private float velocityY = 0f; // Gravidade
    
    private bool isGrounded;
    public bool isAtacking;
    
    [Header("Gravidade")]
    public float gravity = -9.81f;

    // Animação
    private Animator anim;

    [Header("Outros")]
    public int numberOfPlayer;
    public Color[] playersColors;
    private CanvasController canvasController;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();

        // Registrar o método OnSceneLoaded para ser chamado quando uma nova cena for carregada
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        numberOfPlayer = players.Length;

        Debug.Log($"Jogador {numberOfPlayer} entrou.");
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        
        for (int i = 0; i < players.Length; i++)
        {
            meshes.ToList().Where(meshe => !meshe.gameObject.CompareTag("Espada")).ToList().ForEach(meshe => meshe.material.color = playersColors[i]);
        }
    }

    // Método que será chamado quando uma nova cena for carregada
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Atualizar a referência ao CanvasController se necessário
        canvasController = FindFirstObjectByType<CanvasController>();

        characterController.enabled = false;
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        transform.rotation = Quaternion.Euler(0,180,0);
        transform.position = spawnPoints.ToList().Find(spawnPoint => spawnPoint.name == "SpawnPoint" + numberOfPlayer.ToString()).transform.position;
        characterController.enabled = true;
    }

    void OnDestroy()
    {
        // Remover o método OnSceneLoaded do evento quando o objeto for destruído
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (canvasController == null)
        {
            canvasController = FindFirstObjectByType<CanvasController>();
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        
        // Checar se o jogador está no chão
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocityY < 0)
        {
            velocityY = 0f; // Reseta a velocidade vertical quando no chão
        }

        // Aplica gravidade
        velocityY += gravity * Time.deltaTime;
        movement.y = velocityY;

        // Aplica o movimento com a velocidade do jogador
        characterController.Move(movement * speed * Time.deltaTime);

        // Define a animação de andar
        bool isWalk = movement.x != 0 || movement.z != 0;
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isAtacking", isAtacking);

        // Rotação suave em direção ao movimento
        if (movement.x != 0 || movement.z != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // isAtacking = true;
        }
    }

    public void isAtackingToFalse()
    {
        // isAtacking = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Item":
                canvasController.currentItems++;
                canvasController.AtualizarItemsLabel();
                Destroy(collider.gameObject);
                break;

            case "Enemy":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Debug.Log("Colidi");
                break;
        }
    }
}
