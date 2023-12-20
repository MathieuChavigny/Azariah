using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// auteur du code et des commentaires : 
/// </summary>
public class MeilleurScore : MonoBehaviour
{
    int _premierScore = 0; //variable pour le premier score
    int _deuxiemeScore = 1; // variable pour le deuxieme score
    int _troisiemeScore = 2; // variable pour le troisieme score
    [SerializeField] SOSauvegarde _donnesSauvegarde; // variable pour le SOSauvegarde
    [SerializeField] CalculScore _calculScore; // variable pour la classe _calculScore
    [SerializeField] TMP_InputField[] _nomMeilleurJoueurTxt; // tableau de InputField des nom des meilleurs joueurs
    [SerializeField] TextMeshProUGUI[] _meilleurScoreTxt; //tableau de champ text des score des meilleurs joueurs
    [SerializeField] Button[] _ValiderNomButton; // tableau des bouton pour l'ecriture des nom des meilleur joueurs



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _donnesSauvegarde.LireMeilleurScoreSauvegarder(); // lit les donnees dans base de donnees
        ComparerScore(); // appel la fonction ComparerScore
        MiseAJourDonnees(); // appel la fonction MiseAJourDonnees
    }


    /// <summary>
    /// fonction qui compare les score avec grand-total
    /// classe le joueur dans le classement
    /// active les boutons
    /// </summary>
    void ComparerScore()
    {
        // compare si grand total est plus grand que le 1er meilleur Score
        if (_calculScore._grandTotal > _donnesSauvegarde._meilleurScore1)
        {
            int classement = 0;
            _donnesSauvegarde._meilleurScore3 = _donnesSauvegarde._meilleurScore2;
            _donnesSauvegarde._meilleurScore2 = _donnesSauvegarde._meilleurScore1;
            _donnesSauvegarde._meilleurScore1 = _calculScore._grandTotal;            
            _meilleurScoreTxt[_premierScore].text = _donnesSauvegarde._meilleurScore1.ToString();
            ActiverEnregistrement(classement);
            _donnesSauvegarde._nomMeilleurJoueur3 = _donnesSauvegarde._nomMeilleurJoueur2;
            _donnesSauvegarde._nomMeilleurJoueur2 = _donnesSauvegarde._nomMeilleurJoueur1;
        }
        // compare si grand total est plus grand que le 2e meilleur Score
        else if (_calculScore._grandTotal > _donnesSauvegarde._meilleurScore2)
        {
            int classement = 1;
            _donnesSauvegarde._meilleurScore3 = _donnesSauvegarde._meilleurScore2;
            _donnesSauvegarde._meilleurScore2 = _calculScore._grandTotal;
            _meilleurScoreTxt[_deuxiemeScore].text = _donnesSauvegarde._meilleurScore2.ToString();
            _donnesSauvegarde._nomMeilleurJoueur3 = _donnesSauvegarde._nomMeilleurJoueur2;
            ActiverEnregistrement(classement);
        }
        // compare si grand total est plus grand que le 3e meilleur Score
        else if (_calculScore._grandTotal > _donnesSauvegarde._meilleurScore3)
        {
            int classement = 2;
            _donnesSauvegarde._meilleurScore3 = _calculScore._grandTotal;
            _meilleurScoreTxt[_troisiemeScore].text = _donnesSauvegarde._meilleurScore3.ToString();
            ActiverEnregistrement(classement);
        }
        else
        {
            Debug.Log("Score trop bas");
        }
    }
    /// <summary>
    /// fonction qui active le input field et le bouton
    /// </summary>
    /// <param name="classement"></param>
    private void ActiverEnregistrement(int classement)
    {
        _nomMeilleurJoueurTxt[classement].interactable = true; 
        _ValiderNomButton[classement].interactable = true;
        _nomMeilleurJoueurTxt[classement].text = $"Saisir votre nom";
    }

    /// <summary>
    /// fonction qui remplace le nom du premier joueur dans la base de donnees
    /// </summary>
    public void RemplacerNomPremierJoueur()
    {
        int classement = 0;
        _donnesSauvegarde._nomMeilleurJoueur1 = _nomMeilleurJoueurTxt[classement].text;
        EnregistrementNoms(classement);
    }


    /// <summary>
    /// fonction qui remplace le nom du deuxieme joueur dans la base de donnees
    /// </summary>
    public void RemplacerNomDeuxiemeJoueur()
    {
        int classement = 1;
        _donnesSauvegarde._nomMeilleurJoueur2 = _nomMeilleurJoueurTxt[classement].text;
        EnregistrementNoms(classement);
    }
    /// <summary>
    /// fonction qui remplace le nom du troisieme joueur dans la base de donnees
    /// </summary>
    public void RemplacerNomTroisiemeJoueur()
    {
        int classement = 2;
        _donnesSauvegarde._nomMeilleurJoueur3 = _nomMeilleurJoueurTxt[classement].text;
        EnregistrementNoms(classement);
    }
    /// <summary>
    /// enregistre le nom du joueur et desactive le bouton
    /// </summary>
    /// <param name="classement"></param>
    private void EnregistrementNoms(int classement)
    {
        _nomMeilleurJoueurTxt[classement].interactable = false;
        _ValiderNomButton[classement].interactable = false;
        _donnesSauvegarde.EcrireMeilleurScoreSauvegarder();
        _donnesSauvegarde.LireMeilleurScoreSauvegarder();
        MiseAJourDonnees();
    }

    /// <summary>
    /// met a jour les donnees dans l'affichage 
    /// </summary>
    public void MiseAJourDonnees()
    {
        _meilleurScoreTxt[_premierScore].text = _donnesSauvegarde._meilleurScore1.ToString();
        _meilleurScoreTxt[_deuxiemeScore].text = _donnesSauvegarde._meilleurScore2.ToString();
        _meilleurScoreTxt[_troisiemeScore].text = _donnesSauvegarde._meilleurScore3.ToString();
        _nomMeilleurJoueurTxt[_premierScore].text = _donnesSauvegarde._nomMeilleurJoueur1;
        _nomMeilleurJoueurTxt[_deuxiemeScore].text = _donnesSauvegarde._nomMeilleurJoueur2;
        _nomMeilleurJoueurTxt[_troisiemeScore].text = _donnesSauvegarde._nomMeilleurJoueur3;

    }
}
