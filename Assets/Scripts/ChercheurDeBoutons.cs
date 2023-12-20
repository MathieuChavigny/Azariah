using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script à appliquer sur un GameObject au choix, 
/// pour découvrir les numéros des boutons d'un' gamepad
/// 
/// Auteur du script: Jonathan Tremblay*
/// 
/// *Adaptation de la réponse de «asuravada» sur:
/// https://answers.unity.com/questions/411950/index.html
/// 
/// Auteur des commentaires: Jonathan Tremblay
/// </summary>
public class ChercheurDeBoutons : MonoBehaviour
{
    [SerializeField] private int _nbGamepadMax = 8; //le nombre de gamepads à tester
    private int _btnMax = 12; //le nombre de boutons à tester

    // Update is called once per frame
    void Update()
    {
        for(int j=1; j<_nbGamepadMax; j++) //boucle des gamepads
        {
            for(int b=0; b<_btnMax; b++) //boucle des boutons
            {
                string nomBtn = "Joystick"+j+"Button"+b; //construction de la chaine d'un bouton possible
                //récuperation du code de la touche potentielle:
                KeyCode codeBtn = (KeyCode)System.Enum.Parse(typeof(KeyCode), nomBtn); 
                if(Input.GetKey(codeBtn)) //si cette touche est enfoncée
                { 
                    Debug.Log("Bouton appuyé: "+ "Joystick"+j+"Button"+b); 
                } 
            }
        }

        //mécanique similaire pour tester les touches du clavier:
        if(Input.anyKeyDown) Debug.Log("Touche enfoncée: "+Input.inputString);
    }
}