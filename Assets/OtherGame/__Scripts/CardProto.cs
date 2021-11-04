using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardProto : MonoBehaviour
{
    public string suit;
    public int rank;
    public Color color = Color.black;
    public string colS = "Black";  // or "Red"

    public List<GameObject> decoGOs = new List<GameObject>();
    public List<GameObject> pipGOs = new List<GameObject>();

    public GameObject back;  // back of card;
    public CardDefinition def;  // from DeckXML.xml

    public bool faceUp
    {
        get
        {
            return (!back.activeSelf);
        }
        set
        {
            back.SetActive(!value);
        }
    }

    [System.Serializable]
    public class Decorator
    {
        public string type;
        public Vector3 loc;
        public bool flip = false;
        public float scale = 1f;
    }

    [System.Serializable]
    public class CardDefinition
    {
        public string face;
        public int rank;
        public List<Decorator> pips = new List<Decorator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
