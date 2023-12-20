using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 
/// Auteur du code et des commentaires : #tp4 (Alex Guilbault)
/// </summary>
public class CalculScore : MonoBehaviour
{
    int _ChampsScore = 4; // nombre de type de score (champs) #tp4 (Alex Guilbault)
    [SerializeField] TextMeshProUGUI[] _TypeTxt; // tableau de champs de texte du type #tp4 (Alex Guilbault)
    [SerializeField] TextMeshProUGUI[] _sommeTxt; // tableau de champs de texte du calcul du score #tp4 (Alex Guilbault)
    [SerializeField] TextMeshProUGUI[] _totalTxt; // tableau de champs de texte du total #tp4 (Alex Guilbault)
    [SerializeField] TextMeshProUGUI _grandTotalTxt; //champ de texte du grand total #tp4 (Alex Guilbault)

    int _multiplicateurJoyaux; // nombre de joyaux recuperer #tp4 (Alex Guilbault)
    int _multiplicateurFerrailles; //  nombre de Ferrailles recuperer #tp4 (Alex Guilbault)
    int _multiplicateurEnemies; // nombre d'ennemi vaincus #tp4 (Alex Guilbault)
    int _multiplicateurNiveaux; // nombre de niveau terminer #tp4 (Alex Guilbault)
    int _pointJoyaux = 50; // points octroyer par joyau #tp4 (Alex Guilbault)
    int _pointsFerrailles = 100; // points octroyer par Ferraille #tp4 (Alex Guilbault)
    int _pointsEnemies = 500; // points octroyer par Enemies #tp4 (Alex Guilbault)
    int _pointsNiveaux = 1000; //  points octroyer par Niveaux #tp4 (Alex Guilbault)
    int _totalpointJoyaux; // points total pour joyau #tp4 (Alex Guilbault)
    int _totalpointsFerrailles; //  points total pour Ferraille #tp4 (Alex Guilbault)
    int _totalpointsEnemies; //  points total pour l'ennemi #tp4 (Alex Guilbault)
    int _totalpointsNiveaux; // points total pour Niveaux #tp4 (Alex Guilbault)
    public int _grandTotal; //  variable pour le grand-total #tp4 (Alex Guilbault)

    [SerializeField] SOPerso _perso; // Champ pour le ScriptableObjet de Perso #tp4 (Alex Guilbault)
    [SerializeField] AudioClip _sonScore;  //son du calcul du score #tpsynthese (Alex Guilbault)
    [SerializeField] AudioClip _sonGrandTotal;   //son du grandTotal (Alex Guilbault)

    /// <summary>
    /// Awake is called when the script instance is being loaded. #tp4 (Alex Guilbault)
    /// </summary>
    void Awake()
    {
        _multiplicateurJoyaux = _perso._totalJoyauRecuperer;
        _multiplicateurFerrailles = _perso._totalFerailleRecuperer;
        _multiplicateurEnemies = _perso._totalEnemieTuer;
        _multiplicateurNiveaux = _perso.niveau;
        CalculerPoints(); // appel de la fonction CalculerPoints #tp4 (Alex Guilbault)
        MiseAJourPoint(); // appel de la fonction CalculerPoints #tp4 (Alex Guilbault)
        StartCoroutine(CoroutineAffichageProgressifScore()); // demarre la coroutine pour l'affichage du score progressif #tp4 (Alex Guilbault)

    }

    /// <summary>
    /// fonction pour calculer les points #tp4 (Alex Guilbault)
    /// </summary>
    void CalculerPoints()
    {

        _totalpointJoyaux = _pointJoyaux * _multiplicateurJoyaux;
        _totalpointsFerrailles = _pointsFerrailles * _multiplicateurFerrailles;
        _totalpointsEnemies = _pointsEnemies * _multiplicateurEnemies;
        _totalpointsNiveaux = _pointsNiveaux * _multiplicateurNiveaux;
    }

    /// <summary>
    /// fonction pour la Mise A Jour des Point #tp4 (Alex Guilbault) 
    /// </summary>
    private void MiseAJourPoint()
    {
        for (int i = 0; i < _sommeTxt.Length; i++) // boucle qui va chercher tout les champs sommes #tp4 (Alex Guilbault)
        {
            switch (i) //switch qui met A Jour l'affichage
            {
                case 0:
                    _sommeTxt[i].text = $"{_pointJoyaux}pts x {_multiplicateurJoyaux}x";
                    _totalTxt[i].text = $"Total : {_pointJoyaux * _multiplicateurJoyaux} pts";
                    break;
                case 1:
                    _sommeTxt[i].text = $"{_pointsFerrailles}pts x {_multiplicateurFerrailles}x";
                    _totalTxt[i].text = $"Total : {_pointsFerrailles * _multiplicateurFerrailles} pts";
                    break;
                case 2:
                    _sommeTxt[i].text = $"{_pointsEnemies}pts x {_multiplicateurEnemies}x";
                    _totalTxt[i].text = $"Total : {_pointsEnemies * _multiplicateurEnemies} pts";
                    break;
                case 3:
                    _sommeTxt[i].text = $"{_pointsNiveaux}pts x {_multiplicateurNiveaux}x";
                    _totalTxt[i].text = $"Total : {_pointsNiveaux * _multiplicateurNiveaux} pts";
                    break;
            }
        }
        // fait le calcul du grand total a l'aide de tout les sommes #tp4 (Alex Guilbault)
        _grandTotal = _totalpointJoyaux + _totalpointsFerrailles + _totalpointsEnemies + _totalpointsNiveaux;  
        //affichage du grand total dans l'affichage #tp4 (Alex Guilbault)
        _grandTotalTxt.text = $"Grand-total : {_grandTotal}pts";
    }

    /// <summary>
    /// coroutine pour afficher progressivement le score #tp4 (Alex Guilbault)
    /// </summary>    
    /// <returns></returns>
    IEnumerator CoroutineAffichageProgressifScore()
    {
        for (int i = 0; i < _ChampsScore; i++)
        {
            _TypeTxt[i].enabled = true;
            GestSonore.instance.JouerEffetSonore(_sonScore);
            yield return new WaitForSeconds(1f);
            _sommeTxt[i].enabled = true;
            GestSonore.instance.JouerEffetSonore(_sonScore);
            yield return new WaitForSeconds(1f);
            _totalTxt[i].enabled = true;
            GestSonore.instance.JouerEffetSonore(_sonScore);
            yield return new WaitForSeconds(2f);
        }
        yield return new WaitForSeconds(2f);
            GestSonore.instance.JouerEffetSonore(_sonGrandTotal);
        _grandTotalTxt.enabled = true;
    }


}

