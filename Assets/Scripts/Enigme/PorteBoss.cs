using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  classe qui s'occupe de la porte pour mener au boss dans l'enigme
/// </summary>
public class PorteBoss : MonoBehaviour
{

    [SerializeField] Sprite _spritePorteOuverte; // champs pour le sprite de la porte ouverte  #tpSynthese (Alex Guilbault)
    SpriteRenderer _srPorte; // variable pour le SpriteRenderer de la porte  #tpSynthese (Alex Guilbault) 

    public bool _porteOuverte = false; // variable pour la porte ouverte  #tpSynthese (Alex Guilbault)

    [SerializeField] SONavigation SONavigation; //champs pour associer les donnees de navigation #tpSynthese (Alex Guilbault)
    [SerializeField] AudioClip _sonPorte; //champs pour associer les donnees de navigation #tpSynthese (Alex Guilbault)

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _srPorte = GetComponent<SpriteRenderer>(); // definie le SpriteRenderer #tpSynthese (Alex Guilbault)
    }

    /// <summary>
    ///  fonction publique qui active la porte
    /// </summary>
    /// <param name="other"></param>
    public void ActiverPorte()// active la porte du boss
    {
        _srPorte.sprite = _spritePorteOuverte; // definie le sprite de la porte #tpSynthese (Alex Guilbault)
        _porteOuverte = true; // definie la porte comme ouverte #tpSynthese (Alex Guilbault)
    }

        /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Perso perso = other.GetComponent<Perso>(); //accede au composant de Perso #tpSynthese (Alex Guilbault)
        if (perso != null && _porteOuverte) //verifie si perso est different de nul dans le but d'associer les donnees de la cle seulement a perso #tpSynthese (Alex Guilbault)
        {
                StartCoroutine(CoroutineAttendre()); //appel de la coroutine pour attendre 1 seconde avant de changer de scene   #tpSynthese (Alex Guilbault)
        }
    }
    /// <summary>
    /// coroutine d'attente pour son et feedback joueur
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineAttendre()
    {
        yield return new WaitForSeconds(1);
        SONavigation.AllerSceneSuivante(); //appel de la methode pour aller a la prochaine scene dans le systeme de navigation  #tpSynthese (Alex Guilbault)

    }
}
