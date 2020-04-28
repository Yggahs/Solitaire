using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Solitaire : MonoBehaviour
{
    public Sprite[] fronteCarte;
    public GameObject cartaPrefab;
    public GameObject[] bottomPos;
    public GameObject[] topPos;
    public GameObject oggettoMazzo;

    public static string[] figure = new string[] { "C", "Q", "F", "P" };
    public static string[] numeri = new string[] { "A", "2","3","4","5","6","7","8","9","10","J","Q","K" };
    public List<string>[] bottoms;
    public List<string>[] tops;
    public List<string> tripletteDistribuite = new List<string>();
    public List<List<string>> mazzoinTriplette = new List<List<string>>();

    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();

    public List<string> mazzo;
    public List<string> scarti = new List<string>();
    private int posizioneMazzo;
    private int triplette;
    private int tripletteResto;

    public void Start()
    {
        bottoms = new List<string>[] { bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };

        giocaCarte();
    }

    public static List<string> generaMazzo() // genera 52 combinazioni di stringhe e aggiungile alla lista creata, poi randomizzane l'ordine
    {
        List<string> nuovoMazzo = new List<string>();
        foreach(string f in figure)
        {
            foreach(string n in numeri)
            {
                nuovoMazzo.Add(f + n);
            }
        }

        return nuovoMazzo;
    }

    public void giocaCarte()
    {
        foreach(List<string> list in bottoms)
        {
            list.Clear();
        }
        mazzo = generaMazzo();
        Mischia(mazzo);
        //foreach (string carta in mazzo) //test carte in mazzo
        //{
        //    Debug.Log(carta);
        //}
        solitarioOrdina();
        StartCoroutine(solitarioDistribuisci());
        OrdinaMazzoInTriplette();
    }

    void Mischia<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    IEnumerator solitarioDistribuisci()
    {
        for (int i = 0; i < 7; i++)
        {
            float yOffset = 0;
            float zOffset = 0.03f;
            foreach (string carta in bottoms[i])
            {
                yield return new WaitForSeconds(0.05f);
                GameObject nuovaCarta = Instantiate(cartaPrefab, new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), Quaternion.identity, bottomPos[i].transform);
                nuovaCarta.name = carta;
                nuovaCarta.GetComponent<Selezionabile>().riga = i;
                if(carta == bottoms[i][bottoms[i].Count - 1])
                {
                    nuovaCarta.GetComponent<Selezionabile>().facciaInSu = true;
                }
                yOffset += 0.3f;
                zOffset += 0.03f;
                scarti.Add(carta);
            }
        }
        foreach(string carta in scarti)
        {
            if (mazzo.Contains(carta))
            {
                mazzo.Remove(carta);
            }
        }
        scarti.Clear();
    }

    void solitarioOrdina()
    {
        for (int i = 0; i < 7; i++) {
            for (int j = i; j<7; j++)
            {
                bottoms[j].Add(mazzo.Last<string>());   
                mazzo.RemoveAt(mazzo.Count - 1);
            }
        } 
    }

    void OrdinaMazzoInTriplette()
    {
        triplette = mazzo.Count / 3;
        tripletteResto = mazzo.Count % 3;
        mazzoinTriplette.Clear();

        int modificatore = 0;
        for(int i = 0; i<triplette; i++)
        {
            List<string> nuoveTriplette = new List<string>();
            for(int j = 0; j< 3; j++)
            {
                nuoveTriplette.Add(mazzo[j + modificatore]);
            }
            mazzoinTriplette.Add(nuoveTriplette);
            modificatore += 3;
        }
        if(tripletteResto != 0)
        {
            List<string> nuovoResto = new List<string>();
            modificatore = 0;
            for(int k = 0; k< tripletteResto; k++)
            {
                nuovoResto.Add(mazzo[mazzo.Count - tripletteResto + modificatore]);
                modificatore++;
            }
            mazzoinTriplette.Add(nuovoResto);
            triplette++;
        }
        posizioneMazzo = 0;
    }

    public void DistribuisciDaMazzo()
    {
        //aggiungi carte rimaste agli scarti
        foreach(Transform child in oggettoMazzo.transform)
        {
            if (child.CompareTag("Carta"))
            {
                mazzo.Remove(child.name);
                scarti.Add(child.name);
                Destroy(child.gameObject);
            }
        }
        if(posizioneMazzo < triplette)
        {
            //pesca 3 carte
            tripletteDistribuite.Clear();
            float xOffset = 1.0f;
            float zOffset = -0.25f;
            foreach (string card in mazzoinTriplette[posizioneMazzo])
            {
                GameObject nuovaCimaMazzo = Instantiate(cartaPrefab, new Vector3(oggettoMazzo.transform.position.x + xOffset,oggettoMazzo.transform.position.y,oggettoMazzo.transform.position.z+zOffset), Quaternion.identity, oggettoMazzo.transform);
                xOffset += 0.2f;
                zOffset -= 0.2f;
                nuovaCimaMazzo.name = card;
                tripletteDistribuite.Add(card);
                nuovaCimaMazzo.GetComponent<Selezionabile>().facciaInSu = true;
                nuovaCimaMazzo.GetComponent<Selezionabile>().inPila = true;
            }
            posizioneMazzo++;
        }
        else
        {
            //riordina la cima del mazzo
            RiordinaCimaMazzo();
        }
    }

    void RiordinaCimaMazzo()
    {
        mazzo.Clear();
        foreach(string carta in scarti)
        {
            mazzo.Add(carta);
        }
        scarti.Clear();
        OrdinaMazzoInTriplette();

        UserInput.punti -= 100;
        if(UserInput.punti < 0)
            UserInput.punti = 0;
    }
}
