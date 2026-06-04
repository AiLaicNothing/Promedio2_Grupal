using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    // Ahora cada equipo tiene su propio dinero.
    // Antes había una sola variable compartida para todos.
    [Header("Dinero por equipo")]
    public int team1Money = 500;
    public int team2Money = 500;

    // agregué  Variables para generar dinero automáticamente con el tiempo.
    // Esto permite que los equipos reciban recursos aunque no destruyan nada.
    [Header("Generación automática")]
    public int incomeAmount = 10;
    public float incomeInterval = 10f;

    private float timer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ahora Cada cierto tiempo ambos equipos reciben dinero.
    // Se agregó porque  solo ganaban recursos al llamar AddMoney().
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= incomeInterval)
        {
            timer = 0f;

            team1Money += incomeAmount;
            team2Money += incomeAmount;
        }
    }

    // CAMBIO: Ahora se especifica qué equipo está gastando dinero.
    // team = 1 para el equipo 1
    // team = 2 para el equipo 2
    public bool SpendMoney(int team, int amount)
    {
        if (team == 1)
        {
            if (team1Money < amount)
                return false;

            team1Money -= amount;
            return true;
        }

        if (team == 2)
        {
            if (team2Money < amount)
                return false;

            team2Money -= amount;
            return true;
        }

        return false;
    }

    // Ahora el dinero se agrega al equipo indicado.
    public void AddMoney(int team, int amount)
    {
        if (team == 1)
            team1Money += amount;
        else if (team == 2)
            team2Money += amount;
    }
}

// Ejemplo de uso:
//
// EconomyManager.Instance.AddMoney(1, 50);
//
// if (EconomyManager.Instance.SpendMoney(2, 100))
// {
//     Debug.Log("Compra realizada");
// }