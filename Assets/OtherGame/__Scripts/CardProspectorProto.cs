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
public class CardProspectorProto : Card
{
    [Header("Set Dynamically: CardProspectorProto")]
    public eCardState state = eCardState.drawpile;
    public List<CardProspectorProto> hiddenBy = new List<CardProspectorProto>();
    public int layoutID;
    public SlotDefProto slotDef;

    override public void OnMouseUpAsButton()
    {
        ProspectorProto.S.CardClicked(this);
        base.OnMouseUpAsButton();
    }
}
