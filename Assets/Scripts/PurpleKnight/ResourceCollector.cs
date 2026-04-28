using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    private int money = 0;
    private int meat = 0;
    private int wood = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Money
        if (collision.gameObject.CompareTag("MoneyBag"))
        {
            money++;
            Destroy(collision.gameObject);
            UIManager.Instance.UpdateMoney(money);
        }
        //Carne
        else if (collision.gameObject.CompareTag("Meat"))
        {
            meat++;
            Destroy(collision.gameObject);
            UIManager.Instance.UpdateMeat(meat);
        }
        //Madera
        else if (collision.gameObject.CompareTag("Wood"))
        {
            wood++;
            Destroy(collision.gameObject);
            UIManager.Instance.UpdateWood(wood);
        }
    }

    public void UpdateAllResources()
    {
        UIManager.Instance.UpdateMoney(money);
        UIManager.Instance.UpdateMeat(meat);
        UIManager.Instance.UpdateWood(wood);
    }




    /// <summary>
    /// Retornamos los valores para saber si se cumplio la mision
    /// </summary>
    /// <returns></returns>
    public int GetMoney() => money;
    public int GetMeat() => meat;
    public int GetWood() => wood;

    public void SetMoney(int amount) => money = amount;
    public void SetMeat(int amount) => meat = amount;
    public void SetWood(int amount) => wood = amount;
}

