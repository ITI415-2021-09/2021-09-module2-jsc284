using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardProto : MonoBehaviour
{
}

public class DecoratorProto
{
    public string type;
    public Vector3 loc;
    public bool flip = false;
    public float scale = 1f;
}
public class CardDefinitionProto
{
    public string face;
    public int rank;
    public List<DecoratorProto> pips = new List<DecoratorProto>();
}