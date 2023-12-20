using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui controle les collisions de la feraille
/// Auteur des commentaires: Mathieu Chavigny
/// auteur du code Mathieu Chavigny
/// </summary>
public class Feraille : MonoBehaviour
{
    [SerializeField] SOFeraille _donnees; //champs pour associer les donnees de l'objet porte

    public SOFeraille donnees { get => _donnees; set => _donnees = value; } //accesseur pour les scripts externes
    [SerializeField] AudioClip _sonFeraille;

    /// <summary>
    /// Lorsque la feraille touche au joueur il appel la methode pour accumuler les morceaux de ferailles
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Perso perso = other.GetComponent<Perso>(); //accede au composant de Perso
        if(perso != null) //verifie si perso est different de nul dans le but d'associer les donnees de la cle seulement a perso
        {
            GestSonore.instance.JouerEffetSonore(_sonFeraille);
            perso.EconomiserFeraille(donnees); //appel de la methode du perso afin d'economiser
            Destroy(gameObject); //destruction de l'objet car les donnees ont ete tranfere 
        }
          
    }
}
