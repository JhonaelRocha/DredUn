using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuvem : MonoBehaviour
{
    public float lifeTime;
    public float speed;
    void Start()
    {
        StartCoroutine(deSpawn());
    }
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    IEnumerator deSpawn()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
