using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Classe qui controle un panneau d'objet d'une amelioration en vente a la boutique
/// Auteur des commentaires: Mathieu Chavigny
/// auuteur du code  Mathieu Chavigny
/// </summary>
public class PanneauObjet : MonoBehaviour
{
    [Header("LES DONNÉES")]
    [SerializeField] SOAmelioration _donneesObjet;  //declaration du champs pour les donnees des ameliorations
    public SOAmelioration donneesObjet => _donneesObjet; //accesseur aux donnees des ameliorations
    [SerializeField] SOPerso _donneesPerso; //declaration du champs pour les donnees du personnage
    public SOPerso donneesPerso => _donneesPerso; //accesseur aux donnees du personnage

    [Header("LES CONTENEURS")]
    [SerializeField] TextMeshProUGUI _champNom; //declaration du champs pour le nom d'une amelioration
    [SerializeField] TextMeshProUGUI _champPrix; //declaration du champs pour le prix d'une amelioration
    [SerializeField] TextMeshProUGUI _champDescription; //declaration du champs pour la description d'une amelioration
    [SerializeField] Image _image; //declaration du champs pour l'image d'une amelioration
    [SerializeField] CanvasGroup _canvasGroup; //declaration du champs pour le groupe d'objet


    /// <summary>
    /// abonnement a l'evenement et appel a la methode pour la mise a jour des informations d'une amelioration
    /// </summary>
    void Start()
    {
        MettreAJourInfos(); //appel a la methode pour la mise a jour des informations d'une amelioration
        Boutique.instance.donneesPerso.evenementMisAJour.AddListener(MettreAJourInfos); //abonnement a l'evenement
    }


    /// <summary>
    /// Mise a jour des informations d'une amelioration de la boutique
    /// </summary>
    void MettreAJourInfos()
    {
        _champNom.text = donneesObjet.nom; //definit le nom d'une amelioration
        _champPrix.text = donneesObjet.prix+" Ferrailles"; //definit le prix d'une amelioration
        _champDescription.text = donneesObjet.description; //definit la description d'une amelioration
        _image.sprite = donneesObjet.sprite; //definit l'image d'une amelioration
        GererDispo(); //appel de la methode pour verifier si le personnage peut encore acheter une amelioration
    }

    /// <summary>
    /// pour verifier si le personnage peut encore acheter une amelioration
    /// </summary>
    void GererDispo()
    {
        bool aAssezFeraille = Boutique.instance.donneesPerso.argentEnFeraille >= _donneesObjet.prix; //variable qui dit est vrai le le personnage a plus de feraille que le prix d'une amelioration
        
        if(aAssezFeraille)  //condition qui dit que si un personnage a assez de feraille le panneau reste actif et se desactive s'il en a pas assez
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha =1;
        }
        else
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha =0.5f;
        }
    }

    /// <summary>
    /// methode qui s'appel lorsque le joueur appuie sur le bouton acheter et appel la methode d'achat chez le personnage
    /// </summary>
    public void AcheterAmelioration()
    {
        Boutique.instance.donneesPerso.AcheterAmelioration(_donneesObjet); //achat d'une amelioration par le personnage en lui envoyant les donnees de l'amelioration
    }
}