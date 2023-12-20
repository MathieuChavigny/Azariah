using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui controle les collisions de la cle
/// Auteur des commentaires: Mathieu Chavigny
/// auuteur du code Mathieu Chavigny
/// </summary>
public class Cle : MonoBehaviour
{
    
    [SerializeField] SOObjetMap _donnees; //champs pour associer les donnees de l'objet cle

    public SOObjetMap donnees { get => _donnees; set => _donnees = value; } //accesseur pour permettre l'acces par l'exterieur du script
    [SerializeField] AudioClip _sonPrendreCle; //champs pour associer le son de la cle

    /// <summary>
    /// Lorsque la cle entre en contacte avec le personnage, elle lui declenche sa fonction Obtenir
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {

        Perso perso = other.GetComponent<Perso>(); //accede au composant de Perso
        if(perso != null) //verifie si perso est different de nul dans le but d'associer les donnees de la cle seulement a perso
        {
            GestSonore.instance.JouerEffetSonore(_sonPrendreCle); //appel de la methode JouerEffetSonore de la classe GestSonore    
            perso.Obtenir(donnees); //appel de la methode Obetnir du perso avec les donnees de la cle
            Destroy(gameObject); //destruction de l'objet car les donnees ont ete tranfere
        }
    }
}