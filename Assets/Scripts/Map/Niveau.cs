using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
/// <summary>
/// Classe qui controle le niveau
/// Auteurs : Mathieu Chavigny ET Alex Guilbault 
/// Auteur des commentaires: Mathieu Chavigny ET Alex Guilbault
/// </summary>
public class Niveau : MonoBehaviour
{



    [SerializeField] SONiveau _donneesNiveau;


    [SerializeField] Tilemap _tilemap;  //initialisation d'un champ pour obtenir une tilemap

    public Tilemap tilemap => _tilemap;  //accesseur qui initialise le champ comme public pour obtenir une tilemap


    [SerializeField] Salle[] _tSalleModele; //initialisation d'un tableau _tSalleModele avec nos prefabs des salles
    [SerializeField] TileBase _tuileModele; //initialisation d'une tuile qui va être utilisé pour les bordures
    [SerializeField] GameObject _cleModele; //champs pour lier prefab de la cle (Mathieu Chavigny)
    [SerializeField] GameObject _joyauModele; //champs pour lier prefab du joyau (Mathieu Chavigny)
    [SerializeField] GameObject _ferailleModele; //champs pour lier prefab de la feraille (Mathieu Chavigny)

    [SerializeField] int _probApparitionBonus = 50; //champs pour la probabilite d'apparition des bonus (Alex Guilbault)
    private int _nbJoyauxParNiveau = 10; //initialisation de la quantite de joyaux (Mathieu Chavigny)
    private int _nbFeraillesParNiveau = 10; //initialisation de la quantite de feraille (Mathieu Chavigny)

    [SerializeField] GameObject _porteModele; //champs pour lier prefab de la porte (Mathieu Chavigny)
    [SerializeField] GameObject _activateurModele; //champs pour le prefab du gameObject de l'activateur (Alex Guilbault)
    [SerializeField] GameObject _effectorModele; //champs pour le prefab du gameObject de l'effector (Alex Guilbault)
    [SerializeField] public GameObject _conteneurEffector; //champs pour lier gameobject conteneur d'effector (Alex Guilbault)
    [SerializeField] GameObject _conteneurJoyaux; //champs pour lier gameobject conteneur de joyau (Mathieu Chavigny)
    [SerializeField] GameObject _conteneurFeraille; //champs pour lier gameobject conteneur de feraille (Mathieu Chavigny)

    [SerializeField] GameObject _chronoBonusModele;  //champs pour lier le gameobject du chronometre (Alex Guilbault)
    [SerializeField] GameObject _rageBonusModele; //champs pour lier le gameobject du bonus de rage (Alex Guilbault)
    [SerializeField] GameObject _invisibiliteBonusModele; //champs pour lier le gameobject du bonus invisibilite (Alex Guilbault)



    BoundsInt _bounds;  //champs pour limite de la tilemap (Alex Guilbault)


    List<Vector2Int> _lesPosLibres = new List<Vector2Int>();  // liste qui fait l'inventaire des position disponible de la tilemap (Alex Guilbault)
    public List<Vector2Int> _lesPosSurReperes = new List<Vector2Int>(); // liste pour les positions des repere des salles(Alex Guilbault)
    [SerializeField] private Tilemap _tmCible; // champs serialise qui donne la tilemap

    [SerializeField] GameObject _persoModele; //champs pour lier prefab du personnage (Mathieu Chavigny)

    public Tilemap tmCible { get => _tmCible; set => _tmCible = value; }

    /// 
    /// <summary>
    /// fonction dans laquelle on détermine les position des salles
    /// fonction dans laquelle on détermine les salles utilisé
    /// fonction dans laquelle on fait apparaitre les salles et les renomme
    /// fonction dans laquelle on trouve la largeur et hauteur du niveau
    /// fonction dans laquelle on fait apparaitre les bordures
    /// </summary>
    /// /// Awake is called when the script instance is being loaded.
    void Awake()
    {

        //Initialisation d'un vecteur2Int afin de pouvoir faire apparaitre les salles selon la taille donné. dans le script salle (24) moins 1 
        // pour le chevauchement de celles-ci
        Vector2Int tailleAvecBordure = Salle.tailleAvecBordure - Vector2Int.one;
        tailleAvecBordure = CreerSalle(tailleAvecBordure);

        _bounds = _tilemap.cellBounds;

        TrouverPosLibres();

        //bordures
        CreerBordures(tailleAvecBordure);
        PlacementJoyaux(); //appel de la methode pour placer les joyaux (Mathieu Chavigny)
        PlacementFeraille(); //appel de la methode pour placer les feraille (Mathieu Chavigny)
        PlacementBonusChrono(); // appel de la fonction du placemnent du chronometre (Alex Guilbault)
        PlacementBonusInvisibite();  // appel de la fonction du placemnent du bonus d'invisibilité (Alex Guilbault)
        PlacementBonusRage();  // appel de la fonction du placemnent du bonus de rage (Alex Guilbault)
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        GestSonore.instance.ChangerEtatLecturePiste(TypePiste.musiqueBase, true); //appel de la fonction pour jouer la musique de base (Alex Guilbault)
        GestSonore.instance.ChangerEtatLecturePiste(TypePiste.musiqueMenu, false); //appel de la fonction pour arreter la musique du boss (Alex Guilbault)
    }

    /// <summary>
    /// fonction pour trouver les limite et creer les bordures extérieur du niveau
    /// (Alex Guilbault)
    /// </summary>
    /// <param name="tailleAvecBordure"></param>
    private void CreerBordures(Vector2Int tailleAvecBordure)
    {
        Vector2Int min = Vector2Int.zero - Salle.tailleAvecBordure / 2; //trouve la position le plus en bas à droite du niveau
        Vector2Int max = new Vector2Int(_donneesNiveau.taille.x * tailleAvecBordure.x, tailleAvecBordure.y * _donneesNiveau.taille.y) + min;  //trouve la position le plus en haut à gauche du niveau
        for (int y = min.y; y <= max.y; y++)
        {                                           //double boucles qui traverse toutes les position du niveau 
            for (int x = min.x; x <= max.x; x++)
            {
                if (x == min.x || x == max.x || y == max.y || y == min.y) // si elle la position correspond à un des 4 murs extérieur créer une bordure 
                {
                    Vector3Int pos = new Vector3Int(x, y, 0); // va chercher le x et y des boucles
                    _lesPosLibres.Remove((Vector2Int)pos);
                    tilemap.SetTile(pos, _tuileModele); // place la tuiles des bordure sur le niveau
                }
            }
        }
    }
    /// <summary>
    /// fonction pour trouver creer les salles et placer les objets uniques du niveau
    /// (Alex Guilbault)
    /// </summary>
    /// <param name="tailleAvecBordure"></param>
    /// <returns></returns>
    private Vector2Int CreerSalle(Vector2Int tailleAvecBordure)
    {
        // trouve les coordoné des objets uniques (Alex Guilbault)
        Vector2Int placementCle = new Vector2Int(_donneesNiveau.taille.x - 1, Random.Range(0, _donneesNiveau.taille.y));
        Vector2Int placementPorte = new Vector2Int(0, Random.Range(1, _donneesNiveau.taille.y));
        Vector2Int _placementJoueur = placementPorte - new Vector2Int(0, 1); //placement du joueur est relier au placement de la porte (Mathieu Chavigny)
        Vector2Int placementActivateur = new Vector2Int(Random.Range(1, _donneesNiveau.taille.x - 1), Random.Range(1, _donneesNiveau.taille.y));

        //place les salles (Alex Guilbault)
        for (int y = 0; y < _donneesNiveau.taille.y; y++)
        {                                           // double boucles qui place selon le x et y les salles
            for (int x = 0; x < _donneesNiveau.taille.x; x++)
            {
                Vector2Int placementSalle = new Vector2Int(x, y);
                Vector2Int placementEffector = placementSalle;

                int typeSalle = Random.Range(0, _tSalleModele.Length); // fait un random pour déterminer la salle à faire apparaitre
                Vector2 pos = new Vector2(x * tailleAvecBordure.x, y * tailleAvecBordure.y); //initialise la position de la salle
                Salle salle = Instantiate(_tSalleModele[typeSalle], pos, Quaternion.identity, transform); //fait apparraitre les salles
                salle.name = $"Salle_{x + 1}_{y + 1}"; //renomme la sallle selon les position x et y
                
                PlacerObjetUnique(placementCle, _placementJoueur, placementPorte, placementActivateur, placementSalle, placementEffector, salle); // appel de fonction pour  les objets uniques (Alex Guilbault)
                salle.PlacerEnnemiSurRepere();
                
            }
        }
        
        return tailleAvecBordure;
    }
    /// <summary>
    /// fonctions appeller lorsque nous voulons faire apparraitre la clé,la porte,l'activateur et le joueur
    /// (Alex Guilbault)
    /// </summary>
    /// <param name="placementCle"></param>
    /// <param name="placementJoueur"></param>
    /// <param name="placementPorte"></param>
    /// <param name="placementActivateur"></param>
    /// <param name="placementSalle"></param>
    /// <param name="placementEffector"></param>
    /// <param name="Salle"></param>
    private void PlacerObjetUnique(Vector2Int placementCle, Vector2Int placementJoueur, Vector2Int placementPorte, Vector2Int placementActivateur, Vector2Int placementSalle, Vector2Int placementEffector, Salle salle)
    {

        if (placementCle == placementSalle) //condition qui identifie dans quelle salle sera la cle (Mathieu Chavigny)
        {
            Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
            Vector2Int posRep = salle.PlacerCleSurRepere(_cleModele) - decalage;

        }
        if (placementPorte == placementSalle) //condition qui identifie dans quelle salle sera la porte (Mathieu Chavigny)
        {
            Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
            Vector2Int posRep = salle.PlacerPorteSurRepere(_porteModele) - decalage;

        }
        if (placementActivateur == placementSalle) //condition qui identifie dans quelle salle sera l'activateur (Alex Guilbault)
        {
            Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
            Vector2Int posRep = salle.PlacerActivateurSurRepere(_activateurModele) - decalage;

        }
        if (placementEffector == placementSalle) //condition qui identifie dans quelle salle sera l'effector (Alex Guilbault)
        {
            Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
            Vector2Int posRep = salle.PlacerEffectorSurRepere(_effectorModele) - decalage;

        }
        if (placementJoueur == placementSalle) //condition qui identifie dans quelle salle sera le personnage (Mathieu Chavigny)
        {
            Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
            Vector2Int posRep = salle.PlacerPersoSurRepere(_persoModele) - decalage;

        }
    }

    /// <summary>
    /// /// fonction qui reçoit les tuiles des tilemaps du script cartesTuiles et les écrit sur la tilemap du niveau
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="tuile"></param>
    /// <param name="decalage"></param>
    public void TraiterTuiles(Vector3Int pos, TileBase tuile, Vector3Int decalage)
    {
        tilemap.SetTile(pos + decalage, tuile); //place la tuile donné comme paramêtre à la position voulu selon son sa salle et s'assure que la tilemap soit au premier plan
    }

    /// <summary>
    /// fonction qui sert à iddentifier chaque tuile du niveau et de les mettre dans une liste
    /// fonction utilisé pour retiret les position utilisé par d'autre objets
    /// (Alex Guilbault)
    /// </summary>
    void TrouverPosLibres()
    {
        BoundsInt bornes = tmCible.cellBounds;
        for (int x = bornes.xMin; x < bornes.xMax; x++)
        {
            for (int y = bornes.yMin; y < bornes.yMax; y++)
            {
                Vector2Int posTuile = new Vector2Int(x, y);
                TileBase tuile = tmCible.GetTile((Vector3Int)posTuile);
                if (tuile == null)
                {
                    _lesPosLibres.Add(posTuile);
                }
            }
        }

        ///retire les tuiles déjà utilisé
        foreach (Vector2Int pos in _lesPosSurReperes)
        {
            _lesPosLibres.Remove(pos);
        }
    }

    /// <summary>
    /// fonction utilitaire pour l'obtention d'une position aléatoire selon la liste _lesPosLibres
    /// (Alex Guilbault)
    /// </summary>
    /// <returns></returns>
    public Vector2Int ObtenirPosLibre()
    {
        int indexPosLibre = Random.Range(0, _lesPosLibres.Count);
        Vector2Int pos = _lesPosLibres[indexPosLibre];
        _lesPosLibres.RemoveAt(indexPosLibre);
        return pos;
    }
    /// <summary>
    /// Fonction utilisé lors du placement du chrono
    /// (Alex Guilbault)
    /// </summary>
    void PlacementBonusChrono()
    {
        int chanceApparition = Random.Range(0, 101);
        //test les probabilité d'apparition (Alex Guilbault)
        if (chanceApparition >= _probApparitionBonus)
        {
            Vector2Int pos = ObtenirPosLibre();
            Vector3Int posPlacement = (Vector3Int)pos;
            Instantiate(_chronoBonusModele, posPlacement + tilemap.tileAnchor, Quaternion.identity, tilemap.transform);
        }
    }
    /// <summary>
    /// place les joyaux selon une probabilite d'apparition dans des cases libres de la tilemap du niveau
    /// </summary>
    void PlacementJoyaux()
    {
        for (int i = 0; i < _nbJoyauxParNiveau; i++) //boucle pour placer chaque joyau possible dans le niveau
        {
            int chanceApparition = Random.Range(0, 101);
            if (chanceApparition >= _probApparitionBonus) //si pprition est plus grand probabilite on place un joyau sur une position libre
            {
                Vector2Int pos = ObtenirPosLibre();
                Vector3Int posPlacement = (Vector3Int)pos;
                Instantiate(_joyauModele, posPlacement + tilemap.tileAnchor, Quaternion.identity, _conteneurJoyaux.transform);
            }
        }
    }
    /// <summary>
    /// place les ferailles selon une probabilite d'apparition dans des cases libres de la tilemap du niveau
    /// </summary>
    void PlacementFeraille()
    {
        for (int i = 0; i < _nbFeraillesParNiveau; i++) //boucle pour placer chaque feraille possible dans le niveau 
        {
            int chanceApparition = Random.Range(0, 101);
            if (chanceApparition >= _probApparitionBonus) //si pprition est plus grand probabilite on place une feraille sur une position libre
            {
                Vector2Int pos = ObtenirPosLibre();
                Vector3Int posPlacement = (Vector3Int)pos;
                Instantiate(_ferailleModele, posPlacement + tilemap.tileAnchor, Quaternion.identity, _conteneurFeraille.transform);
            }
        }

    }
    /// <summary>
    /// Fonction utilisé lors du placement du bonus d'invisibilité
    /// (Alex Guilbault)
    /// </summary>
    void PlacementBonusInvisibite()
    {
        //boucle qui place un nombre de bonus spécifique(Alex Guilbault)
        for (int i = 0; i < 2; i++)
        {
            int chanceApparition = Random.Range(0, 101);
            if (chanceApparition >= _probApparitionBonus) //test les probabilité d'apparition (Alex Guilbault)
            {
                Vector2Int pos = ObtenirPosLibre();
                Vector3Int posPlacement = (Vector3Int)pos;
                Instantiate(_invisibiliteBonusModele, posPlacement + tilemap.tileAnchor, Quaternion.identity, tilemap.transform);
            }

        }
    }
    /// <summary>
    /// Fonction utilisé lors du placement du bonus de rage
    /// (Alex Guilbault)
    /// </summary>
    void PlacementBonusRage()
    {
        //boucle qui place un nombre de bonus spécifique (Alex Guilbault)
        for (int i = 0; i < 3; i++) 
        {
            int chanceApparition = Random.Range(0, 101);
            if (chanceApparition >= _probApparitionBonus) //test les probabilité d'apparition (Alex Guilbault)
            {
                Vector2Int pos = ObtenirPosLibre();
                Vector3Int posPlacement = (Vector3Int)pos;
                Instantiate(_rageBonusModele, posPlacement + tilemap.tileAnchor, Quaternion.identity, tilemap.transform);
            }

        }
    }

    void OnApplicationQuit()
    {
        _donneesNiveau.taille = new Vector2Int(3, 3);
    }


}



