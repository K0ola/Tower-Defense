using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int rewardAmount = 50;

    [SerializeField] private EnemyHealthBar healthBar;


    private int currentHealth;
    private Transform target;
    private int waypointIndex = 0;
    private WaveSpawner waveSpawner;

    void Start()
    {
        currentHealth = maxHealth;
        waveSpawner = GameObject.FindObjectOfType<WaveSpawner>();

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }


        if (waypoint.points.Length > 0)
            target = waypoint.points[waypointIndex];
    }

    void Update()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        Vector3 lookDir = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookDir);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            GoToNextWaypoint();
        }
    }

    private void GoToNextWaypoint()
    {
        if (waypointIndex >= waypoint.points.Length - 1)
        {
            if (BaseHealthManager.Instance != null)
            {
                BaseHealthManager.Instance.TakeDamage(currentHealth);
            }

            waveSpawner?.EnemyDied();
            Destroy(gameObject);
            return;
        }

        waypointIndex++;
        target = waypoint.points[waypointIndex];
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        waveSpawner?.EnemyDied();

        MoneyManager.Instance?.AddMoney(rewardAmount);

        Destroy(gameObject);
    }
}
