using UnityEngine;

public class FoodCollectible : MonoBehaviour
{
    // Multiplier applied to score when collected
    public int multiplier = 2;

    // How close player must be to collect
    public float collectRadius = 1f;

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
            return;

        if (Vector3.Distance(transform.position, player.transform.position) <= collectRadius)
        {
            // Avoid direct compile-time dependency on GameManager type: find component by name and invoke by reflection
            var all = FindObjectsOfType<MonoBehaviour>();
            foreach (var mb in all)
            {
                if (mb == null)
                    continue;

                var type = mb.GetType();
                if (type.Name == "GameManager")
                {
                    var method = type.GetMethod("MultiplyScore");
                    if (method != null)
                    {
                        method.Invoke(mb, new object[] { multiplier });
                    }
                    break;
                }
            }

            Destroy(gameObject);
        }
    }
}
