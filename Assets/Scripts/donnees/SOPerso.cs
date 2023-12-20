using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


/// <summary>
/// Classe qui controle les donnees du personnage
/// Auteur des commentaires: Mathieu Chavigny et Alex Guilbault
/// auuteur du code  Mathieu Chavigny et Alex Guilbault
/// </summary>
/// 
[CreateAssetMenu(fileName = "Perso", menuName = "Perso")]
public class SOPerso : ScriptableObject
{

    [Header("Liste")]
    List<SOObjetMap> _lesObjets = new List<SOObjetMap>(); //creation d'une liste pour garder en inventaire les objet qui influence la map

    public List<SOObjetMap> lesObjets { get => _lesObjets; set => _lesObjets = value; } //accesseur pour permettre l'acces par l'exterieur du script
    List<SOAmelioration> _lesAmeliorations = new List<SOAmelioration>(); //creation d'une liste pour garder en inventaire les ameliorations du personnage
    public List<SOAmelioration> lesAmeliorations { get => _lesAmeliorations; set => _lesAmeliorations = value; } //accesseur pour permettre l'acces par l'exterieur du script
    List<SOPotion> _lesPotions = new List<SOPotion>(); //creation d'une liste pour garder en inventaire les potions du personnage
    public List<SOPotion> lesPotions { get => _lesPotions; set => _lesPotions = value; } //accesseur pour permettre l'acces par l'exterieur du script

    [Header("Donnees")]
    [SerializeField] SOJoyau _donneesJoyau; //champs pour lier les donnees des joyaux
     [SerializeField] SOFeraille _donneesFeraille; //champs pour lier les donnees des ferailles

    [SerializeField, Range(1, 5)] int _niveau = 1; //champs d'initialisation du niveau

    [SerializeField, Range(0, 25)] int _fleche = 10; //champs d'initialisation des fleches
    [SerializeField, Range(0, 100)] int _rage = 10; //champs d'initialisation du niveau de rage 

    [SerializeField, Range(1, 5)] int _niveauIni = 1; //champs d'initialisation du niveau initial

    [SerializeField, Range(0, 25)] int _flecheIni = 10; //champs d'initialisation des fleches initial
    [SerializeField, Range(0, 100)] public int _totalEnemieTuer; //variable pour cumuler le nombre de joyau recuperer #tp4 (Alex Guilbault)
    
    [SerializeField, Range(0, 100)] public int _totalJoyauRecuperer; //variable pour cumuler le nombre de joyau recuperer  #tp4 (Alex Guilbault)
    [SerializeField, Range(0, 100)] public int _totalFerailleRecuperer; //variable pour cumuler le nombre de ferraille recuperer  #tp4 (Alex Guilbault)
    private int _etape; //gere l'etape de la partie et mene le joueur vers l'enigme et le boss  #tpSynthese (Mathieu Chavigny)
    private int _flecheMax = 25; // determine le nombre de fleche maximum  #tp3 (Alex Guilbault)
    [SerializeField, Range(0,3)] private int _vieJoueur = 3;  // determine la vie du joueur  #tpSynthese (Alex Guilbault)
    private int _vieIni = 3; //initialise la vie initial du joueur  #tpSynthese (Alex Guilbault)
    [SerializeField, Range(0, 100)] public int _rageIni = 10; //champs d'initialisation du niveau de rage initial

    [SerializeField] int _argentEnJoyaux; //champs d'initialisation de l'argent en joyaux
    [SerializeField] int _argentEnFeraille; //champs d'initialisation de l'argent en feraille
    [Header("Evenement")]
    UnityEvent _evenementMisAJour = new UnityEvent(); //creation de l'evenement
    public UnityEvent evenementMisAJour => _evenementMisAJour;   //accesseur pour permettre l'acces par l'exterieur du script
    UnityEvent _evenementActivationBonus = new UnityEvent(); //creation de l'evenement
    public UnityEvent evenementActivationBonus => _evenementActivationBonus; //accesseur pour permettre l'acces par l'exterieur du script

    
    public int niveau { get => _niveau; set => _niveau = Mathf.Clamp(value, 1, int.MaxValue);} //accesseur pour permettre l'acces par l'exterieur du script
    
    public int fleche { get => _fleche; set { _fleche = value; _evenementMisAJour.Invoke(); } } //accesseur pour permettre l'acces par l'exterieur du script
    
    public int rage { get => _rage; set {_rage = value; _evenementMisAJour.Invoke();} } //accesseur pour permettre l'acces par l'exterieur du script
    
    public int argentEnJoyaux { get => _argentEnJoyaux;} //accesseur pour permettre l'acces par l'exterieur du script
    public int argentEnFeraille { get => _argentEnFeraille; set => _argentEnFeraille = value; } //accesseur pour permettre l'acces par l'exterieur du script
    public int niveauIni { get => _niveauIni; set => _niveauIni = value; } //accesseur pour permettre l'acces par l'exterieur du script

    public int _tempsRestant; //integrer du temps restant (Alex Guilbault)
    public int tempsRestant { get => _tempsRestant; set => _tempsRestant = value; }
    public int etape { get => _etape; set => _etape = value; } //accesseur pour permettre l'acces par l'exterieur du script 
    public int flecheMax { get => _flecheMax; set => _flecheMax = value; } //accesseur pour permettre l'acces par l'exterieur du script
    public int vieJoueur { get => _vieJoueur; set => _vieJoueur = value; } //accesseur pour permettre l'acces par l'exterieur du script
    public int vieIni { get => _vieIni; set => _vieIni = value; } //accesseur pour permettre l'acces par l'exterieur du script

    public bool _armeActifArc = false; // determine si c'est l'arc qui est actif #tpSynthese (Alex Guilbault)

    /// <summary>
    /// Remet tous les inventaires du personnages
    /// Remet tous les "portefeuille" du personnages #tp4 (Alex Guilbault)
    /// </summary>
    public void Initialiser()
    {
        _armeActifArc = false;
        _totalEnemieTuer = 0;
        _totalJoyauRecuperer = 0;
        _totalFerailleRecuperer = 0;
        _rage = _rageIni;
        _fleche = _flecheIni;
        _niveau = _niveauIni;
        _lesObjets.Clear();
        _argentEnJoyaux = 0;
        argentEnFeraille = 0;
        _etape = 0;
        _vieJoueur = _vieIni;
    }
    /// <summary>
    /// methode qui compile les joyaux et exprime leur valeur selon leur nombre
    /// </summary>
    public void CompterJoyaux()
    {
        _argentEnJoyaux += _donneesJoyau.valeur;
        _totalJoyauRecuperer ++; //ajout de la valeur du joyau a la variable totalJoyauRecuperer #tp4 (Alex Guilbault)
        Debug.Log(argentEnJoyaux);
    }
    /// <summary>
    /// methode qui compile les ferailles et exprime leur valeur selon leur nombre
    /// </summary>
    public void CompterFeraille()
    {
        argentEnFeraille += _donneesFeraille.valeur;
        _totalFerailleRecuperer ++; //ajout de la valeur de la ferraille a la variable totalFerailleRecuperer #tp4 (Alex Guilbault)
        Debug.Log(_argentEnFeraille);
    }
    public void CompterEnnemi()
    {
        _totalEnemieTuer ++; //ajout de la valeur de la ferraille a la variable totalFerailleRecuperer #tp4 (Alex Guilbault)

    }

    /// <summary>
    /// methode qui s'occupe de l'achat d'une amelioration, diminue l'argent du personnage et ajoute l'amelioration a l'inventaire
    /// </summary>
    /// <param name="donneesObjet"></param>
    public void AcheterAmelioration(SOAmelioration donneesObjet)
    {
        argentEnFeraille -= donneesObjet.prix;
        _evenementMisAJour.Invoke();
        _lesAmeliorations.Add(donneesObjet);

    }

    

    /// <summary>
    /// methode qui s'occupe de l'achat d'une potion, diminue l'argent du personnage et ajoute l'amelioration a l'inventaire
    /// </summary>
    /// <param name="donneesObjet"></param>
    public void AcheterPotion(SOPotion donneesObjet)
    {
        _argentEnJoyaux -= donneesObjet.prix;
        _evenementMisAJour.Invoke();
        _lesPotions.Add(donneesObjet);
    }
    /// <summary>
    /// methode qui s'occupe de reinitialiser le personnage lorsque l'application est quittee
    /// </summary>
    void OnApplicationQuit()
    {
 
        Initialiser();
        _niveau = _niveauIni;
        _lesAmeliorations.Clear();
        _lesPotions.Clear();
    }
    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        _evenementMisAJour.Invoke(); 
    }
    
}