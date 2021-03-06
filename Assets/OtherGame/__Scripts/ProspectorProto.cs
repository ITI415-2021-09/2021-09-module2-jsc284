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
    public TextAsset layoutXML;
    public float xOffset = 3;
    public float yOffset = -2.5f;
    public Vector3 layoutCenter;

    [Header("Set Dynamically")]
    public DeckProto deck;
    public LayoutProto layout;
    public List<CardProspectorProto> deckPile;
    public List<CardProspectorProto> drawPile;
    public Transform layoutAnchor;
    public CardProspectorProto target;
    public List<CardProspectorProto> tableau;
    public List<CardProspectorProto> discardPile;

    void Awake()
    {
        S = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        deck = GetComponent<DeckProto>();
        deck.InitDeck(deckXML.text);
        DeckProto.Shuffle(ref deck.cards);

        /* Card c;
        for (int cNum=0; cNum<deck.cards.Count; cNum++)
        {
            c = deck.cards[cNum];
            c.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
        } */
        layout = GetComponent<LayoutProto>();
        layout.ReadLayout(layoutXML.text);
        drawPile = ConvertListCardsToListCardProspectors(deck.cards);
        LayoutGame();
    }

    List<CardProspectorProto> ConvertListCardsToListCardProspectors(List<Card> lCD)
    {
        List<CardProspectorProto> lCP = new List<CardProspectorProto>();
        CardProspectorProto tCP;
        foreach (Card tCD in lCD)
        {
            tCP = tCD as CardProspectorProto;
            lCP.Add(tCP);
        }
        return (lCP);
    }

    public CardProspectorProto Draw()
    {
        CardProspectorProto cd = drawPile[0];
        drawPile.RemoveAt(0);
        return (cd);
    }

    void LayoutGame()
    {
        if (layoutAnchor == null)
        {
            GameObject tGO = new GameObject("_LayoutAnchor");
            layoutAnchor = tGO.transform;
            layoutAnchor.transform.position = layoutCenter;
        }

        CardProspectorProto cp;
        foreach (SlotDefProto tSD in layout.slotDefs)
        {
            cp = Draw();
            cp.faceUp = tSD.faceUp;
            cp.transform.parent = layoutAnchor;
            cp.transform.localPosition = new Vector3(
                layout.multiplier.x * tSD.x,
                layout.multiplier.y * tSD.y,
                -tSD.layerID);
            cp.layoutID = tSD.id;
            cp.slotDef = tSD;
            cp.state = eCardState.tableau;
            cp.SetSortingLayerName(tSD.layerName);

            tableau.Add(cp);
        }

        foreach (CardProspectorProto tCP in tableau)
        {
            foreach (int hid in tCP.slotDef.hiddenBy)
            {
                cp = FindCardByLayoutID(hid);
                tCP.hiddenBy.Add(cp);
            }
        }

        MoveToTarget(Draw());
        UpdateDrawPile();
    }

    CardProspectorProto FindCardByLayoutID(int layoutID)
    {
        foreach (CardProspectorProto tCP in tableau)
        {
            if (tCP.layoutID == layoutID)
            {
                return (tCP);
            }
        }
        return (null);
    }

    void SetTableauFaces()
    {
        foreach (CardProspectorProto cd in tableau)
        {
            bool faceUp = true;
            foreach (CardProspectorProto cover in cd.hiddenBy)
            {
                if (cover.state == eCardState.tableau)
                {
                    faceUp = false;
                }
            }
            cd.faceUp = faceUp;
        }
    }

    void MoveToDiscard(CardProspectorProto cd)
    {
        cd.state = eCardState.discard;
        discardPile.Add(cd);
        cd.transform.parent = layoutAnchor;
        cd.transform.localPosition = new Vector3(
            layout.multiplier.x * layout.discardPile.x,
            layout.multiplier.y * layout.discardPile.y,
            -layout.discardPile.layerID + 0.5f);
        cd.faceUp = true;
        cd.SetSortingLayerName(layout.discardPile.layerName);
        cd.SetSortOrder(-100 + discardPile.Count);
    }

    void MoveToTarget(CardProspectorProto cd)
    {
        if (target != null) MoveToDiscard(target);
        target = cd;
        cd.state = eCardState.target;
        cd.transform.parent = layoutAnchor;
        cd.transform.localPosition = new Vector3(
            layout.multiplier.x * layout.discardPile.x,
            layout.multiplier.y * layout.discardPile.y,
            -layout.discardPile.layerID);
        cd.faceUp = true;
        cd.SetSortingLayerName(layout.discardPile.layerName);
        cd.SetSortOrder(0);
    }

    void UpdateDrawPile()
    {
        CardProspectorProto cd;
        for (int i = 0; i < drawPile.Count; i++)
        {
            cd = drawPile[i];
            cd.transform.parent = layoutAnchor;

            Vector2 dpStagger = layout.drawPile.stagger;
            cd.transform.localPosition = new Vector3(
                layout.multiplier.x * (layout.drawPile.x + i * dpStagger.x),
                layout.multiplier.y * (layout.drawPile.y + i * dpStagger.y),
                -layout.drawPile.layerID + 0.1f * i);

            cd.faceUp = false;
            cd.state = eCardState.drawpile;
            cd.SetSortingLayerName(layout.drawPile.layerName);
            cd.SetSortOrder(-10 * i);
        }
    }

    public void CardClicked(CardProspectorProto cd)
    {
        switch (cd.state)
        {
            case eCardState.target:
                break;

            case eCardState.drawpile:
                MoveToDiscard(target);
                MoveToTarget(Draw());
                UpdateDrawPile();
                break;

            case eCardState.tableau:
                bool validMatch = true;
                if (!cd.faceUp)
                {
                    validMatch = false;
                }
                if (!EqualsThirteen(cd, target))
                {
                    validMatch = false;
                }
                if (!validMatch) return;

                tableau.Remove(cd);
                MoveToTarget(cd);
                SetTableauFaces();
                break;
        }
        CheckForGameOver();
    }

    void CheckForGameOver()
    {
        if (tableau.Count == 0)
        {
            GameOver(true);
            return;
        }

        if (drawPile.Count > 0)
        {
            return;
        }

        foreach (CardProspectorProto cd in tableau)
        {
            if (EqualsThirteen(cd, target)) {
                return;
        }
        }
            GameOver(false);
}

    void GameOver(bool won)
{
    if (won)
    {
        print("Game Over. You won! :)");
    } else
    {
        print("Game Over. You lost. :(");
    }
    SceneManager.LoadScene("GameScene");
}
        public bool EqualsThirteen(CardProspectorProto c0, CardProspectorProto c1)
        {
            if (!c0.faceUp || !c1.faceUp) return (false);
        
        if (Mathf.Abs(c0.rank) == 13)
        {
            return (true);
        }

        if (Mathf.Abs(c0.rank + c1.rank) == 13 && c1.rank != 13) {
            return (true);
        }
            return (false);
        }

}