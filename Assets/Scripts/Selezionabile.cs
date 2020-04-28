using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selezionabile : MonoBehaviour
{
    public bool facciaInSu = false;
    public bool top = false;
    public bool inPila = false;
    public string seme;
    public int riga;
    public int valore;
    private string valoreStringa;

    void Start()
    {
        if (CompareTag("Carta"))
        {
            seme = transform.name[0].ToString();
            for(int i = 1; i< transform.name.Length; i++)
            {
                char c = transform.name[i];
                valoreStringa += c.ToString();
            }

            switch (valoreStringa)
            {
                case "A":
                    valore = 1;
                    break;
                case "2":
                    valore = 2;
                    break;
                case "3":
                    valore = 3;
                    break;
                case "4":
                    valore = 4;
                    break;
                case "5":
                    valore = 5;
                    break;
                case "6":
                    valore = 6;
                    break;
                case "7":
                    valore = 7;
                    break;
                case "8":
                    valore = 8;
                    break;
                case "9":
                    valore = 9;
                    break;
                case "10":
                    valore = 10;
                    break;
                case "J":
                    valore = 11;
                    break;
                case "Q":
                    valore = 12;
                    break;
                case "K":
                    valore = 13;
                    break;
            }
        }
    }
}
