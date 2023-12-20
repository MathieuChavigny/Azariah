using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui controle l'interaction du bonus d'invisibilite
// =========================================
// Auteur : Alex Guilbault
// Auteur des commentaires: Alex Guilbault
// ==========================================
/// </summary>
public class BonusInvisibilite : MonoBehaviour
{

    bool _estActiver = false; // bool qui montre si le bonus est activé

    [SerializeField] Sprite _invisibiliteActive; // Sprite du bonus activé
    SpriteRenderer _srinvisibilite;  // variable pour le SpriteRenderer du bonus d'invisibilité
    CircleCollider2D _Colliderinvisibilite;  // accède au Collider du bonus d'invisibilité

    Perso _perso;  // accède au script _perso
    [SerializeField] SOPerso _donneesPerso; // accède au scriptable object de _donneesPerso

    SpriteRenderer _srPerso;  // variable pour le SpriteRenderer du Perso

    [SerializeField] ParticleSystem _particuleNuage; //definit le ParticuleSystem

    [SerializeField] AudioClip _sonInvisibilite;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _particuleNuage.GetComponentInChildren<ParticleSystem>(); // definit le spriteRenderer  du ParticleSystem d'invisibilité
        _srinvisibilite = GetComponent<SpriteRenderer>(); // definit le spriteRenderer  du bonus d'invisibilité
        _Colliderinvisibilite = GetComponent<CircleCollider2D>(); // definit le Collider du bonus d'invisibilité
        _donneesPerso.evenementActivationBonus.AddListener(ActiverBonus); // abonne le bonus à l'evenementActivationBonus
        _particuleNuage.Stop(); //arrete le systeme de particule
    }

    // Update is called once per frame
    void Update()
    {
        //changement de sprite et d'alpha quand le bonus est actif
        if (_estActiver == true)
        {
            _srinvisibilite.sprite = _invisibiliteActive;
            _srinvisibilite.color = new Color(1, 1, 1, 1);
        }
        else //changement de l'alpha quand le bonus est inactif
        {
            _srinvisibilite.color = new Color(1, 1, 1, 0.5f);
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
            GestSonore.instance.JouerEffetSonore(_sonInvisibilite);
            _srPerso = other.GetComponent<SpriteRenderer>();
            _srPerso.color = new Color(1, 1, 1, 0.5f);
            _srinvisibilite.enabled = false;
            _Colliderinvisibilite.enabled = false;
            _particuleNuage.Stop();
            StartCoroutine(CoroutineDevenirInvisible(_srPerso));
        }
    }
    /// <summary>
    /// interaction du bonus lorsque le joueur a deja interragie
    /// s'occupe de faire jouer les effets sur le personnage et de detruire le gameObject
    /// </summary>
    /// <param name="_srPerso"></param>
    /// <returns></returns>
    IEnumerator CoroutineDevenirInvisible(SpriteRenderer _srPerso)
    {
        _perso.ActiverEffetInvisible();
        yield return new WaitForSeconds(10f);
        _srPerso.color = new Color(1, 1, 1, 1);
        _perso.DesactiverEffetInvisible();
        Destroy(gameObject);
    }

    // fonction qui est le invoke de l'evenementActiverBonus 
    private void ActiverBonus()
    {
        _estActiver = true;
        _particuleNuage.Play();
    }
}
