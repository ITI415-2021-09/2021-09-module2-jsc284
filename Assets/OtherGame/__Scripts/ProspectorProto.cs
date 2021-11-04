using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProspectorProto : MonoBehaviour
{
    static public ProspectorProto S;

    [Header("Set in Inspector")]
    public TextAsset deckXML;
    [Header("Set Dynamically")]
    public Deck deck;

    void Awake()
    {
        S = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        deck = GetComponent<Deck>();
        deck.InitDeck(deckXML.text);
        Deck.Shuffle(ref deck.cards);

        Card c;
        for (int cNum=0; cNum<deck.cards.Count; cNum++)
        {
            c = deck.cards[cNum];
            c.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
