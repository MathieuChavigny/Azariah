using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui controle la poussiere lors des deplacement
/// Auteurs : Mathieu Chavigny 
/// Auteur des commentaires: Mathieu Chavigny
/// </summary>
public class Poussiere : MonoBehaviour
{
    [SerializeField] Color _teintePoussiere = new Color(193f, 2f, 217f, .3f); //champs serialise pour creer une nouvelle couleur
    
    ParticleSystem _part; //declaration de la propriete pour le systeme de particule 
    ParticleSystem.MainModule _main; //declaration de la propriete pour le module principal
    ParticleSystem.EmissionModule _emission; //declaration de la propriete pour le module d'emission
     

    Perso _perso; //declaration de la propriete pour le personnage
    float _axeH; //declaration de la propriete pour l'axe d'emission

    /// <summary>
    /// Initialisation des proprietes
    /// </summary>
    void Start()
    {
        _part = GetComponent<ParticleSystem>(); //Initialisation du systeme de particule
        _perso  = GetComponentInParent<Perso>(); //Initialisation du personnage
        _main = _part.main; //Initialisation du module principal
        _emission = _part.emission; //Initialisation du module d'emission
        foreach (SOAmelioration amelioration in _perso.donneesPerso.lesAmeliorations) //boucle pour regarder tous les elements de la liste d'amelioration
        {
        
            if (amelioration.nom == "Bottes") //Si la liste contient l'amelioration Bottes on teinte les particules et on arrete la boucle
            {
                Teinter();
                break;
            }
        }
    }
    /// <summary>
    /// teinte la couleur de la poussiere
    /// </summary>
    public void Teinter()
    {
        _main.startColor =_teintePoussiere; //modification du startColor par la couleur souhaitee
    }
    /// <summary>
    /// inverse la vitesse afin de retourner la poussiere pour suivre le mouvement du personnage
    /// </summary>
    public void Inverser()
    {
        _main.startSpeed = _axeH*_perso.vitesse;  //modification du startspeed par la vitesse souhaitee
    }
    
    // Update is called once per frame
    void Update()
    {
        _axeH = _perso.axeH; //Initialisation de la propriete
        
        if(_axeH!=0 && _perso._estAuSol==true) //Verifie si le joueur est en mouvement afin de produire des particules s'il l'est
        {
        
            _emission.rateOverTime = 5; //modification du rythme d'emission 
        }
        else{
           _emission.rateOverTime = 0; //modification du rythme d'emission  
        }
        
        Inverser(); //appel de la methode qui change le cote de l'emission par la vitesse
    }
}
