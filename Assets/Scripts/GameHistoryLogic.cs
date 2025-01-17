using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public Transform enemyPrefab; // Type d'ennemi (prefab)
        public int count;             // Nombre de cet ennemi
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemyType> enemies; // Liste des types d'ennemis pour la vague
        public float spawnRate = 0.5f;  // Intervalle entre chaque spawn d'ennemi
    }

    [Header("Wave Configuration")]
    public List<Wave> waves;            // Liste des vagues (limit� � 10)
    public Transform spawnPoint;        // Point de spawn des ennemis

    [Header("UI Elements")]
    public TMP_Text waveCountdownText;  // Texte pour le compte � rebours
    public TMP_Text waveNumberText;     // Texte pour le num�ro de la vague

    [Header("Wave Settings")]
    public float timeBetweenWaves = 5.5f;
    private float countdown = 2f;

    private int waveIndex = 0;          // Index de la vague actuelle
    private int enemiesAlive = 0;       // Nombre d'ennemis encore vivants

    void Update()
    {
        // Si des ennemis sont encore vivants, on ne commence pas la prochaine vague
        if (enemiesAlive > 0)
        {
            return;
        }

        // D�clenche le countdown vers la prochaine vague
        if (countdown <= 0f)
        {
            if (waveIndex < waves.Count) // S'assurer qu'il reste des vagues
            {
                StartCoroutine(SpawnWave());
                countdown = timeBetweenWaves;
            }
            else
            {
                Debug.Log("Toutes les vagues ont �t� compl�t�es !");
            }
            return;
        }

        countdown -= Time.deltaTime;
        waveCountdownText.text = Mathf.Round(countdown).ToString();
    }

    IEnumerator SpawnWave()
    {
        if (waveIndex >= waves.Count)
        {
            yield break; // Arr�ter si toutes les vagues ont �t� termin�es
        }

        Wave currentWave = waves[waveIndex];
        waveNumberText.text = "Wave " + (waveIndex + 1);

        // Compter le nombre total d'ennemis dans cette vague
        enemiesAlive = 0;
        foreach (var enemyType in currentWave.enemies)
        {
            enemiesAlive += enemyType.count;
        }

        // Spawn des ennemis selon leur type et quantit�
        foreach (var enemyType in currentWave.enemies)
        {
            for (int i = 0; i < enemyType.count; i++)
            {
                SpawnEnemy(enemyType.enemyPrefab);
                yield return new WaitForSeconds(currentWave.spawnRate);
            }
        }

        waveIndex++; // Passer � la vague suivante
    }

    void SpawnEnemy(Transform enemyPrefab)
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Chaque ennemi d�truit doit appeler "EnemyDied()"
    }

    public void EnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            Debug.Log("Tous les ennemis de la vague sont morts !");
        }
    }
}
