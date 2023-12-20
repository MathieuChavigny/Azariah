using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// =========================================
// Auteur : Alex Guilbault
// Auteur des commentaires: Alex Guilbault
///==========================================
/// classe qui gere les fonctionalite de l'arc
/// /// </summary>
public class Arc : MonoBehaviour
{
    [SerializeField] private Vector3 _targetPos; //trouve la position de la souris
    private float angle;//variable pour l'angle
    [SerializeField] private GameObject _fleche; //prefab du fleche du joueur
    [SerializeField] private Transform _boutArc;//variable pour la position de `sortie des projectiles du joueur
    [SerializeField] private GameObject _arc; //variable pour la position de l'arc

    string[] _nomManettes; //détecte les manette branché dans l'ordinateur

    Perso _perso;  // accède au script _perso
    SpriteRenderer _srArc;  // accède au SpriteRenderer  de l'arc 

    bool _manetteActif = false; //variable qui permet de savoir si le joueur utilise une manette ou non
    [SerializeField] AudioClip _sonTirArc; //son du tir de l'arc  #tpsynthese (Alex Guilbault)
    


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _perso = GetComponentInParent<Perso>();  // definit le script Perso dans _perso
        DetecterManette();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_manetteActif == false)
        {
            //vise grâce à la souris
            _targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            //vise grâce à la manette
            _targetPos = new Vector3(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"), 0) + transform.position;
        }



        angle = TrouverAngle(transform.position, _targetPos);
        transform.rotation = Quaternion.Euler(-2, 0, angle * Mathf.Rad2Deg);

        DetecterTir();
    }

    private void DetecterTir()
    {
        // lorsque le joueur appuie sur le clic gauche de la souris ET que c'est l'arc qui choisie ET que le joueur possede l'arc , cela amêne à la fonction fire()
        if (Input.GetButtonDown("Fire") && _perso.donneesPerso._armeActifArc == true && _perso.VerifierArc() == true)
        {
            if (_perso.donneesPerso.fleche > 0) //si le joueur possède encore des flèches
            {
                _perso.DiminueFleche(); //fonction qui diminue les flèches de l'inventaire du joueur
                Fire(); // amène vers la fonction Fire()
            }
        }
    }

    /// <summary>
    ///code qui fait le calcul de l'angle
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns> 
    private float TrouverAngle(Vector2 v1, Vector2 v2)
    {
        Vector2 v = v2 - v1;
        return Mathf.Atan2(v.y, v.x);
    }
    /// <summary>
    ///fonction de tir du joueur
    /// </summary>
    private void Fire()
    {
        //fait apparaitre la flèche
        GameObject go = Instantiate(_fleche, _boutArc.position, _arc.transform.rotation);
        go.GetComponent<Fleche>().Angle(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0));
        GestSonore.instance.JouerEffetSonore(_sonTirArc);
    }
    /// <summary>
    ///fonction qui active le spriteRenderer
    /// </summary>
    public void ActiverArme()
    {
        _srArc = GetComponentInChildren<SpriteRenderer>();
        _srArc.enabled = _perso.donneesPerso._armeActifArc;
        
    }

    void DetecterManette()
    {
        _nomManettes = Input.GetJoystickNames();
        for (int i = 0; i < _nomManettes.Length; i++)
        {
            if (_nomManettes[i].ToLower().Contains("xbox"))
            {
                // Debug.Log("Manette de Xbox connectée !"); Debug.Log pour debugger au besoin
                _manetteActif = true;

            }
            else if (_nomManettes[i].ToLower().Contains("dualshock"))
            {
                // Debug.Log("Manette de PS4 connectée !"); Debug.Log pour debugger au besoin
                _manetteActif = true;

            }
        }
    }


}



