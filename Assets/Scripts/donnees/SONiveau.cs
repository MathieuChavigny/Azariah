using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui controle les donnees d'un joyau
/// Auteur des commentaires: Mathieu Chavigny
/// auteur du code  Mathieu Chavigny
/// </summary>
[CreateAssetMenu(fileName = "Niveau", menuName = "Niveau")]
public class SONiveau : ScriptableObject
{
    [SerializeField] Vector2Int _taille = new Vector2Int(3, 3); //variable Vector2Int pour définir le nombre de salle en x et y

    public Vector2Int taille { get => _taille; set => _taille = value; } //accesseur pour permettre l'acces par l'exterieur du script


    
}

