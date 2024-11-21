using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [Tooltip("Vitesse de déplacement de l'ennemi.")]
    public float speed = 30f;

    [Tooltip("Points de vie de l'ennemi.")]
    public float health = 100f;

    private Transform target; // Cible actuelle
    private int waypointIndex = 0; // Index du waypoint
    private WaveSpawner waveSpawner;

    void Start()
    {
        // Initialisation du premier waypoint
        target = Waypoints.points[0];
        waveSpawner = FindObjectOfType<WaveSpawner>();
    }

    void Update()
    {
        // Mouvement vers le waypoint actuel
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // Vérifier si l'ennemi a atteint le waypoint
        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            // Si l'ennemi atteint la fin, il est détruit
            Destroy(gameObject);
            return;
        }

        // Passer au prochain waypoint
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    // Méthode pour recevoir des dégâts
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    // Méthode appelée lorsque l'ennemi meurt
    void Die()
    {
        if (waveSpawner != null)
        {
            waveSpawner.EnemyDied();
        }

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (waveSpawner != null && health > 0) // Vérification pour éviter un double appel
        {
            waveSpawner.EnemyDied();
        }
    }
}
