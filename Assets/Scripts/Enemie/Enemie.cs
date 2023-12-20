using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe mere qui gere le deplacement de base et le comportement de base aux ennemies
/// Auteur Mathieu Chavigny #tpSynthese
/// </summary>
public class Enemie : MonoBehaviour
{


    [SerializeField] SOPerso _donnees; //champs pour associer les donnees de l'objet porte Mathieu Chavigny #tpSynthese

    public SOPerso donnees { get => _donnees; set => _donnees = value; } //accesseur pour les scripts externes  Mathieu Chavigny #tpSynthese

    [Header("les delais pour les départs")]

    [SerializeField] float _delaiPremierDepart = 0.5f; //delai avant le premier départ
    [SerializeField] float _delaiDepartsSuivants = 1f; //delai entre les départs suivants

    [Header("Propriétés de deplacement")]
    float _vitesseMax; //vitesse maximale des ennemies
    float _force;  //force des ennemies! (10)
    [SerializeField][Range(0.1f, 5f)] float _distFreinage = 2f; //distance à partir de laquelle on commence à freiner 
    [SerializeField] float _toleranceDest = 0.1f;  //distance à partir de laquelle on considère que les ennemies est arrivé à destination

    
    Vector2 _tailleHitboxEnnemi = new Vector2(10f, 3f);  // variable qui sert à déplacé le gizmo et la hitbox pour le saut
    [SerializeField] LayerMask _layerMask;  // champ serialiser utilisé pour le layer du sol
    public bool _toucheJoueur = false;  // vérifie si le joueur est au sol



    int _iDest = 0; //index des destination disponibles

    Rigidbody2D _rb; //reference au rigidbody
    Salle _salle; //reference à la salle
    SpriteRenderer _sr; //reference au sprite renderer
    Coroutine _coroutineGererTrajet; //reference à la coroutine de gestion du trajet
    bool _modeChasse = false; //variable pour le mode chasse
    public bool modeChasse { get => _modeChasse; set => _modeChasse = value; } //accesseur pour le mode chasse
    public int vieEnnemi = 20; //variable pour la vie de l'ennemi Mathieu Chavigny #tpSynthese
    private Collider2D _colJoueur; //variable pour le collider du joueur Mathieu Chavigny #tpSynthese
    public Collider2D colJoueur { get => _colJoueur; set => _colJoueur = value; } //accesseur pour le collider du joueur Mathieu Chavigny #tpSynthese
    public Perso _perso; //variable pour le perso Mathieu Chavigny #tpSynthese
    [SerializeField] AudioClip _sonDegats; //variable pour le son de degats Mathieu Chavigny #tpSynthese

    [SerializeField] AudioClip _sonMort; //variable pour le son de mort Mathieu Chavigny #tpSynthese
    [SerializeField] ParticleSystem _particuleSang; // Definit le ParticleSystem de l'invisibilite 

    /// <summary>
    /// Initialise les composants des ennemies
    /// </summary>
    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _vitesseMax = Random.Range(10f, 20f);
        _force = Random.Range(5f, 10f);  //force  (10)
        _salle = GetComponentInParent<Salle>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.MovePosition(_salle.tDest[0].position);    //placer le camion à la première destination
        _coroutineGererTrajet = StartCoroutine(CoroutineGererTrajet()); //démarrer la coroutine  
        _particuleSang.Stop();

    }

    /// <summary>
    /// verification si l'ennemie voit le joueur
    /// </summary>
    void Update()
    {
        VerifierJoueur();
    }
    /// <summary>
    /// Coroutine qui gère le déplacement des ennemies
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

                yield return new WaitForFixedUpdate();    //attendre la prochaine frame (physique!)
            }
            

            yield return new WaitForSeconds(_delaiDepartsSuivants); //attendre avant de démarrer

        }
    }
    /// <summary>
    /// Ajoute de la force de la direction des la prochaine destination dans le deplacement des ennemies
    /// </summary>
    /// <param name="posDest"></param>
    private void AjouterForceVersDestination(Vector2 posDest)
    {
        Vector2 dir = (posDest - (Vector2)transform.position).normalized; //direction vers la destination
        float dist = Vector2.Distance(transform.position, posDest); //distance vers la destination
        float ratioFreinage = (dist < _distFreinage) ? dist / _distFreinage : 1f;
        _rb.AddForce(dir * _force); //ajouter de la force dans la direction
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _vitesseMax * ratioFreinage); //limiter la vitesse
        if (dir.x > 0) //condition pour tourner l'ennemie selon la direction
        {
            _sr.flipX = false;
        }
        else if (dir.x < 0)
        {
            _sr.flipX = true;
        }
    }
    /// <summary>
    /// tire la prochaine position de destination
    /// </summary>
    /// <returns></returns>
    Vector2 ObtenirPosProchaineDestination()
    {
        _iDest++;
        if (_iDest >= _salle.tDest.Length) _iDest = 0;
        Vector2 pos = _salle.tDest[_iDest].position;
        return pos;


    }

    

    /// <summary>
    ///  Fonction qui détruit l'ennemi
    /// </summary>
    public void MortEnemie()
    {
        Debug.Log("Mort de l'ennemi");
        GestSonore.instance.JouerEffetSonore(_sonMort); //joue le son de mort de l'ennemi #tpsynthese (Alex Guilbault)
        Destroy(gameObject); //détruit le gameObject de l'ennemi #tpsynthese (Alex Guilbault)
    }
    /// <summary>
    /// Fonction qui fait subir des degats a l'ennemi
    /// </summary>
    /// <param name="degat"></param>
    public void SubirDegat(int degat)
    {
        Debug.Log("Degat de l'ennemi");
        vieEnnemi -= degat;
        _particuleSang.Play(); //joue la particule de sang #tpsynthese (Alex Guilbault)
        if (vieEnnemi <= 0)
        {
            donnees._totalEnemieTuer++; //ajoute 1 au total d'ennemi tuer #tpsynthese (Mathieu Chavigny)
            MortEnemie();
        }
        GestSonore.instance.JouerEffetSonore(_sonDegats); //joue le son de degat de l'ennemi #tpsynthese (Alex Guilbault)
    }

    /// <summary>
    /// Fonction qui verifie si l'ennemi voit le joueur
    /// </summary>
    void VerifierJoueur()
    {
        Vector2 pointDepart = (Vector2)transform.position; //calcul les coordonné de la hitbox en dessous du personnage
        _colJoueur = Physics2D.OverlapBox(pointDepart, _tailleHitboxEnnemi, 0, _layerMask); // initialise la hitbox
        _toucheJoueur = _colJoueur != null; //vérifie si il touche au sol
        if (_toucheJoueur)
        { 
            _perso = colJoueur.GetComponent<Perso>();
            StopCoroutine(_coroutineGererTrajet); //arreter la coroutine
            _rb.velocity = new Vector2(0, _rb.velocity.y); //mettre la vitesse à 0
            _modeChasse = true; //activer le mode chasse
            
        }    
    }
    
    
    /// <summary>
    /// Dessine un gizmos pour voir la hitbox de l'ennemi
    /// </summary>
    void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
        {  //si le jeu est en train de jouer
            VerifierJoueur(); ///appelle la méthode VerifierSol
        }
        if (_toucheJoueur)
        { //si il touche au sol
            Gizmos.color = Color.green; //le gizmos sera vert

            StopCoroutine(_coroutineGererTrajet); //arreter la coroutine
            _rb.velocity = Vector2.zero; //mettre la vitesse à 0
            _modeChasse = true; //activer le mode chasse

        }
        else { Gizmos.color = Color.red; } //si il touche pas au sol le gizmos sera rouge

        Vector2 pointDepart = (Vector2)transform.position; //calcul les coordonné de la hitbox en dessous du personnage
        Gizmos.DrawWireCube(pointDepart, _tailleHitboxEnnemi); //dessine dans la scene les bordures carré rouge
    }
}
