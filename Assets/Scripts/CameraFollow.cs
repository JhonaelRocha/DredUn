using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject player;
    public float lerpSpeed, zDistance;
    Vector3 pointToFollow;
    GameObject[] players;
    void FixedUpdate()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length > 1)
        {
            pointToFollow = Vector3.zero;
            Vector3 _point = Vector3.zero;
            for(int i = 0; i < players.Length; i++)
            {
                _point += players[i].transform.position;
            }
            pointToFollow = _point / players.Length;
        }
        else
        {
            try
            {
                player = GameObject.FindGameObjectWithTag("Player");
                pointToFollow = player.transform.position;
            }
            catch
            {
                return;
            }
            
        }


        float posX = Mathf.Lerp(transform.position.x, pointToFollow.x, lerpSpeed);
        float posZ = Mathf.Lerp(transform.position.z, pointToFollow.z + zDistance, lerpSpeed);
        
        transform.position = new Vector3(posX,transform.position.y,posZ);
    }
}
