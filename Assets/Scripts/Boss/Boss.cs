using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class qui gère les déplacements et les degats du boss
/// Auteur du code : Mathieu Chavigny #tpSynthese
/// </summary>
public class Boss: MonoBehaviour
{

    [Header("Deplacement")]
    [SerializeField] Transform[] _tDest; //tableau de destinations
    [SerializeField] float _delaiPremierDepart = 2f; //delai avant le premier départ
    [SerializeField] float _delaiDepartsSuivants = 5f; //delai entre les départs suivants

    [SerializeField] float _vitesseMax = 5f; //vitesse maximale du dragon
    [SerializeField] float _force = 10f;  //force (10)
    [SerializeField] float _distFreinage = 2f;  //distance à partir de laquelle le dragon freine (2)
    [SerializeField] float _toleranceDest = 0.1f;  //distance à partir de laquelle on considère que le dragon est arrivé à destination (0.1)
    int _iDest = 0; //index des destination disponibles

    [Header("Reference")]

    Rigidbody2D _rb; //reference au rigidbody
    SpriteRenderer _sr; //reference au sprite renderer
    [SerializeField] SONavigation _nav; //reference au script de navigation
    [SerializeField] Perso _perso; //reference au script du personnage
    Animator _anim; //reference à l'animator
    public Animator anim { get => _anim; set => _anim = value; } //accesseur de l'animator du boss
    
    [Header("InfoBoss")]
    private int _vieBoss = 50; //vie du boss

    public int vieBoss { get => _vieBoss; set => _vieBoss = value; } //accesseur de la vie du boss




    /// <summary>
    /// Initialise les compasants du dragon et démarre la coroutine de gestion du trajet
    /// </summary>
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _rb.MovePosition(_tDest[0].position);
        StartCoroutine(CoroutineGererTrajet()); 


    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(_rb.velocity.x > 0.1f)
        {
            _sr.flipX = true;
        }
        else if(_rb.velocity.x < -0.1f)
        {
            _sr.flipX = false;
        }
    }

    /// <summary>
    /// Coroutine qui gère le trajet du dragon
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineGererTrajet()
    {
        yield return new WaitForSeconds(_delaiPremierDepart); //attendre avant de démarrer
        while (true)//boucle infinie
        {
            //obtenir la prochaine destination
            Vector2 posDest = ObtenirPosProchaineDestination();
            while (Vector2.Distance(transform.position, posDest) > _toleranceDest) //tant que la distance est plus grande que la tolérance
            {
                AjouterForceVersDestination(posDest); //ajouter de la force vers la destination

                yield return new WaitForFixedUpdate();    //attendre la prochaine frame 
            }

            yield return new WaitForSeconds(_delaiDepartsSuivants); //attendre avant de démarrer

        }
    }

    /// <summary>
    /// Ajoute de la force vers la destination
    /// </summary>
    /// <param name="posDest"></param>
    private void AjouterForceVersDestination(Vector2 posDest)
    {
        Vector2 direction = (posDest - (Vector2)transform.position).normalized; //direction vers la destination
        float dist = Vector2.Distance(transform.position, posDest); //distance vers la destination
        float ratioFreinage = (dist > _distFreinage) ?1: dist / _distFreinage;
        _rb.AddForce(direction * _force); //ajouter de la force dans la direction
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _vitesseMax * ratioFreinage); //limiter la vitesse

    }

    /// <summary>
    /// choisi parmi le tableau de destinations une destination au hasard
    /// </summary>
    /// <returns></returns>
    Vector2 ObtenirPosProchaineDestination()
    {
        _iDest=Random.Range(0,_tDest.Length);
        if (_iDest >= _tDest.Length) _iDest = 0;
        Vector2 pos = _tDest[_iDest].position;
        return pos;
    }

    /// <summary>
    /// Fonction qui gère les dégats du boss
    /// </summary>
    /// <param name="degat"></param>
    public void SubirDegat(int degat)
    {
        Debug.Log("Degat du Boss");
        vieBoss -= degat;
        if (vieBoss <= 0)
        {
            MortBoss();
        }
    }
    /// <summary>
    /// Fonction qui gère la mort du boss
    /// </summary>
    public void MortBoss()
    {
        _nav.RetourJeu(); //retourne à la scène du jeu
        Destroy(gameObject); //détruit le gameObject de l'ennemi #tpsynthese (Alex Guilbault)
    }
}
