using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    public GameObject pannelloVittoria;
    public Text mosse;
    public Text punti;
    
    void Update()
    {
        mosse.text = "Mosse: " + UserInput.mosse;
        punti.text = "Punti: " + UserInput.punti;
    }

    public void Resetta()
    {
        UserInput.mosse = 0;
        UserInput.punti = 0;
        AggiornaSprite[] carte = FindObjectsOfType<AggiornaSprite>(); //rimuovi tutte le carte
        foreach (AggiornaSprite carta in carte)
        {
            Destroy(carta.gameObject);
        }
        RipulisciTop();
        FindObjectOfType<Solitaire>().giocaCarte(); //distribuisci nuove carte
    }

    void RipulisciTop()
    {
        
        Selezionabile[] selezionabili = FindObjectsOfType<Selezionabile>();
        foreach(Selezionabile selezionabile in selezionabili)
        {
            if (selezionabile.CompareTag("Top"))
            {
                selezionabile.seme = null;
                selezionabile.valore = 0;
            }
        }
    }

    public void NuovaPartita()
    {
        pannelloVittoria.SetActive(false);
        Resetta();
    }
}
