using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    [Header("Bullet Settings")]
    [Tooltip("Vitesse de la balle.")]
    public float speed = 30f;

    [Tooltip("Dégâts infligés par la balle.")]
    public float damage = 50f;

    [Header("Effects")]
    [Tooltip("Effet d'impact lorsqu'une balle touche une cible.")]
    public GameObject impactEffect;

    // Méthode pour définir la cible de la balle
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        // Si la cible est détruite avant que la balle n'arrive, détruire la balle
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Calculer la direction vers la cible et le déplacement
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Si la balle atteint la cible
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Déplacer la balle
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    // Gérer l'impact de la balle
    void HitTarget()
    {
        // Instancier l'effet d'impact
        if (impactEffect != null)
        {
            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 2f); // Détruire l'effet après 2 secondes
        }

        // Infliger des dégâts à l'ennemi
        if (target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Appeler la méthode TakeDamage sur l'ennemi
            }
        }

        // Détruire la balle
        Destroy(gameObject);
    }
}
