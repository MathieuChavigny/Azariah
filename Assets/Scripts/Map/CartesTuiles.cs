using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>
/// Classe qui controle une carte tuile
/// Auteurs : Alex Guilbault et Mathieu Chavigny
/// Auteur des commentaires: Alex Guilbault
/// </summary>
public class CartesTuiles : MonoBehaviour
{
    [SerializeField, Range(1f, 100f)] float _probabilite; //Déclaration d'un champ pour la probabilité d'apparition des blocs

    float _maxAlpha = 1f; //Déclaration d'une variable pour l'alpha(opaque) des blocs
    Niveau _niveau; //Déclaration d'un champ pour accéder au script Niveau
    Tilemap _tilemap;  //Déclaration d'un champ pour obtenir une tilemap

    /// <summary>
    /// Called when the script is loaded or a value is changed in the inspector (Called in the editor only).
    /// Appelle la fonction afin de changer l'alpha des blocs selon la probabilité choisie
    /// </summary>
    void OnValidate()
    {
        if (Application.isPlaying == false) //if qui vérifie si le jeu joue et si c'est égal à faux alors on ajuste l'alpha des blocs selon le slider()
        {
            _tilemap = GetComponent<Tilemap>(); //initialisation de la variable _tilemap en atribuant la tilemap
            float valeurAlpha = _probabilite / 100; //Déclaration d'une variable qui divise la probabilité par 100 pour mieux 
            _tilemap.color = new Color(255, 255, 255, valeurAlpha); //Change l'alpha des blocs selon la variable valeurAlpha qui a été définie dans l'inspecteur
        }
    }


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// fonction dans laquelle on détermine les positions des tuiles
    /// fonction dans laquelle on détermine les positions des tuiles
    /// </summary>
    void Awake()
    {
        _tilemap = GetComponent<Tilemap>(); //initialise la tilemap
        _tilemap.color = new Color(255, 255, 255, _maxAlpha); //Met l'alpha de tous les tuiles à leur couleur d'origine et l'alpha à 100%
        int chanceApparition = Random.Range(0, 99);
        BoundsInt bounds = _tilemap.cellBounds; // détermine les bounds d'une boite autour de la carteTuiles
        _niveau = GetComponentInParent<Niveau>();   //initialisation des enfants de niveau dans la variable _niveau
        Vector3Int decalage = Vector3Int.FloorToInt(transform.position);  //Déclaration de décalage pour déplacer les tuiles dans leur niveau respectif
        if (_probabilite > chanceApparition) //if de si la probabilité est plus grande que la chance d'apparition, on va chercher les tuiles individuelement et demande au niveau de la placer la tuile grâce à la fonction TraiterTuile
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)  //Double boucles pour parcourir les positions x,y individuelemnt de la tilemap CarttesTuiles
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {

                    Vector3Int pos = new Vector3Int(x, y, 0);  //initialise un vector3Int qui reçoit la position de la tuiles
                    TileBase tuile = _tilemap.GetTile(pos); //initialise la tuile en prenant la position initialiser juste avant et l'initialise dans tuile
                    _niveau.TraiterTuiles(pos, tuile, decalage);  //appelle la fonction pour traiter les tuiles dans le script Niveau et envoie comme paramètre pos, tuile, decalage
                }
            }
            gameObject.SetActive(false); //désactive le gameobject parce que toute les tuiles de la tilemap a été transféré et affiché dans niveau
        }
        else // //if de si la probabilité est plus petite que la chance d'apparition,
        {
            gameObject.SetActive(false); // on désactive le gameobject parce qu'elle ne sera pas affiché
        }
    }
}
