using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
// =========================================
// Auteur : Alex Guilbault
// Auteur des commentaires: Alex Guilbault
// ==========================================
/// Classe qui s'occupe du fonctionnement des leviers de l'énigme
/// </summary>
public class Levier : MonoBehaviour
{
    [SerializeField] Sprite _LevierActivé; // champs pour les sprites quand le bonus est activé  #tpSynthese (Alex Guilbault)
    SpriteRenderer _srlevier; // variable pour le SpriteRenderer de l'activateur  #tpSynthese (Alex Guilbault)
    Levier _levier; // accède au script _activateur  #tpSynthese (Alex Guilbault)

    Enigme _enigme; // accède au script _enigme  #tpSynthese (Alex Guilbault)
    Perso _perso; // accède au script _perso  #tpSynthese (Alex Guilbault)

    bool _BonLevier = false; // variable pour le bon levier  #tpSynthese (Alex Guilbault)
    bool _MauvaisLevier = false; // variable pour le mauvais levier  #tpSynthese (Alex Guilbault)

    [SerializeField] SOPerso _donneesPerso; // variable pour le ScriptableObject du perso  #tpSynthese (Alex Guilbault)
    bool _peuxActiverLevier = false; // variable pour savoir si le perso peut activer le levier  #tpSynthese (Alex Guilbault)
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        _srlevier = GetComponent<SpriteRenderer>();  // definie le SpriteRenderer  #tpSynthese (Alex Guilbault)
        _enigme = GetComponentInParent<Enigme>(); // definie le Script  #tpSynthese (Alex Guilbault)
        _levier = GetComponent<Levier>(); //definie le Script  #tpSynthese (Alex Guilbault)
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _peuxActiverLevier = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            _peuxActiverLevier = false;
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// invoke l'evenement des bonus de l'activatiion
    /// Desactive le Script
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>

    void OnTriggerStay2D(Collider2D other)
    {
        _perso = other.GetComponent<Perso>(); // definie le Script Perso  #tpSynthese (Alex Guilbault)
        if (_perso != null) // si le perso est dans la hitbox et que le joueur appuie sur E  #tpSynthese (Alex Guilbault)
        {


            if (_peuxActiverLevier)
            {
                _srlevier.sprite = _LevierActivé;
                _levier.enabled = false;
                if (_BonLevier == true)
                {
                    _enigme.ActiverPorte();
                }
                else if (_MauvaisLevier == true)
                {

                }
            }


        }
    }


    /// <summary>
    /// 
    /// </summary>
    public void LevierPorte()
    {
        _BonLevier = true;
    }
    /// <summary>
    /// 
    /// </summary>
    public void LevierMaudit()
    {
        _MauvaisLevier = true;
        _perso.donneesPerso.vieJoueur -=1;
    }
}

