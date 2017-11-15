using System;
using System.Collections.Generic;



public class DealCardArgs
{
    public CharacterType cType;
    public Card card;
    public bool selected;

    public DealCardArgs(CharacterType cType,Card card,bool selected)
    {
        this.cType = cType;
        this.card = card;
        this.selected = selected;
    }
}

