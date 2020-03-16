using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInfoPlayer : MonoBehaviour {

    public string nickName = "DEFAULT_NAME";
    public int team = 0;    //nadawanie druzyny graczowi (potem gameController zrobi to samemu)
    public int race = 0;    //rasa: 0-ziemniak, 1-jednorozec, 2-pegaz
    public bool gender = false; //plec, jezeli true to meska

}
