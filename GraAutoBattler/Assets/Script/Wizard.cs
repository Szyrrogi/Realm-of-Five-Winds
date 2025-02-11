using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wizard : Shuffle
{
    public Image spellImage;
    public Spell spell;
    public List <Spell.SpellType> spellCanLearn;

    void Start ()
    {
        AddSpell(spell.Id);
    }

    public void AddSpell(Spell newSpell)
    {
        spellImage.sprite = newSpell.gameObject.GetComponent<SpriteRenderer>().sprite;
        Debug.Log(newSpell.Id);
        spell = newSpell;//EventSystem.eventSystem.GetComponent<CharacterManager>().Spells[newSpell.Id].GetComponent<Spell>();
        spell.unit = GetComponent<Unit>();
    }

    public void AddSpell(int newSpellId)
    {
        GameObject newSpellObject = Instantiate(EventSystem.eventSystem.GetComponent<CharacterManager>().Spells[newSpellId], new Vector3(70f,0,0), Quaternion.identity);
        AddSpell(newSpellObject.GetComponent<Spell>());
    }
}
