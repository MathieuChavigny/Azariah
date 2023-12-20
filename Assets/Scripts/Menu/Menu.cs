using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GestSonore.instance.ChangerEtatLecturePiste(TypePiste.musiqueBase,false);
        GestSonore.instance.ChangerEtatLecturePiste(TypePiste.musiqueMenu,true);
    }

}