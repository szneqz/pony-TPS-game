using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairSpawn : MonoBehaviour
{

    public Texture2D crosshairImage;    //wybrana textura crosshaira
    public float wepRange = 1000.0f;
    public float wepLenght = 0.0f;      //dlugosc broni (przy doublebarell)

    public bool forwardScriptRelated = false;   //czy forward idzie ze skryptu
    public Vector3 datForward = Vector3.zero;

    private Vector3 screenPos;
    private Vector3 screenPos2;         //zmienna drugiego celownika pomocniczego
    private Player2 playerScript;
    private ContrMovem keyScript;

    private float distance = 0.0f;
    private int size = 0;

    private LayerMask layer = ~((1 << 9));   //celownik uznaje wszystko (oprocz pociskow) za przeszkode i wedlug niej sie kieruje

    private void Start()
    {
        playerScript = transform.parent.GetComponent<Player2>();
        keyScript = transform.parent.GetComponent<ContrMovem>();
        size = (int)(0.08f * Screen.height);
    }

    void OnGUI()
    {
        if (!keyScript.ifBot && keyScript.isLocalPlayer)
        {
            float xMin = screenPos.x - (size * distance / 2);   //wartosc x na ekranie
            float yMin = -screenPos.y - (size * distance / 2) + Screen.height; //wartosc y na ekranie
            float xMin2 = screenPos2.x - (size / 2);   //wartosc x na ekranie
            float yMin2 = -screenPos2.y - (size / 2) + Screen.height;
            if (playerScript.Dead == false)   //jezeli postac zyje to celownik rysowany jest jak dawniej w przeciwnym wypadku nie jest rysowany
            {
                //     GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width * distance, crosshairImage.height * distance), crosshairImage); //rysowanie
                //      GUI.DrawTexture(new Rect(xMin2, yMin2, crosshairImage.width, crosshairImage.height), crosshairImage);
                GUI.DrawTexture(new Rect(xMin, yMin, size * distance, size * distance), crosshairImage); //rysowanie
                GUI.DrawTexture(new Rect(xMin2, yMin2, size, size), crosshairImage);
            }
            GUI.Label(new Rect(0, 0, 100, 100), (1.0f / Time.smoothDeltaTime).ToString());  //wyswietla FPSy
        }
    }

    void LateUpdate()
    {
        if (!keyScript.ifBot && keyScript.isLocalPlayer)
        {
            Vector3 localForward;
            if (forwardScriptRelated)
                localForward = datForward;
            else
                localForward = transform.forward;

            RaycastHit h;
            Vector3 targetPos;
            if (Physics.Raycast(transform.position + localForward * wepLenght, localForward, out h, wepRange, layer))    //rysuje RayCast na odleglosc strzalu i sprawdzam gdzie trafia
            {
                targetPos = h.point;    //jezeli trafia sciagam info
            }
            else
            {
                targetPos = transform.position + localForward.normalized * wepRange;   //jezeli nie trafia to udaje ze target jest na odleglosci strzalu
            }
            screenPos = Camera.main.WorldToScreenPoint(targetPos);  //tu zmienia wartosc znalezionej pozycji strzalu na pozycje ekranu
            screenPos2 = Camera.main.WorldToScreenPoint(transform.position + localForward);    //jedna jednostka od broni

            distance = (targetPos - transform.position).magnitude;  //obliczanie odleglosci dalekiego celownika
            distance = Mathf.Clamp(distance, 0.1f, 100.0f) / 25.0f;
            distance = 1.0f / distance;
            distance = Mathf.Clamp(distance, 0.3f, 1.0f);           //zmienna wielkosci celownika na duzej odleglosci
        }
    }
}
