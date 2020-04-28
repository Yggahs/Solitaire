using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UserInput : MonoBehaviour
{
    public static int punti = 0;
    public static int mosse = 0;
    static string s;
    public GameObject slot1;
    private Solitaire solitario;
    private float timer;
    private float doppioTapTimer = 0.3f;
    private int contaTap = 0;
    void Start()
    {
        solitario = FindObjectOfType<Solitaire>();
        slot1 = this.gameObject;
    }

    void Update()
    {
        //print(mosse);
        if(contaTap == 1)
        {
            timer += Time.deltaTime;
        }
        if(contaTap == 3)
        {
            timer = 0;
            contaTap = 1;
        }
        if(timer > doppioTapTimer)
        {
            timer = 0;
            contaTap = 0;
        }
        GetTap();
    }

    void GetTap()
    {
        if (Input.touchCount > 0) //comandi per telefono
        {
            Touch touch = Input.GetTouch(0);
            if (Input.touchCount == 1 && touch.phase == TouchPhase.Began)
            {
                contaTap++;
                Vector3 posizioneTouch = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -10));
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                if (hit)
                {
                    switch (hit.collider.tag)
                    {
                        case "Mazzo":
                            Mazzo();
                            //toccato mazzo
                            break;
                        case "Carta":
                            Carta(hit.collider.gameObject);
                            //toccata carta
                            break;
                        case "Top":
                            Top(hit.collider.gameObject);
                            //toccato top
                            break;
                        case "Bottom":
                            Bottom(hit.collider.gameObject);
                            //toccato bottom
                            break;
                    }
                }

            }
        }

        if (Input.GetMouseButtonDown(0)) //comandi per comuter
        {
            contaTap++;
            Vector3 posizioneTouch = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                switch (hit.collider.tag)
                {
                    case "Mazzo":
                        Mazzo();
                        //toccato mazzo
                        break;
                    case "Carta":
                        Carta(hit.collider.gameObject);
                        //toccata carta
                        break;
                    case "Top":
                        Top(hit.collider.gameObject);
                        //toccato top
                        break;
                    case "Bottom":
                        Bottom(hit.collider.gameObject);
                        //toccato bottom
                        break;
                }
            }
        }
    }

    void Mazzo() 
    {
       
        mosse++;
        //print("cliccato su mazzo");
        solitario.DistribuisciDaMazzo();
        slot1 = this.gameObject;
    }
    void Carta(GameObject selezionato) 
    {
        //print("Cliccato su carta");
        if (!selezionato.GetComponent<Selezionabile>().facciaInSu) //se la carta cliccata è a faccia in giù
        {
            if (!Bloccato(selezionato)) //se la carta non è bloccata
            {
                //gira la carta
                selezionato.GetComponent<Selezionabile>().facciaInSu = true;
                mosse++;
                punti += 5;
                slot1 = this.gameObject;
            }
        }
        else if (selezionato.GetComponent<Selezionabile>().inPila) //se la carta cliccata è tra le triplette
        {
            if (!Bloccato(selezionato)) //se non è bloccato
            {
                if (slot1 == selezionato)
                {
                    if (DoppioTap())
                    {
                        //prova a mettere in pila automaticamente
                        AutoPila(selezionato);
                        punti += 10;
                    }
                }
                else
                {
                    slot1 = selezionato;
                }
            }
        }
        else
        {
            //se la carta è girata
            //e non c'è nessun altra carta selezionata
            //seleziona la carta

            if (slot1 == this.gameObject) //non è null perchè passiamo questo gameobject
            {
                slot1 = selezionato;
            }
            else if (slot1 != selezionato) //se c'è già una carta selezionata e non è la stessa carta
            {
                //se la nuova carta forma una pila valida con la vecchia carta
                if (PilaValida(selezionato))
                {
                    Pila(selezionato);
                }
                else
                {
                    //seleziona la nuova carta
                    slot1 = selezionato;
                }
            }
            else if(slot1 == selezionato)
            {
                if (DoppioTap()) //se la stessa carta è tappata due volte
                {
                    AutoPila(selezionato); //cerca di mettere in pila automaticamente
              
                }
            }
        }
    }
    void Top(GameObject selezionato) 
    {
        //print("Cliccato su pila top");
        if (slot1.CompareTag("Carta")) 
        {
            if(slot1.GetComponent<Selezionabile>().valore == 1) //se la carta è un asso e lo spazio sotto è vuoto allora metti in pila
            {
                Pila(selezionato);
            }
        }
    }
    void Bottom(GameObject selezionato) 
    {
        //print("Cliccato su pila bottom");
        if (slot1.GetComponent<Selezionabile>().valore == 13) //se la carta è k e lo spazio sotto è vuoto allora metti in pila
        {
            Pila(selezionato);
        }
    }

    bool PilaValida(GameObject selezionato)
    {
        Selezionabile s1 = slot1.GetComponent<Selezionabile>();
        Selezionabile s2 = selezionato.GetComponent<Selezionabile>();
        //controlla se formano una pila valida

        if (!s2.inPila)
        {
            if (s2.top) //se nel top ordinare da asso a K 
            {
                if (s1.seme == s2.seme || (s1.valore == 1 && s2.seme == null))
                {
                    if (s1.valore == s2.valore + 1)
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else //se nel bottom mettere in pila con colori alternati
            {
                if (s1.valore == s2.valore - 1)
                {
                    bool carta1Rossa = true;
                    bool carta2Rossa = true;

                    if (s1.seme == "F" || s1.seme == "P")
                    {
                        carta1Rossa = false;
                    }

                    if (s2.seme == "F" || s2.seme == "P")
                    {
                        carta2Rossa = false;
                    }

                    if (carta1Rossa == carta2Rossa)
                    {
                        //print("Non valida");
                        return false;
                    }
                    else
                    {
                        //print("Pila valida");
                        //punti += 5;
                        return true;
                    }
                }
            }
        }
        return false;
    }
    void Pila(GameObject selezionato)
    {
        //se sopra un K o su una pila inferiore vuota posiziona le carte
        //altrimenti posiziona le carte con un yoffset negativo

        Selezionabile s1 = slot1.GetComponent<Selezionabile>();
        Selezionabile s2 = selezionato.GetComponent<Selezionabile>();
        float yOffset = 0.3f;

        if(s2.top || (!s2.top && s1.valore == 13))
        {
            yOffset = 0;
        }

        slot1.transform.position = new Vector3(selezionato.transform.position.x, selezionato.transform.position.y - yOffset, selezionato.transform.position.z-0.01f);
        slot1.transform.SetParent(selezionato.transform); 

        if (s1.inPila) //rimuovi carte dalle triplette per prevenire carte duplicate
        {
            solitario.tripletteDistribuite.Remove(slot1.name);
            if (s2.CompareTag("Top"))
            {
                punti += 10;
            }else if (s2.CompareTag("Bottom")) 
            {
                punti += 5;
            }
        }
        else if (s1.top && s2.top && s1.valore == 1) //permetti il movimento delle carte tra le pile top
        {
            solitario.topPos[s1.riga].GetComponent<Selezionabile>().valore = 0;
            solitario.topPos[s1.riga].GetComponent<Selezionabile>().seme = null;
        }
        else if (s1.top) //tieni conto del valore dei top quando una carta viene rimossa
        {
            solitario.topPos[s1.riga].GetComponent<Selezionabile>().valore = s1.valore - 1;
            punti -= 15;
        }
        else //rimuovi la carta dalla pila appropriata tra i bottom
        {
            solitario.bottoms[s1.riga].Remove(slot1.name);
        }

        s1.inPila = false; //non è possibile aggiungere carte alle triplette
        s1.riga = s2.riga;

        if (s2.top) //sposta una carta ai top ed assegnane il valore e il seme
        {
            solitario.topPos[s1.riga].GetComponent<Selezionabile>().valore = s1.valore;
            solitario.topPos[s1.riga].GetComponent<Selezionabile>().seme = s1.seme;
            s1.top = true;
            punti += 10;
        }
        else
        {
            s1.top = false;
        }
        mosse++;
        //dopo aver completato la mossa svuota slot1 dagli oggetti con tag carta
        slot1 = this.gameObject;

    }

    bool Bloccato(GameObject selezionato)
    {
        Selezionabile s2 = selezionato.GetComponent<Selezionabile>();
        if(s2.inPila == true)
        {
            if (s2.name == solitario.tripletteDistribuite.Last()) //se è l'ultima carta di una tripletta non è bloccata
            {
                return false;
            }
            else
            {
                //print(s2.name + " è bloccata da " + solitario.tripletteDistribuite.Last());
                return true;
            }
        }
        else
        {
            if(s2.name == solitario.bottoms[s2.riga].Last()) //controlla se è l'ultima carta
            {
                return false;
            }
            else
            {                
                return true;
            }
        }
    }

    bool DoppioTap()
    {


        if (timer < doppioTapTimer && (contaTap == 2 || Input.touchCount == 2))
        {
            //(print("Doppio Click");
            return true;
        }
        else
        {
            return false;
        }
    }

    void AutoPila(GameObject selezionato)
    {
        for (int i = 0; i < solitario.topPos.Length; i++){
            Selezionabile pila = solitario.topPos[i].GetComponent<Selezionabile>();
            if (selezionato.GetComponent<Selezionabile>().valore == 1) //se è un asso
            {
                if(solitario.topPos[i].GetComponent<Selezionabile>().valore == 0) //e la posizione su top è vuota
                {
                    slot1 = selezionato;
                    Pila(pila.gameObject); //metti l'asso sulla posizione in top
                    break;                 //nella prima posizione vuota trovata
                }
            }
            else
            {
                if((solitario.topPos[i].GetComponent<Selezionabile>().seme == slot1.GetComponent<Selezionabile>().seme) && (solitario.topPos[i].GetComponent<Selezionabile>().valore == slot1.GetComponent<Selezionabile>().valore - 1))
                {
                    //se è l'ultima carta, quindi non ha children
                    if (nessunChild(slot1))
                    {
                        slot1 = selezionato;
                        //trova un top che permette di mettere in pila automaticamente se esiste
                        string nomeUltimaCarta = pila.seme + pila.valore.ToString();
                        switch (pila.valore)
                        {
                            case 1:
                                nomeUltimaCarta = pila.seme + "A";
                                break;
                            case 11:
                                nomeUltimaCarta = pila.seme + "J";
                                break;
                            case 12:
                                nomeUltimaCarta = pila.seme + "Q";
                                break;
                            case 13:
                                nomeUltimaCarta = pila.seme + "K";
                                break;
                        }
                        GameObject ultimaCarta = GameObject.Find(nomeUltimaCarta);
                        Pila(ultimaCarta);
                        break;
                    }
                }
            }
        }
    }

    bool nessunChild(GameObject card)
    {
        int i = 0;
        foreach (Transform child in card.transform)
        {
            i++;
        }
        if (i == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
