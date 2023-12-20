using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui controle les donnees d'une amelioration
/// Auteur des commentaires: Mathieu Chavigny
/// auuteur du code  Mathieu Chavigny
/// </summary>
[CreateAssetMenu(fileName = "Amelioration", menuName = "Amelioration boutique")]
public class SOAmelioration : ScriptableObject
{

    [SerializeField] string _nom = "Arc"; //champs pour definir le nom de l'amelioration

    [SerializeField]/*Tooltip*/ Sprite _sprite; //champs pour definir le sprite de l'amelioration

    [SerializeField] int _prix = 25; //champs pour definir le prix de l'amelioration

    [SerializeField, TextArea] string _description; //champs pour definir ls description de l'amelioration


    public string nom { get => _nom; set => _nom = value; } //accesseur pour permettre l'acces par l'exterieur du script
    public Sprite sprite { get => _sprite; set => _sprite = value; } //accesseur pour permettre l'acces par l'exterieur du script
    public int prix { get => _prix; set => _prix = value; } //accesseur pour permettre l'acces par l'exterieur du script
    public string description { get => _description; set => _description = value; }  //accesseur pour permettre l'acces par l'exterieur du script
}