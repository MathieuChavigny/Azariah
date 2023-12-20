using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
// =========================================
// Auteur : Alex Guilbault
// Auteur des commentaires: Alex Guilbault et Mathieu Chavigny
// ==========================================
/// Classe qui controle les déplacements a travers les scenes
/// </summary>
[CreateAssetMenu(fileName = "Navigation", menuName = "Navigation")]
public class SONavigation : ScriptableObject
{
    [SerializeField] SOPerso _donneesPerso; // Champs pour parler au SOPerso
    [SerializeField] SONiveau _donneesNiveau; // Champs pour parler au SOPerso


    /// <summary>
    /// fonction qui transporte le joueur dans la scene de jeu et renitialise les inventaires
    /// </summary>
    public void CommencerJeu()
    {
        _donneesPerso.Initialiser();
        SceneManager.LoadScene(3);
    }

    /// <summary>
    /// fonction qui retransporte le joueur dans la scene de jeu du niveau suivant 
    /// renitialise l'inventaire des objets 
    /// affiche l'argent (joyau et ferraille) restant au joueur
    /// </summary>
    public void SortirBoutique()
    {

        _donneesPerso.lesObjets.Clear();
        _donneesPerso.niveau++;
        _donneesPerso.etape++;
        _donneesPerso.fleche = _donneesPerso.flecheMax;
        _donneesNiveau.taille += Vector2Int.right;
        if(_donneesPerso.etape == 2)
        {
            AllerSceneEnigme();
        }
        else{
            
            AllerScenePrecedente(); //fonction qui retransporte le joueur dans la scene de jeu du niveau suivant 

        }

    }

    /// <summary>
    /// fonction qui transporte le joueur dans la scene de la boutique
    /// renitilialise l'inventaire des ameliorations
    /// </summary>
    public void AllerSceneSuivante()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _donneesPerso.lesAmeliorations.Clear();
    }

    /// <summary>
    /// fonction qui retransporte le joueur dans la scene de jeu du niveau suivant
    /// </summary>
    public void AllerScenePrecedente()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    /// <summary>
    /// Mêne le joueur au menu principal #tp4 (Alex Guilbault)
    /// </summary>
    public void AllerMenuPrincipal()
    {
        SceneManager.LoadScene(0);
        _donneesPerso.Initialiser();
    }

    /// <summary>
    /// Mêne le joueur a la scene de fin de partie	#tp4 (Alex Guilbault)
    /// </summary>
    public void AllerPartieTeminer()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1); 
        
    }

    /// <summary>
    /// Mêne le joueur a la scene de l'enigme #tpSynthese
    /// </summary>
    public void AllerSceneEnigme()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        Debug.Log("AllerSceneEnigme");
    }
    /// <summary>
    /// Mêne le joueur a la scene du boss Mathieu Chavigny #tpSynthese
    /// </summary>
    public void AllerSceneBoss()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        Debug.Log("AllerSceneBoss");
    }
    /// <summary>
    /// Mêne le joueur a la scene de l'instruction Mathieu Chavigny #tpSynthese
    /// </summary>
    public void AllerSceneInstruction()
    {
        SceneManager.LoadScene(2); 
        Debug.Log("AllerSceneInstruction");
    }	
    /// <summary>
    /// Mêne le joueur au jeu  Mathieu Chavigny #tpSynthese
    /// </summary>
    public void RetourJeu()
    {
        _donneesPerso.etape = 0;
        SceneManager.LoadScene(3); 
        Debug.Log("RetourJeu");
    }

}

