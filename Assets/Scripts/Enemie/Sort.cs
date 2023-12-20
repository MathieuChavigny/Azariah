using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort: MonoBehaviour
{

    //variable de la vitesse des projectiles
    private float _vitesse = 6.5f;
    //variable du rigidbody du projectile
    private Rigidbody2D _rb2D;
    
    
    public string playerTag = "Player";
    GameObject _perso;
    Vector3 _direction;
    


    //fonction qui démarre au début
    void Start()
    {
        _perso = GameObject.FindGameObjectWithTag(playerTag);
        _direction = (_perso.transform.position - transform.position).normalized;
        if (_perso == null)
        {
            Debug.LogWarning("Player object not found!");
        }
        
        //definie la variable du rigibody du sort du boss
        _rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {    }

    /// <summary>
    /// Gère les collisions du projectile si le joueur est toucher il prend des dégats si c'est un mur/sol le sort se detruit
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {   
        //avec le joueur
        if (other.gameObject.CompareTag("Player"))
        {   
            Debug.Log("Le joueur a été touché");
            other.gameObject.GetComponent<Perso>().DegatJoueur();
            //détruit le gameObject
            Destroy(gameObject);
        }
        //avec les mur
        if (other.gameObject.CompareTag("Sol"))
        {
            //détruit le gameObject
            Destroy(gameObject);
        }
        


    }
    

    /// <summary>
    /// bouge le rigidBody du projectile dans la direction voulue
    /// </summary>
    void FixedUpdate()
    {   
        if (_perso != null)
        {
            _rb2D.MovePosition(transform.position + _direction * _vitesse * Time.fixedDeltaTime);
        }
    }   
}
