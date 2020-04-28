using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggiornaSprite : MonoBehaviour
{
    public Sprite fronteCarta;
    public Sprite retroCarta;

    private SpriteRenderer spriteRenderer;
    private Selezionabile selezionabile;
    private Solitaire solitario;
    private UserInput userInput;

    
    void Start()
    {
        List<string> mazzo = Solitaire.generaMazzo();
        solitario = FindObjectOfType<Solitaire>();
        userInput = FindObjectOfType<UserInput>();
        int i = 0;
        foreach(string carta in mazzo)
        {
            if(this.name == carta)
            {
                fronteCarta = solitario.fronteCarte[i];
                break;
            }
            i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        selezionabile = GetComponent<Selezionabile>();
    }

  
    void Update()
    {
        if(selezionabile.facciaInSu == true)
        {
            spriteRenderer.sprite = fronteCarta;
        }
        else
        {
            spriteRenderer.sprite = retroCarta;
        }
        if (userInput.slot1)
        {
            if (name == userInput.slot1.name)
            {
                spriteRenderer.color = Color.yellow;

            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
    }
}
