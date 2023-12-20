using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui controle les collisions de la porte
/// Auteur des commentaires: Mathieu Chavigny
/// auteur du code Mathieu Chavigny
/// </summary>
public class Porte : MonoBehaviour
{
    [SerializeField] SOObjetMap _donnees; //champs pour associer les donnees de l'objet porte
    [SerializeField] SONavigation SONavigation; //champs pour associer les donnees de navigation
    Perso _perso; //acces au personnage 

    [SerializeField] AudioClip _sonPorte; //champs pour associer le son de la porte

   /// <summary>
   /// Lorsque la porte entr en contact avec le joueur elle lui demande la cle pour passer a la prochaine scene la boutique
   /// </summary>
   /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        

        _perso = other.GetComponent<Perso>(); //accede au composant de Perso
        if (_perso != null) //verifie si perso est different de nul dans le but d'associer les donnees de la cle seulement a perso
        {
           if(_perso.VerifierCle()) //Verifie si le perso a la cle 
           {
            GestSonore.instance.JouerEffetSonore(_sonPorte); //joue le son de la porte
            StartCoroutine(CoroutineAttendre()); //appel de la coroutine pour attendre 1 seconde avant de changer de scene  #tp4 (Alex Guilbault)
           }
        } 
    }

    IEnumerator CoroutineAttendre()
    {
        yield return new WaitForSeconds(1);
        SONavigation.AllerSceneSuivante(); //appel de la methode pour aller a la prochaine scene dans le systeme de navigation #tp4 (Alex Guilbault)

    }

}
