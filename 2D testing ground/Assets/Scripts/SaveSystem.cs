using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class SaveSystem : MonoBehaviour
{
    string scoreKey = "Score";

    public int CurrentScore { get; set; }

    

    private void Awake()
    {
        CurrentScore = PlayerPrefs.GetInt(scoreKey);

        
    }

    public void SetScore(int score)
    {
        PlayerPrefs.SetInt(scoreKey, score);
    }


   



}