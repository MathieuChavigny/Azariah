using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
// =========================================
// Auteur : Alex Guilbault
// Auteur des commentaires: Alex Guilbault
// ==========================================
/// Classe qui controle les deplacement et collision des fleches
/// </summary>
public class Fleche : MonoBehaviour
{

    private float _vitesse = 8f; //variable de la vitesse des projectiles

    private Rigidbody2D _rb2D;//variable du rigidbody du projectile

    private Vector3 _direction;//donne la direction à laquelle le projectile a été tirer

    [SerializeField] AudioClip _sonFleche; //son de la fleche


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        //definie la variable du rigibody du sort du boss
        _rb2D = GetComponent<Rigidbody2D>();
    }


    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    ///fonction qui vérifie les colisions du sort
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Sol")) //si la fleche touche au Sol
        {
            GestSonore.instance.JouerEffetSonore(_sonFleche); //joue le son de la fleche
            DetruireFleche(); //detruit la fleche
        }
        else if(other.gameObject.CompareTag("Enemie"))
        {
            Enemie enemie = other.gameObject.GetComponent<Enemie>();
            enemie.SubirDegat(10);
            DetruireFleche();
        }


    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        _rb2D.MovePosition(transform.position + _direction * _vitesse * Time.fixedDeltaTime); //déplace le projectiles
    }

    //donne l'angle du projectile
    public void Angle(Vector3 v)
    {
        _direction = v;
    }

    public void DetruireFleche()
    {
        Destroy(gameObject);
    }
}


