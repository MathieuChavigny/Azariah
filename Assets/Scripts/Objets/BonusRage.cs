using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui controle les déplacements du personnage
/// Auteurs : Alex Guilbault 
/// Auteur des commentaires: Alex Guilbault
/// </summary>
public class BonusRage : MonoBehaviour
{
    bool _estActiver = false; // bool qui montre si le bonus est activé

    [SerializeField] Sprite _rageActive; // Sprite du bonus activé
    SpriteRenderer _srRage;  // variable pour le SpriteRenderer du bonus de rage

    int _rageGagner = 25; // nb de rage gagner lors de la reception du bonus
    CircleCollider2D _Colliderrage;  // accède au Collider du bonus  de rage

    Perso _perso; // accède au script _perso
    [SerializeField] SOPerso _donneesPerso;  // accède au scriptable object de _donneesPerso

    [SerializeField] ParticleSystem _particuleEclair; //definit le ParticuleSystem
    SpriteRenderer _srPerso; // variable pour le SpriteRenderer du Perso

    [SerializeField] AudioClip _sonRage;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    ///
    /// </summary>
    void Start()
    {
        _srRage = GetComponent<SpriteRenderer>();  // definit le spriteRenderer  du SpriteRenderer de rage
        _particuleEclair.GetComponentInChildren<ParticleSystem>();  // definit le spriteRenderer  du ParticleSystem de rage
        _Colliderrage = GetComponent<CircleCollider2D>();  // definit le spriteRenderer  du CircleCollider2D de rage
        _donneesPerso.evenementActivationBonus.AddListener(ActiverBonus); // abonne le bonus à l'evenementActivationBonus
        _particuleEclair.Stop(); // arrete l'effet du bonus

    }

    // Update is called once per frame
    void Update()
    {

        if (_estActiver == true) //changement de sprite et d'alpha quand le bonus est actif
        {
            _srRage.sprite = _rageActive; //change le sprite pour celui qui est actif
            _srRage.color = new Color(1, 1, 1, 1); //change l'alpha pour qu'il soit opaque
        }
        else
        {
            _srRage.color = new Color(1, 1, 1, 0.5f); //change l'alpha pour qu'il soit semi-transparent
        }
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// interaction du bonus lorsque le joueur lui touche
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        _perso = other.GetComponent<Perso>();
        if (_estActiver == true && _perso != null) // quand le joueur y touche et que le bonus est actif
        {
            GestSonore.instance.JouerEffetSonore(_sonRage);
            _srPerso = other.GetComponent<SpriteRenderer>();
            _Colliderrage.enabled = false; //desactive le  _Collider de rage
            _srRage.enabled = false;
            _srPerso = other.GetComponent<SpriteRenderer>(); // definit le SpriteRenderer du perso
            if (_donneesPerso.rage >= 100) // verifie si la barre de rage du perso est pleine
            {
                _donneesPerso.rage = 100; // plafonne la valeur de rage à 100 (max)
                _perso.ActiverEffetRage();// active l'effet de rage
            }
            else
            {
                _donneesPerso.rage += _rageGagner; //augmente la barre de rage de la valeur du bonus (25)
            }
            StartCoroutine(CoroutineDestructionObjet());//s'occupe de faire jouer les effets et de detruire le gameObject
        }
    }
        /// interaction du bonus lorsque le joueur a deja interragie
    /// s'occupe de faire jouer les effets et de detruire le gameObject
    /// </summary>
    /// <param name="_srPerso"></param>
    /// <returns></returns>
    IEnumerator CoroutineDestructionObjet()
    {
        _particuleEclair.Stop(); //arrete les particule
        yield return new WaitForSeconds(2f); //pause de 2 secondes
        Destroy(gameObject); // detruire les gameObject
    }

    /// <summary>
    /// fonction qui est le invoke de l'evenementActiverBonus
    /// </summary>
    private void ActiverBonus()
    {
        _estActiver = true; 
        _particuleEclair.Play();
    }
}
