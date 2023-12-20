using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui gère le ciblage du boss
/// Auteur du code : Mathieu Chavigny #tpSynthese
/// </summary>
public class CiblageBoss : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject _player; //reference au joueur
    [SerializeField] GameObject _projectile ; //reference au projectile
    [SerializeField] private GameObject _tete; //reference à la tete du boss
    [SerializeField] private Transform _bouche; //reference à la bouche du boss
    Boss _boss; //reference au boss
    
    [Header("InfoPerso")]
    private Vector3 _targetPos; //position du joueur
    private float _angle; //angle entre le boss et le joueur

    [Header("Audio")]
    private AudioSource _audio; //reference à l'audio source
    [SerializeField] private AudioClip _sonAttaque; //son de l'attaque



    /// <summary>
    /// Initialise les composants du ciblage et démarre la coroutine de tir
    /// </summary>
    private void Awake()
    {
        _boss = GetComponentInParent<Boss>();
        _audio = GetComponent<AudioSource>();
        StartCoroutine(CoroutineTirBouleFeu());
    }


    /// <summary>
    /// Cherche l'anle entre le boss et le joueur et le fait tourner vers le joueur
    /// </summary>
    void Update()
    {
        // code qui vise le joueur 
        _targetPos = _player.transform.position;
        _angle = TrouverAngle(transform.position, _targetPos);
        transform.rotation = Quaternion.Euler(0, 0, _angle * Mathf.Rad2Deg);

    }

    /// <summary>
    /// Trouve l'angle entre deux vecteurs
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
    /// Fait apparaitre le projectile et joue le son de l'attaque
    /// </summary>
    private void TirBouleFeu()
    {
        _boss.anim.SetTrigger("attaque");
        GestSonore.instance.JouerEffetSonore(_sonAttaque);
        //fait apparaitre le projectile
        GameObject go = Instantiate(_projectile, _bouche.position, _tete.transform.rotation);
        go.GetComponent<Projectile>().Angle(new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0));

    }

    /// <summary>
    /// Coroutine qui gère la frequence de tir de boule de feu
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineTirBouleFeu()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            TirBouleFeu();
        }
    }


}
