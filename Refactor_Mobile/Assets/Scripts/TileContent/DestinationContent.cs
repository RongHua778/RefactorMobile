using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationContent : TrapContent
{
    public override int DieProtect { get => GameRes.DieProtect; }
    public override GameTileContentType ContentType => GameTileContentType.Destination;
    protected override void Awake()
    {
        IsReveal = true;
        Important = true;
    }

    public override void OnContentPass(Enemy enemy, GameTileContent content = null,int index=0)
    {

    }

}


