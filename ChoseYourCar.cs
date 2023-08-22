using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ChoseYourCar : MonoBehaviour
{
    public GameObject[] Cars; 
    public int count = 0;  
    public string selectedCharacter; 

    // Start is called before the first frame update
    void Start()
    {
        selectedCharacter = Cars[count].name; 
        for(int i = 0; i < Cars.Length; i++) {
            Cars[i].SetActive(false); 
        }
        Cars[count].SetActive(true);      
    }
    public void previousCar() {
        if(count > 0) {
        Debug.Log("previous");
            Cars[count].SetActive(false);
            count--; 
            Cars[count].SetActive(true); 
            selectedCharacter = Cars[count].name; 
        }

    }
        public void nextCar() {
            if(count < Cars.Length -1) {
        Debug.Log("next");
            Cars[count].SetActive(false);
            count++; 
            Cars[count].SetActive(true); 
            selectedCharacter = Cars[count].name;
            }
    }
    public void checkStart() {
            PlayerPrefs.SetString("selectedCharacter", selectedCharacter); 
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single); 
    }

}
