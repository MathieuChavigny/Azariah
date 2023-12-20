using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe qui controle fait le fonctionement de l'enigme
/// /// Auteur des commentaires: Alex Guilbault 
/// auuteur du code Alex Guilbault 
/// </summary>
public class Enigme : MonoBehaviour
{

    [SerializeField] Sprite[] _spriteEnigme; // champs pour les sprites des différentes égnimes  #tpSynthese (Alex Guilbault)
    [SerializeField] Levier[] _leviers; // champs pour les leviers de l'énigme  #tpSynthese (Alex Guilbault)

    SpriteRenderer _srEnigme; // variable pour le SpriteRenderer de l'énigme  #tpSynthese (Alex Guilbault)

    int _typeEnigme; // variable pour le bon levier  #tpSynthese (Alex Guilbault)

    [SerializeField] PorteBoss _porteBoss; // variable pour la porte du boss  #tpSynthese (Alex Guilbault)

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        _srEnigme = GetComponent<SpriteRenderer>(); // definie le SpriteRenderer  #tpSynthese (Alex Guilbault)


        _typeEnigme = Random.Range(0, 3); // definie le bon levier  #tpSynthese (Alex Guilbault)
        Debug.Log($"le bon levier est : {_typeEnigme}");
        _leviers[_typeEnigme].LevierPorte(); // active le bon levier  #tpSynthese (Alex Guilbault)
        _srEnigme.sprite = _spriteEnigme[_typeEnigme]; // definie le sprite de l'énigme  #tpSynthese (Alex Guilbault)
        foreach (Levier levier in _leviers) // active les mauvais leviers  #tpSynthese (Alex Guilbault)
        {
            if (levier != _leviers[_typeEnigme])
            {
                levier.LevierMaudit();
            }
        }
    }

    /// <summary>
    ///  fonction pour activer
    /// </summary>
    public void ActiverPorte() // active la porte du boss
    {
        _porteBoss.ActiverPorte();
    }
}
