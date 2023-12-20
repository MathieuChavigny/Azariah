using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe qui controle le personnage
/// /// Auteur des commentaires: Alex Guilbault et Mathieu Chavigny
/// auuteur du code Alex Guilbault et Mathieu Chavigny
/// </summary>

public class Perso : MonoBehaviour
{

    [Header("Acesseur SOPerso et SONav")]
    [SerializeField] SOPerso _donneesPerso; //champ pour lier les donnees au personnage (Mathieu Chavigny)
    public SOPerso donneesPerso => _donneesPerso; //accesseur des donnees du personnage pour les autres scripts (Mathieu Chavigny)
    [SerializeField] SONavigation _soNavigation; // variable pour le SONavigation

    [Header("Acesseur divers")]
    [SerializeField] float _vitesse = 5; // vitesse de déplacement du joueur (Mathieu Chavigny)
    public float vitesse { get => _vitesse; set => _vitesse = value; }  // accesseur de la vitesse (Mathieu Chavigny)
    float _axeH; //l'axe horizontal (joystick ou clavier)
    public float axeH { get => _axeH; set => _axeH = value; }  // accesseur de l'axe horizontal (Mathieu Chavigny)

    float _axeV; //l'axe vertical (joystick ou clavier)

    [Header("Component important")]
    Rigidbody2D _rb;    //variable pour le rigidody2D
    SpriteRenderer _sr;   // variable pour le SpriteRenderer

    [Header("hitbox saut")]
    Vector2 _distanceDebutSol = new Vector2(0, 0.8f); // variable qui sert à déplacé le gizmo et la hitbox pour le saut
    Vector2 _tailleHitboxSol = new Vector2(0.45f, 0.25f);  // variable qui sert à déplacé le gizmo et la hitbox pour le saut
    
    [SerializeField] Planche _planche; // variable pour le script Planche (Mathieu Chavigny) #tp4

    [Header("Variable pour sauter")]
    [SerializeField] LayerMask _layerMask;  // champ serialiser utilisé pour le layer du sol
    public bool _estAuSol = false;  // vérifie si le joueur est au sol
    float _forceSaut = 90f;    // champ pour la puissance des saut effectuer du personnage
    int _nbFramesMax = 10; // champ pour le nombre de frame max pour la durer du saut
    int _nbFramesRestants = 0;  // champ pour le nombre de frame restant du saut
    bool _veutSauter = false;
    

    

    [Header("Particules")]
    [SerializeField] ParticleSystem _particuleInvisible; // Definit le ParticleSystem de l'invisibilite (Alex Guilbault)
    [SerializeField] ParticleSystem _particuleRage; // Definit le ParticleSystem de Rage (Alex Guilbault)

    [Header("Scripts")]
    Animator _anim; // variable pour l'Animator (Mathieu Chavigny) #tp4
    Arc _arc; // accède au script arc Alex Guilbault


    

    [Header("Mecaniques combats")]
    bool _peutJouerSonChute = true;

    [SerializeField] SOAmelioration _objetArc; // variable pour l'objet arc 
    public float _rayonAttaque = 1f; // variable pour le rayon d'attaque de l'epee #tpsynthese (Alex Guilbault)
    public float _posAttaque = 0.5f; // variable pour la position de l'attaque #tpsynthese (Alex Guilbault)
    public int _degatAttaque = 10; // variable pour le degat de l'eppe  #tpsynthese (Alex Guilbault)
    [SerializeField] LayerMask _layerEnnemi; // variable pour le layer de l'enemi #tpsynthese (Alex Guilbault)
    bool _modeRage = false; // boolen pour savoir si le joueur a activer la rage #tpsynthese (Alex Guilbault)
    bool _peutAttaquer = true; // bool pour savoir si le joueur peut attaquer #tpsynthese (Alex Guilbault)

    public bool _estInvisible = false; // variable pour l'invisibilite (Alex Guilbault) 

    bool _peutSubirDegat = true; // variable pour les degats (Alex Guilbault)

    [Header("Audio")]
    [SerializeField] AudioClip _sonAttaque; // variable pour les attaques #tpsynthese (Alex Guilbault)
    [SerializeField] AudioClip _sonDegatJoueur; // variable pour les attaques #tpsynthese (Alex Guilbault)
    [SerializeField] AudioClip _sonChute; // variable pour le son de chute
    [SerializeField] AudioClip _sonRage; // variable pour les attaques #tpsynthese (Alex Guilbault)
    [SerializeField] AudioClip _sonMort; // variable pour les attaques #tpsynthese (Alex Guilbault)
    Vector2 _forceDegat = new Vector2(5, 3); // variable pour la force de recul lors du degat du joueur#tpsynthese (Alex Guilbault)


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _anim = GetComponent<Animator>(); // attribut l'Animator au champ
        _particuleInvisible.Stop(); //arrete l'effet d'invisibilite#tpsynthese (Alex Guilbault)
        _particuleRage.Stop(); //arrete l'effet de rage(Alex Guilbault)
        donneesPerso.tempsRestant = 300; //temps de la partie(Alex Guilbault)
        StartCoroutine(CoroutineTempsPartie()); //demarre le timer de la partie(Alex Guilbault)
        _rb = GetComponent<Rigidbody2D>(); // attribut le rigidbody au champ
        _sr = GetComponent<SpriteRenderer>(); // attribut le SpriteRenderer au champ
        VerifierBotte();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        _veutSauter = Input.GetButton("Jump");  // attribut au champ le bouton `jump`
        _axeH = Input.GetAxis("Horizontal");    // attribut au champ l'axe horizontale
        _axeV = Input.GetAxis("Vertical");       // attribut au champ l'axe Verticale
        //le if et elseif suivant fait garder la dernière direction du sprite lors du déplacement et l'animation (Mathieu Chavigny) #tp4
        if (_axeH < 0) { _sr.flipX = true; _anim.SetFloat("vitesseX", _axeH); }
        else if (_axeH > 0) { _sr.flipX = false; _anim.SetFloat("vitesseX", _axeH); }
        else { _anim.SetFloat("vitesseX", 0); }

        if (_rb.velocity.y == 0 && _peutJouerSonChute == true && _estAuSol == true) //si le joueur est au sol et qu'il peut jouer le son de chute (Alex Guilbault
        {
            GestSonore.instance.JouerEffetSonore(_sonChute); //joue le son de chute (Alex Guilbault)
            _peutJouerSonChute = false;
        }


        if (Input.GetKeyDown("q")) //Si Q est enfoncer (Alex Guilbault)
        {
            if (VerifierArc()) // si le joueur possede l'arc(Alex Guilbault)
            {
                ChangerArme(); // change l'arme equiper(Alex Guilbault)
                _planche.ChangerPlanche();
            }
        }
        //condition qui gère la potion de vie (Mathieu Chavigny #tpSynthese)
        if (Input.GetKeyDown("f")) //Si f est enfoncer (Mathieu Chavigny #tpSynthese)
        {
            if (VerifierPotionVie()) // si le joueur possede la potion(Mathieu Chavigny #tpSynthese)
            {
                _donneesPerso.vieJoueur = _donneesPerso.vieIni; // redonne la vie au perosnnage(Mathieu Chavigny #tpSynthese)
                _planche.ChangerPlanche(); 
            }
        }
        //condition qui gère la potion de rage (Alex Guilbault)
        if (Input.GetKeyDown("c")) //Si c est enfoncer (Alex Guilbault)
        {
            if (VerifierPotionRage()) // si le joueur possede l'arc(Alex Guilbault)
            {
                _donneesPerso.rage += 25; // change l'arme equiper(Alex Guilbault)
                _planche.ChangerPlanche();
            }
        }
        //condition qui gère l'effet de rage (Mathieu Chavigny) 
        if (Input.GetKeyDown("r")) //Si r est enfoncer 
        {
            if (_donneesPerso.rage == 100) // si le joueur a 100 de rage
            {
                ActiverEffetRage();// active l'effet de rage
            }
        }

        //condition qui gère le saut du personnage (Mathieu Chavigny) #tp4
        if (_estAuSol == false)
        {
            _anim.SetFloat("vitesseY", _rb.velocity.y);
        }
        else
        {
            _anim.SetFloat("vitesseY", 0);
        }

        if (Input.GetMouseButtonDown(0)) // si le joueur clique gauche (Alex Guilbault)
        {

            Debug.Log($"{_donneesPerso._armeActifArc} {_peutAttaquer}");
            if (_donneesPerso._armeActifArc == false && _peutAttaquer == true && _estAuSol == true)
            {
                FrapperEpee();
            }

        }
        /// <summary>
        /// touches pour aider au debuggage
        /// ou pour tricher :D
        /// </summary>
        /// <returns></returns>
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            DegatJoueur(); // fonction qui fait subir des degats au joueurs
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            _donneesPerso.lesAmeliorations.Add(_objetArc);
            Debug.Log("/give 1 objetArc");
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            _donneesPerso.rage = 100;
            Debug.Log("/give 100 RAGE");
        }
    }
    /// <summary>
    ///  fonction pour lorsque le joueur frappe a l'epee
    /// </summary>
    void FrapperEpee()
    {
        _peutAttaquer = false;
        GestSonore.instance.JouerEffetSonore(_sonAttaque);
        _anim.SetTrigger("attaque"); //joue l'animation d'attaque   #tpsynthese (Alex Guilbault)
        //donne un tableau de collider de colision et creer un colider en cercle #tpsynthese (Alex Guilbault)
        Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + _axeH, transform.position.y), _rayonAttaque, _layerEnnemi); 
        foreach (Collider2D hit in hits) // boucle qui verifie tout les coups #tpsynthese (Alex Guilbault)
        {
            Boss boss = hit.GetComponent<Boss>() ; // assigne le script du boss #tpsynthese (Alex Guilbault)
            Enemie ennemi = hit.GetComponent<Enemie>(); // assigne le script de l'ennemi #tpsynthese (Alex Guilbault)

            if (_modeRage == true) { _degatAttaque = 20; } // condition qui verifie si le joueur est en mode rage et monte ses degat au besoin #tpsynthese (Alex Guilbault)
            else { _degatAttaque = 10; }

            if (ennemi != null) 
            {
                ennemi.SubirDegat(_degatAttaque); // fait subir les degats a l'ennemi #tpsynthese (Alex Guilbault)
            }
            else if (boss != null)
            {
                boss.SubirDegat(_degatAttaque); // fait subir les degats au boss #tpsynthese (Alex Guilbault)
            }
        }
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + _axeH, transform.position.y), _rayonAttaque);
    }
    /// <summary>
    /// donne droit d'attaquer au joueur #tpsynthese (Alex Guilbault)
    /// </summary>
    public void FinAttaque()
    {
        _peutAttaquer = true;
    }


    /// <summary>
    /// Alex Guilbault
    /// fonction qui cause le changement d'arme avec l'arc #tp3
    /// </summary>
    private void ChangerArme()
    {
        _donneesPerso._armeActifArc = !_donneesPerso._armeActifArc; // active/desactive le bool sur si le arc est actif(Alex Guilbault)

        _arc = GetComponentInChildren<Arc>(); //definit le script de Arc (Alex Guilbault)
        _arc.ActiverArme(); //appel la fonction ChangerArme() dans arc(Alex Guilbault)
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        _rb.velocity = new Vector2(_axeH * _vitesse, _rb.velocity.y); //déplace le personnage


        VerifierSol(); //appelle la méthode VerifierSol
        SauterJoueur(); //appelle la méthode SauterJoueur
    }

    /// <summary>
    ///   une méthode a appeler lorsque le joueur veut sauter
    /// </summary>
    void SauterJoueur()
    {

        if (_estAuSol)//si le personnage est au sol
        {

            if (_veutSauter) //si le personnage est au sol ET qu'il veut sauter
            {
                CalculerForceSaut(); //appelle la méthode CalculerForceSaut
            }
            else { _nbFramesRestants = _nbFramesMax; }//réinitialise le _nbFramesRestants
        }
        else //si le personnage n'est pas au sol
        {
            bool peutSauterPlus = _nbFramesRestants > 0;  // verifie si le joueur a encore de frames restant
            if (_veutSauter && peutSauterPlus) //si le personnage n'est pas au sol, qu'il veut sauter et qu'il peut sauter plus
            {
                CalculerForceSaut(); //appelle la méthode CalculerForceSaut
            }
            else _nbFramesRestants = 0; // garde les frames à 0
        }
    }
    /// <summary>
    /// une méthode a appeler pour calculé la puissance pour les saut
    /// </summary>
    void CalculerForceSaut()
    {

        float fractionDeForce = (float)_nbFramesRestants / _nbFramesMax; //réduit la puissance du saut si il est maintenu
        float puissance = _forceSaut * fractionDeForce; // établie la puissance du saut
        _rb.AddForce(Vector2.up * puissance);  //fait sauter le personnage
        _nbFramesRestants--; //réduire le nombre de frames restantes au saut
        if (_nbFramesRestants < 0) _nbFramesRestants = 0; // si le nombre de framesRestant va dans le négatif, garder le nb à 0
    }

    /// <summary>
    ///   une méthode a appeler pour faire la détection entre le personnage et le sol
    /// </summary>
    void VerifierSol()
    {
        Vector2 pointDepart = (Vector2)transform.position - _distanceDebutSol; //calcul les coordonné de la hitbox en dessous du personnage
        Collider2D col = Physics2D.OverlapBox(pointDepart, _tailleHitboxSol, 0, _layerMask); // initialise la hitbox
        _estAuSol = col != null; //vérifie si il touche au sol
        if (_estAuSol) //si il touche au sol
        {
            _anim.SetBool("estAuSol", true); // active l'animation au sol
        }
        else if (Application.isPlaying)
        { _anim.SetBool("estAuSol", false); _peutJouerSonChute = true; } //si il touche pas au sol désactive l'animation au sol   
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
        {  //si le jeu est en train de jouer
            VerifierSol(); ///appelle la méthode VerifierSol
        }
        if (_estAuSol)
        { //si il touche au sol
            Gizmos.color = Color.green; //le gizmos sera vert
        }
        else { Gizmos.color = Color.red; } //si il touche pas au sol le gizmos sera rouge

        Vector2 pointDepart = (Vector2)transform.position - _distanceDebutSol; //calcul les coordonné de la hitbox en dessous du personnage
        Gizmos.DrawWireCube(pointDepart, _tailleHitboxSol); //dessine dans la scene les bordures carré rouge
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineTempsPartie()
    {
        for (int i = donneesPerso.tempsRestant; donneesPerso.tempsRestant >= 0; donneesPerso.tempsRestant--) // boucle qui à chaque seconde diminue le temps de 1 (Alex Guilbault)
        {
            _planche.ChangerPlanche();
            if (donneesPerso.tempsRestant <= 100)
            {
                GestSonore.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenA, true);
            }
            if (donneesPerso.tempsRestant <= 0) // si le temps atteint 0
            {
                _soNavigation.AllerPartieTeminer(); //change la scène pour la scène de fin (Alex Guilbault)
                StopCoroutine(CoroutineTempsPartie()); //arrête la coroutine du chronometre de la partie (timer) (Alex Guilbault)
            }
            yield return new WaitForSeconds(1f); //attend 1 seconde (Alex Guilbault)

        }
    }

    /// <summary>
    /// fonction qui permet au personnage de prendre les objets de map et de les mettre dans son inventaire (Mathieu Chavigny)
    /// </summary>
    /// <param name="donneesObjet"></param>
    public void Obtenir(SOObjetMap donneesObjet)
    {
        _donneesPerso.lesObjets.Add(donneesObjet);
    }
    /// <summary>
    /// fonction qui permet au personnage de mettre ses joyaux dans son inventaire (Mathieu Chavigny)
    /// </summary>
    /// <param name="donneesJoyau"></param>
    public void EconomiserJoyaux(SOJoyau donneesJoyau)
    {
        _planche.ChangerPlanche();
        _donneesPerso.CompterJoyaux();
    }
    /// <summary>
    /// fonction qui permet au personnage de Compter le nombre d'ennemi decapiter (Alex Guilbault) #tp4
    /// </summary>
    public void RajouterEnnemi(SOPerso _donneesPerso)
    {
        _donneesPerso.CompterEnnemi();
    }

    /// <summary>
    /// fonction qui permet au personnage de mettre ses ferailles dans son inventaire (Mathieu Chavigny)
    /// </summary>
    /// <param name="donneesFeraille"></param>
    public void EconomiserFeraille(SOFeraille donneesFeraille)
    {
        _planche.ChangerPlanche();
        _donneesPerso.CompterFeraille();
    }
    /// <summary>
    /// fonction qui permet au personnage de savoir s'il possede la cle pour franchir la porte
    /// retourne une valeur booleene (Mathieu Chavigny)
    /// </summary>
    /// <returns></returns>
    public bool VerifierCle()
    {
        foreach (SOObjetMap objetMap in _donneesPerso.lesObjets)
        {
            if (objetMap.nom == "Clé") //cherche dans toute la liste d'objet de map (Mathieu Chavigny)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Alex Guilbault
    /// fonction qui verifie si le perso à l'arc (Alex Guilbault)
    /// </summary>
    /// <returns></returns>
    public bool VerifierArc()
    {
        foreach (SOAmelioration amelioration in _donneesPerso.lesAmeliorations)
        {
            if (amelioration.nom == "Arc")
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// fonction qui permet au personnage de savoir s'il possede la potion de vie Mathieu Chavigny #tpSynthese
    /// </summary>
    /// <returns></returns>
    public bool VerifierPotionVie()
    {
        foreach (SOPotion potion in _donneesPerso.lesPotions)
        {
            if (potion.nom == "Vie")
            {
                return true;
            }
            
        }
        return false;
    }
    /// <summary>
    /// fonction qui permet au personnage de savoir s'il possede la potion de rage Mathieu Chavigny #tpSynthese
    /// </summary>
    /// <returns></returns>
    public bool VerifierPotionRage()
    {
        foreach (SOPotion potion in _donneesPerso.lesPotions)
        {
            
            if(potion.nom == "Rage")
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// fonction qui permet au personnage de savoir s'il possede les botte et modifie sa vitesse (Mathieu Chavigny)
    /// </summary>
    public void VerifierBotte()
    {
        foreach (SOAmelioration amelioration in _donneesPerso.lesAmeliorations) //cherche dans toute la liste d'amelioration (Mathieu Chavigny)
        {
            if (amelioration.nom == "Bottes") //si il y a une amelioration qui s'appelle botte (Mathieu Chavigny)
            {
                _vitesse = _vitesse * 2;
                break;
            }
        }
    }
    /// <summary>
    /// /// methoque que lorsque l'on quitte l'application les inventaires sont reinitialiser (Mathieu Chavigny)
    /// </summary>
    void OnApplicationQuit()
    {
        _donneesPerso.Initialiser();
        _donneesPerso.lesObjets.Clear();
        _donneesPerso.lesAmeliorations.Clear();
        _donneesPerso.lesPotions.Clear();
        _donneesPerso.niveau = _donneesPerso.niveauIni;
        _planche.ChangerPlanche();
    }
    /// <summary>
    /// Alex Guilbault
    /// méthode qui fait diminue de 1 fleche
    /// </summary>
    public void DiminueFleche()
    {
        _donneesPerso.fleche -= 1;
    }
    /// <summary>
    /// Alex Guilbault
    /// méthode qui active l'effet le bonus d'invisibilité
    /// </summary>
    public void ActiverEffetInvisible()
    {
        _particuleInvisible.Play();
        _estInvisible = true;
    }
    /// <summary>
    /// Alex Guilbault
    /// méthode qui désactive l'effet le bonus d'invisibilité
    /// </summary>
    public void DesactiverEffetInvisible()
    {
        _particuleInvisible.Stop();
        _estInvisible = false;
    }
    /// <summary>
    /// Alex Guilbault
    /// méthode qui activé l'effet le bonus de rage
    /// </summary>

    public void ActiverEffetRage()
    {
        _peutSubirDegat = false;
        _particuleRage.Play();
        _sr.color = Color.red;
        StartCoroutine(CoroutineTempsRage());
        _modeRage = true;
        GestSonore.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenB, true);
        GestSonore.instance.JouerEffetSonore(_sonRage);

    }
    /// <summary>
    /// Alex Guilbault
    /// méthode qui desactive l'effet le bonus de rage
    /// </summary>
    public void DesactiverEffetRage()
    {
        _peutSubirDegat = true;
        StopCoroutine(CoroutineTempsRage());
        _particuleRage.Stop();
        _sr.color = Color.white;
        _modeRage = false;
        GestSonore.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenB, false);
    }
    /// <summary>
    /// Coroutine pour la diminution de la rage
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineTempsRage()
    {
        int tempsRage = 20;
        for (int i = 0; i < tempsRage; i++) // boucle qui fait diminuer la rage petit a petit
        {
            Debug.Log(tempsRage);
            _donneesPerso.rage -= 5;
            if (_donneesPerso.rage <= 0) // si fini
            {
                DesactiverEffetRage(); // desactive la rage
            }
            yield return new WaitForSeconds(1f);
        }
        
    }
    /// <summary>
    /// fonction appeller dans l'animation pour faire joueur le son
    /// </summary>
    /// <param name="sonSaut"></param>
    public void SonSaut(AudioClip sonSaut)
    {
        GestSonore.instance.JouerEffetSonore(sonSaut);
    }

    /// <summary>
    /// fonction qui fait subir des degats au joueur
    /// </summary>
    public void DegatJoueur()
    {
        if (_peutSubirDegat == true) // verifie si il n'est pas invincible #tpsynthese (Alex Guilbault)
        {
            
            if (_estAuSol == true) // si il est au sol #tpsynthese (Alex Guilbault)
            {
                _rb.AddForce(_forceDegat, ForceMode2D.Impulse); // si oui , donne du recul lorsque qu'il subit des degats #tpsynthese (Alex Guilbault)
            }
            _donneesPerso.vieJoueur -= 1; //diminue la vie #tpSynthese (Alex Guilbault)
            _peutSubirDegat = false;
            StartCoroutine(CoroutineTempsInvincible()); // rend invincible #tpSynthese (Alex Guilbault)
            _planche.ChangerPlanche(); // update la planche du UI #tpSynthese (Alex Guilbault)
            if (_donneesPerso.vieJoueur <= 0) // si plus vie #tpSynthese (Alex Guilbault)
            {
                GestSonore.instance.JouerEffetSonore(_sonMort); // joue son de mort #tpSynthese (Alex Guilbault)
                _soNavigation.AllerPartieTeminer(); // deplace vers partie terminer #tpSynthese (Alex Guilbault)
            }
            else //sinon ...
            {
                GestSonore.instance.JouerEffetSonore(_sonDegatJoueur); // joue son de degat  #tpSynthese (Alex Guilbault)
            }
        }

    }

    /// <summary>
    ///  fonction de lorsque le joueur se fait toucher #tpSynthese (Alex Guilbault)
    ///  rend invincible pendant quelque secondes
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineTempsInvincible()
    {
        int nbFoisClignote = 5; // nombre de fois qu'il refait la boucle #tpSynthese (Alex Guilbault)
        for (int i = 0; i < nbFoisClignote; i++) // boucle qui fait clingnoter le joueur #tpSynthese (Alex Guilbault)
        {
            _sr.color = new Color(1, 1, 1, 0.25f); 
            yield return new WaitForSeconds(0.25f);
            _sr.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.25f);
        }

        _peutSubirDegat = true; // rend le joueur toucheable #tpSynthese (Alex Guilbault)
    }

}