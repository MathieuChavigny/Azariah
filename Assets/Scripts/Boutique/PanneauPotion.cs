using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Classe qui controle un panneau d'objet d'une potion en vente a la boutique
/// Auteur des commentaires: Mathieu Chavigny
/// auuteur du code  Mathieu Chavigny
/// </summary>
public class PanneauPotion : MonoBehaviour
{
    [Header("LES DONNÉES")]
    
    [SerializeField] SOPotion _donneesPotion; //declaration du champs pour les donnees d'une potion
    public SOPotion donneesPotion => _donneesPotion; //accesseur aux donnees d'une potion
    [SerializeField] SOPerso _donneesPerso; //declaration du champs pour les donnees du personnage
    public SOPerso donneesPerso => _donneesPerso; //accesseur aux donnees du personnage

    [Header("LES CONTENEURS")]
    [SerializeField] TextMeshProUGUI _champNom; //declaration du champs pour le nom d'une potion
    [SerializeField] TextMeshProUGUI _champPrix; //declaration du champs pour le prix d'une potion
    [SerializeField] TextMeshProUGUI _champDescription; //declaration du champs pour la description d'une potion
    [SerializeField] Image _image; //declaration du champs pour l'image d'une potion
    [SerializeField] CanvasGroup _canvasGroup; //declaration du champs pour le groupe d'objet


    /// <summary>
    /// abonnement a l'evenement et appel a la methode pour la mise a jour des informations d'une potion
    /// </summary>
    void Start()
    {
        MettreAJourInfos(); //appel a la methode pour la mise a jour des informations d'une potion
        Boutique.instance.donneesPerso.evenementMisAJour.AddListener(MettreAJourInfos); //abonnement a l'evenement
    }

    /// <summary>
    /// Mise a jour des informations d'une potion de la boutique
    /// </summary>
    void MettreAJourInfos()
    {
        _champNom.text = donneesPotion.nom; //definit le nom d'une potion
        _champPrix.text = donneesPotion.prix+" Joyaux"; //definit le prix d'une potion
        _champDescription.text = donneesPotion.description; //definit la description d'une potion
        _image.sprite = donneesPotion.sprite; //definit l'image d'une potion
        GererDispo(); //appel de la methode pour verifier si le personnage peut encore acheter une potion
    }

    /// <summary>
    /// pour verifier si le personnage peut encore acheter une potion
    /// </summary>
    void GererDispo()
    {
        bool aAssezJoyau = Boutique.instance.donneesPerso.argentEnJoyaux >= _donneesPotion.prix; //variable qui dit est vrai le le personnage a plus de joyaux que le prix d'une potion
        
        if(aAssezJoyau) //condition qui dit que si un personnage a assez de joyaux le panneau reste actif et se desactive s'il en a pas assez
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
    public void AcheterPotion()
    {
        Boutique.instance.donneesPerso.AcheterPotion(_donneesPotion);  //achat d'une potion par le personnage en lui envoyant les donnees d'une potion
    }
}