using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui controle les collisions d'un joyau
/// Auteur des commentaires: Mathieu Chavigny
/// auteur du code Mathieu Chavigny
/// </summary>
public class Joyau : MonoBehaviour 
{
    [SerializeField] SOJoyau _donnees; //champs pour associer les donnees de l'objet porte

    public SOJoyau donnees { get => _donnees; set => _donnees = value; } //accesseur pour les scripts externes
    [SerializeField] AudioClip _sonJoyau; //son du joyau #tpsynthese (Alex Guilbault)

    /// <summary>
    /// Lorsque le joyau touche au joueur il appel la methode pour accumuler les joyaux
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Perso perso = other.GetComponent<Perso>(); //accede au composant de Perso
        if(perso != null) //verifie si perso est different de nul dans le but d'associer les donnees de la cle seulement a perso
        {
            perso.EconomiserJoyaux(donnees); //appel de la methode du perso afin d'economiser 
            Destroy(gameObject); //destruction de l'objet car les donnees ont ete tranfere 
            GestSonore.instance.JouerEffetSonore(_sonJoyau);
        }
         
    }
}
