using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Classe qui controle le fonctionnement du chronometre
/// Auteurs : Alex Guilbault 
/// Auteur des commentaires: Alex Guilbault
/// </summary>
public class Chronometre : MonoBehaviour
{
    bool _estActiver = false;  // bool qui montre si le bonus est activé
    int _tempsAjouter = 100; // nb de temps gagner lors de la reception du bonus 
    int _tempsAvecBonus; //temps lorsque le temps restant et le temps ajouter est additionner

    [SerializeField] Sprite _chronometreActive; // Sprite du bonus activé
    SpriteRenderer _srchronometre;  // variable pour le SpriteRenderer du bonus de chronometre
    Perso _perso; // accède au script _perso
    [SerializeField] SOPerso _donneesPerso; // // accède au Scriptable object SOPerso

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _srchronometre = GetComponent<SpriteRenderer>();   // definit le spriteRenderer  du SpriteRenderer du chrono
        _donneesPerso.evenementActivationBonus.AddListener(ActiverBonus); // abonne le bonus à l'evenementActivationBonus

    }

    // Update is called once per frame
    void Update()
    {

        if (_estActiver == true)  //changement de sprite et d'alpha quand le bonus est actif
        {

            _srchronometre.sprite = _chronometreActive; //change le sprite pour celui qui est actif
            _srchronometre.color = new Color(1, 1, 1, 1);  //change l'alpha pour qu'il soit opaque
        }
        else
        {
            _srchronometre.color = new Color(1, 1, 1, 0.5f); //change l'alpha pour qu'il soit semi-transparent
        }
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    ///  interaction du bonus lorsque le joueur lui touche
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        _perso = other.GetComponent<Perso>();
        if (_estActiver == true && _perso != null) // quand le joueur y touche et que le bonus est actif
        {

            _tempsAvecBonus = _donneesPerso.tempsRestant + _tempsAjouter; // fait l'addition du temps recherche et le _tempsAjouter 
            _donneesPerso.tempsRestant = _tempsAvecBonus; //remplace le temps du perso par _tempsAvecBonus (update)
            Destroy(gameObject); //detruit le gameObject
        }
    }
    // fonction qui est le invoke de l'evenementActiverBonus 
    private void ActiverBonus()
    {
        _estActiver = true;
    }
}
