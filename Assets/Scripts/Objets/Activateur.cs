using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
// =========================================
// Auteur : Alex Guilbault
// Auteur des commentaires: Alex Guilbault
// ==========================================
/// Classe qui s'occupe du fonctionnement de l'activateur
/// </summary>
public class Activateur : MonoBehaviour
{
    [SerializeField] Sprite _activateurActivé; // champs pour les sprites quand le bonus est activé
    SpriteRenderer _srActivateur; // variable pour le SpriteRenderer de l'activateur
    Activateur _activateur; // accède au script _activateur

    Perso _perso; // accède au script _perso

    [SerializeField] SOPerso _donneesPerso; // accède au scriptable object de _donneesPerso


    // Start is called before the first frame update
    void Start()
    {
        _srActivateur = GetComponent<SpriteRenderer>();  // definie le SpriteRenderer
        _activateur = GetComponent<Activateur>(); //definie le Script
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// invoke l'evenement des bonus de l'activatiion
    /// Desactive le Script
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>

    void OnTriggerEnter2D(Collider2D other)
    {
        _perso = other.GetComponent<Perso>(); // definie le Script Perso
        if (_perso != null)
        {
            _srActivateur.sprite = _activateurActivé;
            _donneesPerso.evenementActivationBonus.Invoke();
            _activateur.enabled = false;
        }
    }
}

