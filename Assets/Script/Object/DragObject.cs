using UnityEngine;

public class DragObject : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    public static GameObject moveObject;
    public GameObject pole;
    public GameObject potencialPole;

    void Start()
    {
        mainCamera = Camera.main;  // Pobierz główną kamerę
    }

    void OnMouseDown()
    {
        isDragging = true;

        moveObject = this.gameObject;
        Vector3 mousePosition = GetMouseWorldPosition();
        offset = transform.position - mousePosition;
        DescriptionManager.opis.SetActive(false);
    }

    void OnMouseUp()
    {
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
            if(distance < 0.8f)
            {
                if(potencialPole.GetComponent<Pole>().unit != null && pole != null)
                {
                    if(pole.GetComponent<Pole>().unit.GetComponent<Unit>().Name == potencialPole.GetComponent<Pole>().unit.GetComponent<Unit>().Name 
                    && potencialPole.GetComponent<Pole>().unit.GetComponent<Unit>() && pole != potencialPole && !ShopManager.isLoockUpgrade)   //ŁĄczenie jednostek!!!
                    {
                        if(pole.GetComponent<Pole>().unit.GetComponent<Unit>().Health > potencialPole.GetComponent<Pole>().unit.GetComponent<Unit>().Health)
                        {
                            bool helper = pole.GetComponent<Pole>().unit.GetComponent<Heros>().Evolution;
                            pole.GetComponent<Pole>().unit.GetComponent<Heros>().UpgradeHeros(potencialPole.GetComponent<Pole>().unit.GetComponent<Unit>());
                            if(helper == pole.GetComponent<Pole>().unit.GetComponent<Heros>().Evolution)
                            {
                                pole.GetComponent<Pole>().unit = null;
                                pole.GetComponent<Pole>().potencialUnit = null;
                            }
                            Debug.Log("potencialPole.name");
                            //potencialPole.GetComponent<Pole>().unit = pole.GetComponent<Pole>().unit.gameObject;
                        }
                        else
                        {
                            potencialPole.GetComponent<Pole>().unit.GetComponent<Heros>().UpgradeHeros(pole.GetComponent<Pole>().unit.GetComponent<Unit>());
                            return;
                            // Destroy(gameObject);
                            // Debug.Log(pole.name + " " + gameObject.name);
                            // potencialPole.GetComponent<Pole>().unit = pole.GetComponent<Pole>().unit.gameObject;
                        }
                    }
                    else
                    {
                    //GameObject temp = potencialPole.GetComponent<Pole>().unit;
                        pole.GetComponent<Pole>().unit = potencialPole.GetComponent<Pole>().unit;
                        pole.GetComponent<Pole>().unit.GetComponent<DragObject>().pole = pole;
                        Vector3 noway = pole.transform.position;
                        noway.z -= 2f;
                        pole.GetComponent<Pole>().unit.transform.position = noway;  
                    }   
                }
                else
                if(pole != null)
                {
                    pole.GetComponent<Pole>().unit = null;
                }
                pole = potencialPole;
                pole.GetComponent<Pole>().unit = this.gameObject;
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
