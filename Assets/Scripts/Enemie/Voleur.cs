using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui gere le voleur
/// /// Auteur des commentaires: Alex Guilbault 
/// auuteur du code Alex Guilbault 
/// </summary>
public class Voleur : Enemie
{

    [SerializeField] float _vitesse = 1.5f; //vitesse du voleur #tpSynthese (Alex Guilbault)


    SpriteRenderer _srEnnemi; // sprite renderer du voleur#tpSynthese (Alex Guilbault)
    Transform _posVoleur; // determine la position du voleur#tpSynthese (Alex Guilbault)

    Animator _anim; // animator du voleur

    [SerializeField] AudioClip _sonAttaque; // champ pour le son d'attaque du voleur#tpSynthese (Alex Guilbault)
    

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _anim = GetComponent<Animator>(); // assigne l'animator #tpSynthese (Alex Guilbault)
        _srEnnemi = GetComponent<SpriteRenderer>(); //assigne le SpriteRenderer #tpSynthese (Alex Guilbault)

        _posVoleur = GetComponent<Transform>(); //assinge le transform du voleur #tpSynthese (Alex Guilbault)

    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {

        if (modeChasse) // si le voleur chasse le joueur #tpSynthese (Alex Guilbault)
        {
            Vector3 targetPosition = new Vector3(_perso.transform.position.x, transform.position.y, transform.position.z); // trouve la pos du joueur #tpSynthese (Alex Guilbault)
            float pas = _vitesse * Time.deltaTime; // pas par seconde du voleur #tpSynthese (Alex Guilbault)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, pas); // Se dirige vers le joueur #tpSynthese (Alex Guilbault)
            if (targetPosition.x -transform.position.x  < 0) { _srEnnemi.flipX = true; } // condition pour flipper le voleur en X #tpSynthese (Alex Guilbault)
            else if (targetPosition.x -transform.position.x > 0) { _srEnnemi.flipX = false; }
        }

    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        //avec le joueur
        if (other.gameObject.CompareTag("Player"))
        {
            _anim.SetTrigger("attaque");
            Debug.Log("Le joueur a été touché");
            other.gameObject.GetComponent<Perso>().DegatJoueur();
            
        }
    }
}
