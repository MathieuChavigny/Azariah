using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Classe qui gère l'affichage des informations sur la planche dans l'interface de jeu
/// auteur du code et des commentaires : Mathieu Chavigny
/// </summary>
public class Planche : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; //reference au scriptable object qui contient les donnees du personnage #tp4
    [SerializeField] TextMeshProUGUI _txtJoyaux; //reference au text qui affiche le nombre de joyaux #tp4
    [SerializeField] TextMeshProUGUI _txtFerailles; //reference au text qui affiche le nombre de ferailles
    [SerializeField] TextMeshProUGUI _txtTemps; //reference au text qui affiche le temps restant

    [SerializeField] TextMeshProUGUI _txtNiveau; //reference au text qui affiche le niveau actuel

    [SerializeField] Sprite[] _SpriteBulleRage; //reference au sprite renderer du bouclier Alex Guilbault #tpsynthese
    [SerializeField] Image _srBulleRage; //reference au sprite renderer du coeur Alex Guilbault #tpsynthese
    int _imageRage;  // determine l'image de sprite utilise
    float _deltaRage; // delta pour la rage

    [SerializeField] Image _srArme; //reference au sprite renderer du coeur Alex Guilbault #tpsynthese
    [SerializeField] Image[] _srCoeur; //reference au sprite renderer du coeur Alex Guilbault #tpsynthese
    [SerializeField] Sprite _coeur; //reference au sprite renderer de l'epee Alex Guilbault #tpsynthese

    [SerializeField] Sprite _coeurvide; //reference au sprite renderer du bouclier Alex Guilbault #tpsynthese
    [SerializeField] Sprite _armeEpee; //reference au sprite renderer du bouclier Alex Guilbault #tpsynthese
    [SerializeField] Sprite _armeArc; //reference au sprite renderer du bouclier Alex Guilbault #tpsynthese

    int _nbCoeurMax = 3; //nombre de coeur dans la barre de vie Alex Guilbault #tpsynthese
    int _nbRageMax = 100; //nombre de coeur dans la barre de vie Alex Guilbault #tpsynthese

    /// <summary>
    /// affiche les informations essentiel du joueur sur la planche et les met à jour #tp4
    /// </summary>
    public void ChangerPlanche()
    {
        _txtJoyaux.text = _donneesPerso.argentEnJoyaux.ToString();
        _txtFerailles.text = _donneesPerso.argentEnFeraille.ToString();
        _txtTemps.text = _donneesPerso.tempsRestant.ToString();
        _txtNiveau.text = "N" + _donneesPerso.niveau.ToString();
        int CoeurVide = _nbCoeurMax - _donneesPerso.vieJoueur; // code qui vérifie la vie du joueur   Alex Guilbault #tpsynthese
        for (int i = 0; i < CoeurVide; i++) // boucle qui met des coueur vide au besoin selon  la vie du joueur  Alex Guilbault #tpsynthese
        {
            
            _srCoeur[i].sprite = _coeurvide;
        }
        if (CoeurVide == 0)
        {
            for (int i = 0; i < _nbCoeurMax; i++) // boucle qui met des coeur pleins selon  la vie du joueur  Mathieu Chavigny #tpSynthese
            {

                _srCoeur[i].sprite = _coeur;
            }
            
        }

        
        if (_donneesPerso._armeActifArc == true) //condition si l'arc est actif  #tpSynthese (Alex Guilbault)
        {
            _srArme.sprite = _armeArc; // affiche l'arc  #tpSynthese (Alex Guilbault)
        }
        else // sinon  #tpSynthese (Alex Guilbault)
        {
            _srArme.sprite = _armeEpee; // affiche l'epee  #tpSynthese (Alex Guilbault)
        }

        _deltaRage = (float)_donneesPerso.rage / _nbRageMax; // calcul le delta pour la rage  #tpSynthese (Alex Guilbault)
        if (_donneesPerso.rage > 100) // si la rage depasse 100  #tpSynthese (Alex Guilbault)
        {
            _donneesPerso.rage = 100; //remet la rage a 100 #tpSynthese (Alex Guilbault)
        }
        
        

        switch (_deltaRage) // switch pour determiner le sprite de la bulle de rage dans planche #tpSynthese (Alex Guilbault)
        {
            case 1f:
                _imageRage = 3;
                _srBulleRage.sprite = _SpriteBulleRage[_imageRage]; // change les sprite de la bulle #tpSynthese (Alex Guilbault)
                break;

            case > 0.5f:
                _imageRage = 2;
                _srBulleRage.sprite = _SpriteBulleRage[_imageRage]; // change les sprite de la bulle #tpSynthese (Alex Guilbault)
                break;

            case > 0.25f:
                _imageRage = 1;
                _srBulleRage.sprite = _SpriteBulleRage[_imageRage]; // change les sprite de la bulle #tpSynthese (Alex Guilbault)
                break;
            default:
                _imageRage = 0;
                _srBulleRage.sprite = _SpriteBulleRage[_imageRage]; // change les sprite de la bulle #tpSynthese (Alex Guilbault)
                break;

        }

    

    }
}
