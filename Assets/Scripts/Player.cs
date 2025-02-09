using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Propriedades do Player")]
    public float speed = 5f;
    private float initialSpeed;
    private Vector2 moveInput;
    private CharacterController characterController;
    private PlayerInput playerInput;
    private float velocityY = 0f;

    private bool isGrounded;
    public bool isAtacking;

    [Header("Configuração do Pulo")]
    public float jumpForce = 8f; // Força inicial do pulo
    public float maxJumpTime = 0.25f; // Tempo máximo do pulo ao segurar o botão
    private float jumpTimeCounter;
    private bool jumpButtonHeld;

    [Header("Gravidade")]
    public float gravity = -9.81f;
    public float fallMultiplier = 2.5f; // Acelera a queda
    public float lowJumpMultiplier = 2f; // Pulo menor quando o botão é solto rápido
    public float terminalSpeed = -2f; // Velocidade terminal

    // Animação
    private Animator anim;

    [Header("Outros")]
    public int numberOfPlayer;
    public Color[] playersColors;
    private CanvasController canvasController;

    void Awake()
    {
        initialSpeed = speed;
    }

    void Start()
    {
        

        DontDestroyOnLoad(gameObject);

        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();

        SceneManager.sceneLoaded += OnSceneLoaded;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        numberOfPlayer = players.Length;

        Debug.Log($"Jogador {numberOfPlayer} entrou.");
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < players.Length; i++)
        {
            meshes.ToList().Where(meshe => !meshe.gameObject.CompareTag("Espada"))
                .ToList().ForEach(meshe => meshe.material.color = playersColors[i]);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvasController = FindFirstObjectByType<CanvasController>();

        characterController.enabled = false;
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        transform.rotation = Quaternion.Euler(0, 180, 0);
        transform.position = spawnPoints.ToList().Find(spawnPoint => spawnPoint.name == "SpawnPoint" + numberOfPlayer.ToString()).transform.position;
        characterController.enabled = true;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        //Seta a velocidade da animação igual a velocidade real do player
        anim.speed = speed / initialSpeed;
        if (canvasController == null)
        {
            canvasController = FindFirstObjectByType<CanvasController>();
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        isGrounded = characterController.isGrounded;

        if (isGrounded && velocityY < 0)
        {
            velocityY = 0f;
        }

        // Lógica do pulo
        if (jumpButtonHeld && jumpTimeCounter > 0)
        {
            velocityY = jumpForce;
            jumpTimeCounter -= Time.deltaTime;
        }
        else if (!isGrounded)
        {
            if (velocityY > 0)
            {
                velocityY += gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
            else
            {
                velocityY += gravity * (fallMultiplier - 1) * Time.deltaTime;
            }
        }

        velocityY += gravity * Time.deltaTime;
        if(velocityY < terminalSpeed) velocityY = terminalSpeed;
        movement.y = velocityY;

        characterController.Move(movement * speed * Time.deltaTime);

        bool isWalk = movement.x != 0 || movement.z != 0;
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isAtacking", isAtacking);

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
        if (context.started && isGrounded)
        {
            jumpButtonHeld = true;
            jumpTimeCounter = maxJumpTime;
            velocityY = jumpForce;
        }
        else if (context.canceled)
        {
            jumpButtonHeld = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Debug.Log("Run chamado.");
            speed = initialSpeed * 1.25f;
        }
        else if(context.canceled)
        {
            speed = initialSpeed;
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
