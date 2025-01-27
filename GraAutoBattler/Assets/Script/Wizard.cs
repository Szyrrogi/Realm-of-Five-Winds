using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wizard : Shuffle
{
    public Image spellImage;
    public GameObject spell;

    public void AddSpell(Spell newSpell)
    {
        spellImage.sprite = newSpell.gameObject.GetComponent<SpriteRenderer>().sprite;
        Debug.Log(newSpell.Id);
        spell = EventSystem.eventSystem.GetComponent<CharacterManager>().Spells[newSpell.Id];
        //spell = newSpell;
    }
}
