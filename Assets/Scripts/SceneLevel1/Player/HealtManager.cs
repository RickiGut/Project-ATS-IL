using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealtManager : MonoBehaviour
{
    public static int health = 3;
    public Image[] hearts;

    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Awake(){
        health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Image gmbr in hearts){
            gmbr.sprite = emptyHeart;
        }

        for(int i = 0; i < health; i++){
            hearts[i].sprite = fullHeart;
        } 
        
    }
}
