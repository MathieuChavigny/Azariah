using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;

[CreateAssetMenu(menuName = "Sauvegarde", fileName = "Sauvegarde")]
public class SOSauvegarde : ScriptableObject
{


    public int _meilleurScore1; //champs pour le meilleur score en premiere position #tp4 (Alex Guilbault)
    public int _meilleurScore2; //champs pour le meilleur score en deuxieme position #tp4 (Alex Guilbault)
    public int _meilleurScore3; //champs pour le meilleur score en troisieme position #tp4 (Alex Guilbault)

    public string _nomMeilleurJoueur1; // champs pour le nom du joueur en premiere position #tp4 (Alex Guilbault)
    public string _nomMeilleurJoueur2; // champs pour le nom du joueur en deuxieme position #tp4 (Alex Guilbault)
    public string _nomMeilleurJoueur3; // champs pour le nom du joueur en troisieme position #tp4 (Alex Guilbault)

    // nom du fichier pour sauvegarder les meilleurs scores dans les dossier locaux #tp4 (Alex Guilbault) 
    [SerializeField] string _fichier = "MeilleurScore.txt";


    [DllImport("__Internal")] // Synchroniser WebGL
    private static extern void SynchroniserWebGL(); // Synchroniser WebGL


    [ContextMenu("Lire fichier")] // permet de lire le fichier de sauvegarde dans les trois points de l'inspecteur #tp4 (Alex Guilbault)
    /// <summary>
    /// permet de lire le fichier de sauvegarde #tp4 (Alex Guilbault)
    /// </summary>
    public void LireMeilleurScoreSauvegarder() // permet de lire le fichier de sauvegarde #tp4 (Alex Guilbault)

    {
        // C:/Users/Utilisateur/AppData/LocalLow/TIM CSTJ/TP1 (Jeu 4) chemin du fichier de sauvegarde pour ordinateur(home) debugage Alex#tp4 (Alex Guilbault)
        string fichierETChemins = Application.persistentDataPath + "/" + _fichier; // chemin du fichier de sauvegarde #tp4 (Alex Guilbault)
        Debug.Log(fichierETChemins); // debug log pour debugage et accees au fichier dans l'explorateur de fichier #tp4 (Alex Guilbault)

        if (File.Exists(fichierETChemins)) // si le fichier avec le path du fichier existe #tp4 (Alex Guilbault)
        {
            string contenuJson = File.ReadAllText(fichierETChemins); // lit le fichier et le converti en string #tp4 (Alex Guilbault)
            Debug.Log(contenuJson);
            JsonUtility.FromJsonOverwrite(contenuJson, this); // converti le string en json #tp4 (Alex Guilbault)
            #if UNITY_EDITOR // permet de sauvegarder les changements dans l'inspecteur #tp4 (Alex Guilbault)
            UnityEditor.EditorUtility.SetDirty(this); // permet de sauvegarder les changements dans l'inspecteur #tp4 (Alex Guilbault)
            UnityEditor.AssetDatabase.SaveAssets(); // permet de sauvegarder les changements dans l'inspecteur #tp4 (Alex Guilbault)
            #endif
        }

    }

    [ContextMenu("Ecrire fichier")] // permet de Ecrire le fichier de sauvegarde dans les trois points de l'inspecteur #tp4 (Alex Guilbault)
    /// <summary>
    /// fonction pour ecrire les donnees dans le fichier de sauvegarde #tp4 (Alex Guilbault)
    /// </summary>
    public void EcrireMeilleurScoreSauvegarder() 
    {

        string fichierETChemins = Application.persistentDataPath + "/" + _fichier; // chemin du fichier de sauvegarde #tp4 (Alex Guilbault)
        Debug.Log(fichierETChemins); // debug log pour debugage et accees au fichier dans l'explorateur de fichier #tp4 (Alex Guilbault)

        string contenu = JsonUtility.ToJson(this, true); // converti le json en string #tp4 (Alex Guilbault)
        File.WriteAllText(fichierETChemins, contenu); // ecrit le string dans le fichier #tp4 (Alex Guilbault) 
        if (Application.platform == RuntimePlatform.WebGLPlayer) // Synchroniser WebGL
        {
            Debug.Log("Synchroniser WebGL");
            SynchroniserWebGL(); // Synchroniser WebGL
        }

    }


}

