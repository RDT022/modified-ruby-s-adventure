using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }
    
    public Image mask;
    public Text score;
    public Text ammo;
    public Text gameOverText;

    float originalSize;
    
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
        gameOverText.text = " ";
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }

    public void SetScore(int s)
    {
        score.text = "Robots Fixed: " + s;
    }
    public void SetAmmo(int a)
    {
        ammo.text = "Gears Left: " + a;
    }
    public void winMessage()
    {
        gameOverText.text = "Congratulations, you won! By: Adam Romanowski [Press R to restart]";
    }
    public void loseMessage()
    {
        gameOverText.text = "Too bad, you lost! [Press R to restart]";
    }
    public void levelMessage()
    {
        gameOverText.text = "Congratulations, you beat the first level! Talk to Jambi the frog to enter level 2!";
    }
}
