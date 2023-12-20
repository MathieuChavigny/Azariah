using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui controle les donnees d'un joyau
/// Auteur des commentaires: Mathieu Chavigny
/// auuteur du code  Mathieu Chavigny
/// </summary>
[CreateAssetMenu(fileName = "Joyau", menuName = "Joyau")]
public class SOJoyau : ScriptableObject
{
    //[SerializeField] string _nom = "Joyau"; //champs pour definir le nom de l'objet
    [SerializeField] int _valeur = 10; //champs pour definir la valeur de l'objet

    public int valeur { get => _valeur; set => _valeur = value; } //accesseur pour permettre l'acces par l'exterieur du script
    [SerializeField]/*Tooltip*/ Sprite _sprite; //champs pour definir le sprite de l'objet

   
}
