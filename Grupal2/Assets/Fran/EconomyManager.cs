using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [Header("Economía")]
    public int money = 500;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool SpendMoney(int amount)
    {
        if (money < amount)
            return false;

        money -= amount;
        return true;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public int GetMoney()
    {
        return money;
    }
}

// Ejemplo de uso:
// EconomyManager.Instance.AddMoney(50);
//
// if (EconomyManager.Instance.SpendMoney(100))
// {
//     Debug.Log("Compra realizada");
// }