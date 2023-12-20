using System;
using UnityEngine;

/// <summary>
/// Classe qui controle Pistes de musique
/// /// Auteur des commentaires: Alex Guilbault 
/// auuteur du code Alex Guilbault 
/// </summary>
[CreateAssetMenu(menuName = "Piste musicale", fileName = "DonneesPiste")]
public class SOPiste : ScriptableObject
{

    [SerializeField] public TypePiste _type; //le type de piste #tp4 (Alex Guilbault) 
    [SerializeField] AudioClip _clip; //le clip à jouer #tp4 (Alex Guilbault)
    [SerializeField] public bool _estActifIni;  //permet de choisir l'état initial #tp4 (Alex Guilbault)

    public TypePiste type => _type; //permet de récupérer le type de piste #tp4 (Alex Guilbault)
    public AudioClip clip => _clip; //permet de récupérer le clip #tp4 (Alex Guilbault)
    private Coroutine _coroutineFondu; //permet de récupérer la coroutine #tp4 (Alex Guilbault)
    public Coroutine coroutineFondu //permet de récupérer la coroutine #tp4 (Alex Guilbault)
    {
        get => _coroutineFondu;
        set
        {
            _coroutineFondu = value;
        }
    }

    [SerializeField] bool _estActif; //c'est l'état actuel #tp4 (Alex Guilbault)
    public bool estActif //permet de récupérer l'état actuel #tp4 (Alex Guilbault)
    {
        get => _estActif;
        set
        {
            _estActif = value;
        }
    }

    AudioSource _audio;
    public AudioSource audio // permet de récupérer l'audio source #tp4 (Alex Guilbault)
    {
        get => _audio;
        set => _audio = value;

    }


    /// <summary>
    /// fonction qui configure les audio sources #tp4 (Alex Guilbault)
    /// </summary>
    /// <param name="source"></param>
    public void Initialiser(AudioSource source)
    {
        _audio = source;
        _audio.volume = 0f;
        _audio.clip = _clip;
        _audio.loop = true;
        estActif = _estActifIni;
        _audio.playOnAwake = false;
        _audio.Play();
    }

    public void AjusterVolume() // ajuste le volumes des pistes   #tp4 (Alex Guilbault)
    {
        if (estActif)
        {
            _audio.volume = GestSonore.instance.volumeMusicalRef;
        }
        else
        {
            _audio.volume = 0;
        }
    }
}
