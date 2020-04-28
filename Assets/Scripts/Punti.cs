using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punti : MonoBehaviour
{
    public Selezionabile[] pileTop;
    public GameObject pannelloVittoria;

    void Update()
    {
        if (checkVittoria())
        {
            Vittoria();
        }
    }

    public bool checkVittoria()
    {
        int i = 0;
        foreach(Selezionabile pila in pileTop)
        {
            i += pila.valore;
        }
        if (i >= 52)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Vittoria()
    {
        pannelloVittoria.SetActive(true);
    }
}
