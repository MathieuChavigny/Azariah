using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui gère le projectile du boss
/// Auteur du code : Mathieu Chavigny #tpSynthese
/// </summary>
public class Projectile : MonoBehaviour
{

    private float _vitesse = 6.5f; //vitesse du projectile
    
    private Rigidbody2D _rb2D; //reference au rigidbody
    
    private Vector3 _direction; //direction du projectile
    


    
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>(); //initialise le rigidbody
    }

    /// <summary>
    /// Gère les collisions du projectile
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter2D(Collision2D other)
    {   
        //avec le joueur
        if (other.gameObject.CompareTag("Player"))
        {   
            other.gameObject.GetComponent<Perso>().DegatJoueur();
            Destroy(gameObject);
        }
        //avec les mur et sol
        if (other.gameObject.CompareTag("Sol"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// bouge le rigidBody du projectile dans la direction voulue
    /// </summary>
    void FixedUpdate()
    {           
        _rb2D.MovePosition(transform.position + _direction * _vitesse * Time.fixedDeltaTime);
    }
    /// <summary>
    /// Donne une direction au projectile
    /// </summary>
    /// <param name="v"></param>
    public void Angle(Vector3 v)
    {
        _direction = v;
    } 
}
