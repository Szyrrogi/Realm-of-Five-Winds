using UnityEngine;

public class DragObject : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    public static GameObject moveObject;
    public Pole pole;
    public Pole potencialPole;

    void Start()
    {
        mainCamera = Camera.main;  // Pobierz główną kamerę
    }


    public void OnMouseDown()
    {
        if(FightManager.IsFight == false)
        {
            isDragging = true;

            moveObject = this.gameObject;
            Vector3 mousePosition = GetMouseWorldPosition();
            offset = transform.position - mousePosition;
            DescriptionManager.opis.SetActive(false);
        }
    }

    void OnMouseUp()
    {
        Debug.Log("em");
        isDragging = false;
        moveObject = null;
        if(EventSystem.eventSystem.GetComponent<ShopManager>().dol.transform.position.x < gameObject.transform.position.x && 
            EventSystem.eventSystem.GetComponent<ShopManager>().gora.transform.position.x > gameObject.transform.position.x &&
            EventSystem.eventSystem.GetComponent<ShopManager>().dol.transform.position.y < gameObject.transform.position.y && 
            EventSystem.eventSystem.GetComponent<ShopManager>().gora.transform.position.y > gameObject.transform.position.y)
                gameObject.GetComponent<Unit>().Sell();
        if(potencialPole != null)
        {
            
            float distanceX = Mathf.Sqrt(Mathf.Pow(potencialPole.transform.position.x - transform.position.x, 2));
            float distanceY = Mathf.Sqrt(Mathf.Pow(potencialPole.transform.position.y - transform.position.y, 2));
            float distance = distanceX + distanceY;

            if(distance < 0.8f && (!GetComponent<Spell>() || potencialPole.line == null || (GetComponent<Spell>() && potencialPole.unit != null 
            && potencialPole.unit.GetComponent<Wizard>() && potencialPole.unit.GetComponent<Wizard>().spellCanLearn.Contains(GetComponent<Spell>().spellType))))
            {
                if(potencialPole.unit != null && pole != null)
                {
                    if(GetComponent<Spell>() && potencialPole.unit.GetComponent<Wizard>())
                    {
                        Debug.Log("NAUCZYL SIE");
                        potencialPole.unit.GetComponent<Wizard>().AddSpell(GetComponent<Spell>());
                        pole.unit = null;
                        pole = null;
                        potencialPole = null;
                        transform.position = new Vector3(90f,90f,90f);
                        //Destroy(this.gameObject);
                        return;
                    }
                    else
                    {
                        if(pole.unit.GetComponent<Unit>().Name == potencialPole.unit.GetComponent<Unit>().Name 
                        && potencialPole.unit.GetComponent<Unit>() && pole != potencialPole && !ShopManager.isLoockUpgrade && pole.unit.GetComponent<Heros>())   //ŁĄczenie jednostek!!!
                        {
                            if(pole.unit.GetComponent<Unit>().Health > potencialPole.unit.GetComponent<Unit>().Health)
                            {
                                bool helper = pole.unit.GetComponent<Heros>().Evolution;
                                pole.unit.GetComponent<Heros>().UpgradeHeros(potencialPole.unit.GetComponent<Unit>());
                                if(helper == pole.unit.GetComponent<Heros>().Evolution)
                                {
                                    pole.unit = null;
                                    pole.potencialUnit = null;
                                }
                            }
                            else
                            {
                                potencialPole.unit.GetComponent<Heros>().UpgradeHeros(pole.unit.GetComponent<Unit>());  //???
                                return;
                            }
                        }
                        else
                        {
                            pole.unit = potencialPole.unit;
                            pole.unit.GetComponent<DragObject>().pole = pole;
                            Vector3 noway = pole.transform.position;
                            noway.z -= 2f;
                            pole.unit.transform.position = noway;  
                        }   
                    }
                }
                else
                if(pole != null)
                {
                    pole.unit = null;
                }
                
                pole = potencialPole;
                pole.unit = this.gameObject;
            }
        }
        if(pole != null)
        {
            if (pole != null)
            {
                Vector3 newPosition = pole.transform.position;

                newPosition.z -= 5f;

                transform.position = newPosition;
            }

        }
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            mousePosition.z = -5f;
            transform.position = mousePosition + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;  
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
}
