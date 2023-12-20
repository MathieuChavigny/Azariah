using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui controle les sons et musique du jeu
/// /// Auteur des commentaires: Alex Guilbault 
/// auuteur du code Alex Guilbault 
/// </summary>
public class GestSonore : MonoBehaviour
{
    [Header("hauteur des pitch")]
    [SerializeField] float _pitchGrave = 0.8f;  // pitch grave pour les effetSonore #tpSynthese (Alex Guilbault)
    [SerializeField] float _pitchAigu = 1.2f; // pitch aigu pour les effetsSonore #tpSynthese (Alex Guilbault)
    [Header("acesseur des pistes")]
    [SerializeField] SOPiste[] _tPistes; // tableau pour les scriptable objets des différentes musiques #tpSynthese (Alex Guilbault)
    public SOPiste[] tPistes => _tPistes; // acesseur des pistes de musiques #tpSynthese (Alex Guilbault)

    [Header("Singleton du gestionnaire")]
    static GestSonore _instance; // instance Gestionnaire Sonore #tpSynthese (Alex Guilbault)
    static public GestSonore instance => _instance; //acceseur Singleton du Gestionnaire Sonore #tpSynthese (Alex Guilbault)
    [Header("acesseur du volume")]
    [SerializeField, Range(0, 1)] float _volumeMusiqueMaxRef = 0.7f; // volume de la musique de base #tpSynthese (Alex Guilbault)
    public float volumeMusicalRef => _volumeMusiqueMaxRef;  // acesseur du volume de la musique #tpSynthese (Alex Guilbault)
    [Header("AudioSource ")]
    public AudioSource _audioFx; // audioSource pour les effets sonore #tpSynthese (Alex Guilbault)
 
    /// <summary>
    /// Awake is called when the script instance is being loaded. #tpSynthese (Alex Guilbault)
    /// </summary>
    void Awake()
    {
        if (_instance == null) _instance = this; // regarde si il y a une instance du gestionnaire déjà présente #tpSynthese (Alex Guilbault)
        else // si oui , détruit le deuxième
        {
            Debug.Log("Un gestionnaire audio existe déjà, donc celui sur la scène sera détruit");
            Destroy(gameObject); 
            return;
        }
        _audioFx = gameObject.AddComponent<AudioSource>(); // assigne l'audioSource #tpSynthese (Alex Guilbault)
        CreerPisteMusical(); // créer les différentes audioSource pour les pistes musical #tpSynthese (Alex Guilbault)
        DontDestroyOnLoad(gameObject); // empêche de détruire le gestionnaire #tpSynthese (Alex Guilbault)
        foreach (var piste in _tPistes) // initie le volumes des pistes #tpSynthese (Alex Guilbault)
        {
            ChangerEtatLecturePiste(piste.type, piste.estActif); //appel de ChangerEtatLecturePiste #tpSynthese (Alex Guilbault)
        }
    }
    
    /// <summary>
    /// fonction qui créer les pistes musicals
    /// </summary>
    private void CreerPisteMusical()
    {
        foreach (SOPiste piste in _tPistes)
        {
            piste.Initialiser(gameObject.AddComponent<AudioSource>()); // appel la fonction initialisé dans SOpiste #tpSynthese (Alex Guilbault)

        }
    }

    /// <summary>
    /// fonction qui permet de faire jouer les effetsSonores #tpSynthese (Alex Guilbault)
    /// fonction qui change le pitch du Fx 
    /// </summary>
    /// <param name="clip"></param>
    public void JouerEffetSonore(AudioClip clip)
    {
        float pitch = Random.Range(_pitchGrave, _pitchAigu);
        _audioFx.pitch = pitch;
        _audioFx.PlayOneShot(clip);
    }
    /// <summary>
    /// fonction qui change le pitch de l'audioSource  #tpSynthese (Alex Guilbault)
    /// </summary>
    /// <param name="pitch"></param>
    public void ChangerPitch(float pitch)
    {
        foreach (SOPiste piste in _tPistes)
        {

            piste.audio.pitch = pitch;

        }
    }
    
    /// <summary>
    /// Coroutine qui permet de faire un fondue du volume de la piste voulue  #tpSynthese (Alex Guilbault)
    /// </summary>
    /// <param name="piste"></param>
    /// <param name="volumeCible"></param>
    /// <param name="duree"></param>
    /// <returns></returns>
    IEnumerator CoroutineChangerVolume(SOPiste piste, float volumeCible, float duree)
    {

        float volumeFinal = volumeCible;
        float volumeInitial = piste.audio.volume;
        float tempsInitial = Time.time;
        float tempsFinal = tempsInitial + duree;
        float tempsActuel = tempsInitial;
        while (tempsActuel < tempsFinal)
        {
            
            tempsActuel = Time.time;
            float progression = (tempsActuel - tempsInitial) / duree;
            piste.audio.volume = Mathf.Lerp(volumeInitial, volumeFinal, progression);
            yield return null;
        }
        piste.audio.volume = volumeFinal;

    }

    /// <summary>
    /// Fonction qui active les pistes voulue en renvoyant vers CoroutineChangerVolume  #tpSynthese (Alex Guilbault)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="estActif"></param>
    public void ChangerEtatLecturePiste(TypePiste type, bool estActif)
    {
        foreach (SOPiste piste in _tPistes)
        {
            if (piste.type == type)  // regarde si la piste correspond avec la piste voulue  #tpSynthese (Alex Guilbault)
            {
                piste.estActif = estActif;
                if (piste.estActif) //si oui , active la piste et monte le son  #tpSynthese (Alex Guilbault)
                {
                    if (piste.coroutineFondu != null) StopCoroutine(piste.coroutineFondu);
                    piste.coroutineFondu = StartCoroutine(CoroutineChangerVolume(piste, _volumeMusiqueMaxRef, 1f));
                }
                else //si oui , desactive la piste et descend le son  #tpSynthese (Alex Guilbault)
                {
                    if (piste.coroutineFondu != null)
                    {
                        StopCoroutine(piste.coroutineFondu);
                    }
                    piste.coroutineFondu = StartCoroutine(CoroutineChangerVolume(piste, 0f, 1f));
                }
            }

        }
    }
}


