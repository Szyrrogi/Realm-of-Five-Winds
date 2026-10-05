using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plaga : Heros
{
    public override IEnumerator Fight()
    {
        foreach (Pole pole in gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if (pole.unit != null && pole.unit.GetComponent<Unit>().Enemy != Enemy)
            {
                Unit enemyUnit = pole.unit.GetComponent<Unit>();
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, (int)(AP / (Evolution ? 5 : 10)) * (FightManager.Turn))), TypeDamage.typeDamage.Magic));
            }
        }

        foreach (Pole pole in gameObject.GetComponent<DragObject>().pole.line.LineNext.pola)
        {
            if (pole.unit != null && pole.unit.GetComponent<Unit>().Enemy != Enemy)
            {
                Unit enemyUnit = pole.unit.GetComponent<Unit>();
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, (int)(AP / (Evolution ? 5 : 10)) * (FightManager.Turn))), TypeDamage.typeDamage.Magic));
            }
        }
        yield return null;
    }
    
    public override string DescriptionEdit()
    {
        if (!Evolution)
        {
            switch (PauseMenu.Language)
            {
                case 0: return "<b>Akcja: </b>Zadaje wszystkim wrogom w rzędzie obrażenia równe <color=#B803FF>" + (AP/10) + "</color> rosnące o <color=#B803FF>" + (AP/10) + "</color> w każdej turze" ;
                case 1: return "<b>Action: </b>Deals damage to all enemies in the row equal to <color=#B803FF>" + (AP/10) + "</color>, increasing by <color=#B803FF>" + (AP/10) + "</color> each turn";
                case 2: return "<b>Acción: </b>Inflige daño a todos los enemigos en la fila igual a <color=#B803FF>" + (AP/10) + "</color>, aumentando en <color=#B803FF>" + (AP/10) + "</color> cada turno";
                case 3: return "<b>Action: </b>Inflige des dégâts à tous les ennemis de la rangée égaux à <color=#B803FF>" + (AP/10) + "</color>, augmentant de <color=#B803FF>" + (AP/10) + "</color> à chaque tour";
                case 4: return "<b>Aktion: </b>Fügt allen Feinden in der Reihe Schaden in Höhe von <color=#B803FF>" + (AP/10) + "</color> zu, der sich in jeder Runde um <color=#B803FF>" + (AP/10) + "</color> erhöht";
            }
        }
        else
        {
            switch (PauseMenu.Language)
            {
                case 0: return "<b>Akcja: </b>Zadaje wszystkim wrogom w rzędzie obrażenia równe <color=#B803FF>" + (AP/5) + "</color> rosnące o <color=#B803FF>" + (AP/5) + "</color> w każdej turze" ;
                case 1: return "<b>Action: </b>Deals damage to all enemies in the row equal to <color=#B803FF>" + (AP/5) + "</color>, increasing by <color=#B803FF>" + (AP/5) + "</color> each turn";
                case 2: return "<b>Acción: </b>Inflige daño a todos los enemigos en la fila igual a <color=#B803FF>" + (AP/5) + "</color>, aumentando en <color=#B803FF>" + (AP/5) + "</color> cada turno";
                case 3: return "<b>Action: </b>Inflige des dégâts à tous les ennemis de la rangée égaux à <color=#B803FF>" + (AP/5) + "</color>, augmentant de <color=#B803FF>" + (AP/5) + "</color> à chaque tour";
                case 4: return "<b>Aktion: </b>Fügt allen Feinden in der Reihe Schaden in Höhe von <color=#B803FF>" + (AP/5) + "</color> zu, der sich in jeder Runde um <color=#B803FF>" + (AP/5) + "</color> erhöht";
            }
        }
        return null;
    }
}
