using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreElement : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text scoreText;
    public TMP_Text nameText;
    // Start is called before the first frame update
    public void Setup(int rank, int score, string nameString)
    {
        rankText.text = rank.ToString();
        scoreText.text = score.ToString();
        nameText.text = nameString;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
