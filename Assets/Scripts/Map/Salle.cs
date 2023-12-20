using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>
/// Classe qui controle une salle
/// Auteurs : Alex Guilbault et Mathieu Chavigny
/// Auteur des commentaires: Alex Guilbault
/// </summary>
public class Salle : MonoBehaviour
{
    private static Vector2Int _tailleAvecBordures = new Vector2Int(24, 24);  // variable qui initialise en private static la largeur et hauteur des salles
    public static Vector2Int tailleAvecBordure => _tailleAvecBordures;  // accesseur qui initialise en public static la largeur et hauteur des salles

    public Transform[] tDest { get => _tDest; set => _tDest = value; }

    [SerializeField] Transform _repereCle; //champs pour lier la position du repere de la cle (Mathieu Chavigny)
    [SerializeField] Transform _reperePorte; //champs pour lier la position du repere de la porte (Mathieu Chavigny)
    [SerializeField] Transform _repereActivateur; // champs pour le transform de l'activateur(Alex Guilbault)
    [SerializeField] Transform _repereEffector; // champs pour le transform de l'effector (Alex Guilbault)
    [SerializeField] Transform _reperePerso; //champs pour lier la position du repere du perosnnage (Mathieu Chavigny)
    [SerializeField] Transform _repereEnnemi; //champs pour lier la position du bandit (Mathieu Chavigny)
    [SerializeField] GameObject[] _ennemiModele; //champs pour lier prefab du personnage (Mathieu Chavigny)
    [SerializeField] Transform[] _tDest;

    
    /// <summary>
    /// fonction qui trouve le point cenntre de la salle
    /// fonction qui donne la grosseur de la salle
    /// fonction qui déssine un gizmo autour des salles pour voir les bords des salles
    /// </summary>
    void OnDrawGizmos()
    {
        Vector2 pointDepart = (Vector2)transform.position; //Initialisation du point centre des salles pour le gizmo 
        Vector2 tailleSalle = tailleAvecBordure; //Initialisation de la taille de la salle en x et en y
        Gizmos.color = Color.green;//Détermine la couleur des gizmos
        Gizmos.DrawWireCube(pointDepart, tailleSalle); //dessine dans la scene les bordures carré vert
    }
    /// <summary>
    /// Methode qui s'occupe de placer la cle sur son repere dans la salle (Mathieu Chavigny)
    /// </summary>
    /// <param name="modele"></param>
    /// <returns></returns>
    public Vector2Int PlacerCleSurRepere(GameObject modele){
        Vector3 pos = _repereCle.position; //assigne la position du repere a un vector3 (Mathieu Chavigny)
        Niveau niveau = GetComponentInParent<Niveau>();
        Instantiate(modele, pos, Quaternion.identity, niveau.tmCible.transform); //intancie le prefab de la cle et le place sur la tilemap du niveau (Mathieu Chavigny)
        niveau._lesPosSurReperes.Add(Vector2Int.FloorToInt(_repereCle.position)); //ajoute la position du repere a une liste (Mathieu Chavigny)
        return Vector2Int.FloorToInt(pos);
    }
    /// <summary>
    /// Methode qui s'occupe de placer la porte sur son repere dans la salle (Mathieu Chavigny)
    /// </summary>
    /// <param name="modele"></param>
    /// <returns></returns>
    public Vector2Int PlacerPorteSurRepere(GameObject modele){
        Vector3 pos = _reperePorte.position; //assigne la position du repere a un vector3 (Mathieu Chavigny)
        Niveau niveau = GetComponentInParent<Niveau>(); 
        Instantiate(modele, pos, Quaternion.identity, niveau.tmCible.transform); //intancie le prefab de la porte et le place sur la tilemap du niveau (Mathieu Chavigny)
        niveau._lesPosSurReperes.Add(Vector2Int.FloorToInt(_reperePorte.position)); //ajoute la position du repere a une liste (Mathieu Chavigny)
        return Vector2Int.FloorToInt(pos);
    }
    /// <summary>
    /// fonction qui positionne l'activateur mais aussi retourne sa position
    /// (Alex Guilbault)
    /// </summary>
    /// <param name="modele"></param>
    /// <returns></returns>
    public Vector2Int PlacerActivateurSurRepere(GameObject modele){
        Vector3 pos = _repereActivateur.position; //va chercher la position du repere dans salle (Alex Guilbault)
        Niveau niveau = GetComponentInParent<Niveau>();
        Instantiate(modele, pos, Quaternion.identity, niveau.tmCible.transform); //instantie le gameObject dans la salle (Alex Guilbault)
        niveau._lesPosSurReperes.Add(Vector2Int.FloorToInt(_repereActivateur.position)); //ajoute la pos a la liste _lesPosSurReperes (Alex Guilbault)
        return Vector2Int.FloorToInt(pos);
    }
    /// <summary>
    /// fonction qui positionne l'effector mais aussi retourne sa position
    /// (Alex Guilbault)
    /// </summary>
    /// <param name="modele"></param>
    /// <returns></returns>
    public Vector2Int PlacerEffectorSurRepere(GameObject modele){
        Vector3 pos = _repereEffector.position; //va chercher la position du repere dans salle (Alex Guilbault)
        Niveau niveau = GetComponentInParent<Niveau>();
        Instantiate(modele, pos, Quaternion.identity, niveau._conteneurEffector.transform); //instantie le gameObject dans la salle (Alex Guilbault)
        niveau._lesPosSurReperes.Add(Vector2Int.FloorToInt(_repereEffector.position)); //ajoute la pos a la liste _lesPosSurReperes (Alex Guilbault)
        return Vector2Int.FloorToInt(pos); 
    }
    /// <summary>
    /// Methode qui s'occupe de placer le personnage sur son repere dans la salle (Mathieu Chavigny)
    /// </summary>
    /// <param name="modele"></param>
    /// <returns></returns>
    public Vector2Int PlacerPersoSurRepere(GameObject modele){
        Vector3 pos = _reperePerso.position; //assigne la position du repere a un vector3 (Mathieu Chavigny)
        Niveau niveau = GetComponentInParent<Niveau>();
        //Instantiate(modele, pos, Quaternion.identity, transform); //intancie le prefab du personnage (Mathieu Chavigny)
        modele.transform.position = pos; //assigne la position du repere a un vector3 (Mathieu Chavigny)
        niveau._lesPosSurReperes.Add(Vector2Int.FloorToInt(_reperePerso.position)); //ajoute la position du repere a une liste (Mathieu Chavigny)
        return Vector2Int.FloorToInt(pos);
    }
    public void PlacerEnnemiSurRepere(){
        Vector3 pos = _repereEnnemi.position; //assigne la position du repere a un vector3 (Mathieu Chavigny)
        Niveau niveau = GetComponentInParent<Niveau>();
        Instantiate(_ennemiModele[Random.Range(0,2)], pos, Quaternion.identity, transform); //intancie le prefab de l'ennemi (Mathieu Chavigny)
        niveau._lesPosSurReperes.Add(Vector2Int.FloorToInt(_repereEnnemi.position)); //ajoute la position du repere a une liste (Mathieu Chavigny)
        //return Vector2Int.FloorToInt(pos);
    }


}