using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  classe qui s'occupe du generique
/// </summary>
public class PaneauGenerique : MonoBehaviour
{
    [SerializeField] SONavigation _navigation; // accede au ScriptableObject navigation   #tpsynthese (Alex Guilbault)
    int DureeGenerique = 70; // durée du générique en secondes #tpsynthese (Alex Guilbault)
    Rigidbody2D _rb2D; // accede au composant Rigidbody2D #tpsynthese (Alex Guilbault)
    float _vitesse = 0.5f; // vitesse de déplacement du générique  #tpsynthese (Alex Guilbault)

    // Start is called before the first frame update #tpsynthese (Alex Guilbault)
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>(); // assigne le rigibody au generique
        StartCoroutine(CoroutineTempsGenerique()); // lance la coroutine qui va déclencher la duree du générique #tpsynthese (Alex Guilbault)
    }

    // Update is called once per frame
    void Update()
    {
        _rb2D.MovePosition(transform.position + (Vector3)(Vector2.up * _vitesse)); //déplace le projectiles #tpsynthese (Alex Guilbault)
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) // si on clique sur l'ecran ou appuie sur espace #tpsynthese (Alex Guilbault)
        {
            _navigation.AllerScenePrecedente(); // on retourne au menu #tpsynthese (Alex Guilbault)
        }

    }
    /// <summary>
    ///  Coroutine qui va déclencher la duree du générique #tpsynthese (Alex Guilbault)
    ///  retourne au menu apres la duree du générique
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineTempsGenerique()
    {
        yield return new WaitForSeconds(DureeGenerique);
        _navigation.AllerScenePrecedente(); // fais revenir au menu

    }
}