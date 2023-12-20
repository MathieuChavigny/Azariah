using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui controle les donnees d'un morceau de feraille
/// Auteur des commentaires: Mathieu Chavigny
/// auuteur du code  Mathieu Chavigny
/// </summary>
[CreateAssetMenu(fileName = "Feraille", menuName = "Feraille")]
public class SOFeraille : ScriptableObject
{

    [SerializeField] int _valeur = 5; //champs pour definir le nom de l'objet

    public int valeur { get => _valeur; set => _valeur = value; } //accesseur pour permettre l'acces par l'exterieur du script
    [SerializeField]/*Tooltip*/ Sprite _sprite; //champs pour definir le sprite de l'objet

}
