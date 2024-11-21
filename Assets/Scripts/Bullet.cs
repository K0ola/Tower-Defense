using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    [Header("Bullet Settings")]
    [Tooltip("Vitesse de la balle.")]
    public float speed = 30f;

    [Tooltip("D�g�ts inflig�s par la balle.")]
    public float damage = 50f;

    [Header("Effects")]
    [Tooltip("Effet d'impact lorsqu'une balle touche une cible.")]
    public GameObject impactEffect;

    // M�thode pour d�finir la cible de la balle
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        // Si la cible est d�truite avant que la balle n'arrive, d�truire la balle
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Calculer la direction vers la cible et le d�placement
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Si la balle atteint la cible
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // D�placer la balle
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    // G�rer l'impact de la balle
    void HitTarget()
    {
        // Instancier l'effet d'impact
        if (impactEffect != null)
        {
            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 2f); // D�truire l'effet apr�s 2 secondes
        }

        // Infliger des d�g�ts � l'ennemi
        if (target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Appeler la m�thode TakeDamage sur l'ennemi
            }
        }

        // D�truire la balle
        Destroy(gameObject);
    }
}
