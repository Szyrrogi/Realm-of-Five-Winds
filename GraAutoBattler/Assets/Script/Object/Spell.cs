using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Unit
{
    public enum SpellType
    {
        Summon,
        Fire,
        Magic
    }

    public SpellType spellType;
}
