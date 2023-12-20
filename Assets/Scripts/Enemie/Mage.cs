using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui controle le mage qui herite de enemie
/// Auteur du code Mathieu Chavigny #tpSynthese
/// </summary>
public class Mage : Enemie
{
    [SerializeField] GameObject _sort; //prefab du sort
    Animator _anim; //reference a l'animator
    Rigidbody2D _rb; //reference au rigidbody
    [SerializeField] AudioClip _sonAttaque; //son de l'attaque


    /// <summary>
    /// Initialise les composants du mage et démarre la coroutine de lancer de sort
    /// </summary>
    void Start()
    {
        StartCoroutine(CoroutineLancerSort());
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        
    }

    /// <summary>
    /// Si le mage ne bouge pas il est en idle sinon il marche
    /// </summary>
    void FixedUpdate()
    {
        if(_rb.velocity.x == 0)
        {
            _anim.SetTrigger("idle");
        }
        else
        {
            _anim.SetTrigger("marche");
        }
        

    }
    /// <summary>
    /// Coroutine qui lance le sort toutes les 5 secondes
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineLancerSort()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            if (modeChasse)
            {
            
                Instantiate(_sort, transform.position, Quaternion.identity, transform);
                _anim.SetTrigger("attaque");
                GestSonore.instance.JouerEffetSonore(_sonAttaque);
                _rb.velocity = Vector2.zero;
            
            
            }
        }
        
    }
}