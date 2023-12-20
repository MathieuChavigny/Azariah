using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui créer le confineur pour la cinemachine
/// auteur du code et des commentaires : Mathieu Chavigny
/// </summary>
public class Confineur : MonoBehaviour
{
    Transform _transform; // variable pour le composant Transform du confineur #tp4
    Vector3Int posDepart = new Vector3Int(-12, -12,0); // position de départ du confineur #tp4
    [SerializeField] SONiveau _niveau; // variable pour le scriptable object du niveau #tp4
    private int _FacteurConversion = 23; // variable pour le facteur de conversion pour convertir la taille du niveau en grandeur pour le confineur #tp4
    private int _bordure = 1; // variable pour la bordure de la map qui agrandi le confineur de 1 unité #tp4
    PolygonCollider2D _collider; // variable pour le composant PolygonCollider2D du confineur #tp4


    /// <summary>
    /// Définit la position de départ et la grosseur du confineur de façon à ce qu'il soit de la même grosseur que le niveau	
    /// </summary>
    void Start()
    {
        _transform = GetComponent<Transform>(); // récupère le composant Transform du confineur #tp4
        _collider = GetComponent<PolygonCollider2D>(); // récupère le composant PolygonCollider2D du confineur #tp4
        _transform.position = posDepart; // définit la position de départ du confineur #tp4
        _transform.localScale = new Vector3(_niveau.taille.x * _FacteurConversion + _bordure, _niveau.taille.y * _FacteurConversion + _bordure, 1); // définit la grosseur du confineur #tp4

    }

}
