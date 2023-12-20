using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui controle les donnees des objets de la map
/// Auteur des commentaires: Mathieu Chavigny
/// auuteur du code Mathieu Chavigny
/// </summary>
[CreateAssetMenu(fileName = "ObjetMap", menuName = "ObjetMap")]
public class SOObjetMap : ScriptableObject
{

    [SerializeField] string _nom = "Clé"; //champs pour definir le nom de l'objet
    [SerializeField]/*Tooltip*/ Sprite _sprite; //champs pour definir le sprite de l'objet
    [SerializeField]/*TextArea*/ string _description; //champs pour definir la description de l'objet

    public string nom { get => _nom; set => _nom = value; } //accesseur pour permettre l'acces par l'exterieur du script
}
