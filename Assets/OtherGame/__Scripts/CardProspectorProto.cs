using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCardStateProto
{
    drawpile,
    tableau,
    target,
    discard
}
public class CardProspectorProto : MonoBehaviour
{
    [Header("Set Dynamically: CardProspector")]
    public eCardState state = eCardState.drawpile;
    public List<CardProspector> hiddenBy = new List<CardProspector>();
    public int layoutID;
    public SlotDef slotDef;

    override public void OnMouseUpAsButton()
    {
        ProspectorProto.S.CardClicked(this);
        base.OnMouseUpAsButton();
    }
}
