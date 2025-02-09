using UnityEngine;

public class CobraBoss : MonoBehaviour
{
    public float moveSpeed = 5f;           // Velocidade de movimentação para frente
    public float rotationSpeed = 50f;      // Velocidade de rotação

    void Update()
    {
        // Movimentar a cobra para frente
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Aplicar uma rotação constante para criar um movimento circular
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}