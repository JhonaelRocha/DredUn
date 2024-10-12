using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuvemGenerator : MonoBehaviour
{
    public GameObject[] nuvens;
    public float minNuvemSpeed, maxNuvemSpeed;
    public float generationSpeed;
    public float minZ, maxZ;
    public float nuvensLifeTime = 5f;


    void Start()
    {
        Gerar();
        StartCoroutine(Gerador());
    }

    IEnumerator Gerador()
    {
        yield return new WaitForSeconds(generationSpeed);
        Gerar();
        StartCoroutine(Gerador());
    }

    void Gerar()
    {
        Vector3 localSpawn = new Vector3(transform.position.x, transform.position.y, Random.Range(transform.position.z + minZ, transform.position.z + maxZ));
        GameObject _nuvem = Instantiate(nuvens[Random.Range(0,nuvens.Length)],localSpawn, Quaternion.identity);
        _nuvem.GetComponent<Nuvem>().speed = Random.Range(minNuvemSpeed, maxNuvemSpeed);
        _nuvem.GetComponent<Nuvem>().lifeTime = nuvensLifeTime;
    }
}
