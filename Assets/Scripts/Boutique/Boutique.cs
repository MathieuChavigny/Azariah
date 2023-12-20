using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// Classe qui s'occupe des activites de la boutique
/// Auteur des commentaires: Mathieu Chavigny
/// auuteur du code  Mathieu Chavigny
/// </summary>

public class Boutique : MonoBehaviour
{  
    [Header("Donnees")]
    [SerializeField] SOPerso _donneesPerso; //declaration du champs pour les donnees du personnage
    public SOPerso donneesPerso => _donneesPerso; //accesseur aux donnees du personnage
    [Header("Champs textes")]
    [SerializeField] TextMeshProUGUI _champNiveau; //declaration du champs pour le champ de texte niveau
    [SerializeField] TextMeshProUGUI _champJoyau; //declaration du champs pour le champ de texte joyaux
    [SerializeField] TextMeshProUGUI _champFeraille; //declaration du champs pour le champ de texte feraille
    [Header("Singleton")]
    static Boutique _instance; //mecanique de singleton
    static public Boutique instance => _instance; //mecanique de singleton
    [Header("Son")]
    [SerializeField] AudioClip _sonClochette; //declaration du champs pour son de clochette #tpsynthese (Alex Guilbault)
    
    
    /// <summary>
    /// mise en place de la mecanique de singleton et abonnement a unity event
    /// </summary>
    void Awake()
    {
        GestSonore.instance.JouerEffetSonore(_sonClochette); //joue la musique de la boutique #tpSynthese
        GestSonore.instance.ChangerEtatLecturePiste(TypePiste.musiqueMenu, true); //joue la musique de la boutique
        GestSonore.instance.ChangerEtatLecturePiste(TypePiste.musiqueBase, false); //joue la musique de la boutique
        GestSonore.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenB, false); //joue la musique de la boutique
        GestSonore.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenA, false); //joue la musique de la boutique
        if (_instance != null) { Destroy(gameObject); return; } //mecanique de singleton
        _instance = this; //mecanique de singleton
        _donneesPerso.evenementMisAJour.AddListener(MettreAJourInfos); //abonnement a unity event
        MettreAJourInfos(); //appel de la methode pour mettre a jour les infos du joueurs

    }
    /// <summary>
    /// Lorsque l'application est quittee on remet les valeur du personnage aux valeurs de base
    /// </summary>
    void OnApplicationQuit()
    {
        _donneesPerso.Initialiser(); 
    }
    /// <summary>
    /// met a jour les informations du personnage et affiche les infos dans l'interface
    /// </summary>
    private void MettreAJourInfos()
    {
        _champJoyau.text = _donneesPerso.argentEnJoyaux + " Joyaux"; //Met a jour le nombre de joyaux
        _champFeraille.text = _donneesPerso.argentEnFeraille + " Ferrailles"; //Met a jour le nombre de feraille
        _champNiveau.text = "Niveau "+_donneesPerso.niveau;  //Met a jour le niveau
    }
    /// <summary>
    /// lorsque la boutique est detruite on enleve les Events
    /// </summary>
    void OnDestroy()
    {
        _donneesPerso.evenementMisAJour.RemoveAllListeners(); 
    }  
}