using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour {

    public Player player;
    public Sprite nextSprite;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if(player.getKey == false)
        {
            return;
        }
        else
        {
            LoadSprite();
        }
    }

    public void LoadSprite()
    {
         GetComponent<Image>().sprite = nextSprite;
    }
}
