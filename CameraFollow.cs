using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject[] playerModels; 
    private Transform player; 
    private Rigidbody playerRb;
    public Vector3 Offset; 
    public float speed; 
    public string stringCarToFollow;
    
    private void Start()
    {
        Debug.Log(stringCarToFollow); 
        stringCarToFollow = PlayerPrefs.GetString("selectedCharacter"); 
        
         for(int i = 0; i < playerModels.Length; i++) {
            playerModels[i].SetActive(false); 
         }
        for(int i = 0; i < playerModels.Length; i++) {
            if(playerModels[i].name.Equals(stringCarToFollow)) {
                playerModels[i].SetActive(true); 
                player = playerModels[i].transform; 
            }

        }

        playerRb = player.GetComponent<Rigidbody>(); 

        
    }
    void FixedUpdate() {
        Vector3 playerForward = (playerRb.velocity+player.transform.forward).normalized; 
        transform.position = Vector3.Lerp(transform.position, 
        player.position+player.transform.TransformVector(Offset)+playerForward*(-1.5f), 
        speed*Time.deltaTime); 
        transform.LookAt(player);  
    }


}
