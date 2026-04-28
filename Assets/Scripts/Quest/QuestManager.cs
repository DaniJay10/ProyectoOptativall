using UnityEngine;
using UnityEngine.UIElements;

public class QuestManager : MonoBehaviour
{
    public Quest[] quests;
    private int currentQuestIndex = 0;

    private bool questActive = false;

    private ResourceCollector Player;

    private void Start()
    {
        Player = FindFirstObjectByType<ResourceCollector>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            UIManager.Instance.ShowQuestPanel();

            if(!questActive)
            {
                questActive = true;
                ShowQuest(quests[currentQuestIndex]);
            }
            else
            {
                Quest currentQuest = quests[currentQuestIndex];

                if(currentQuest.IsCompleted(Player.GetWood(), Player.GetMoney(), Player.GetMeat()))
                {
                    AdvanceQuest();
                }
                else
                {
                    ShowQuest(currentQuest);
                }
            }
        }
    }

    /// <summary>
    /// Mostrar mision actual
    /// </summary>
    /// <param name="quest"></param>
    public void ShowQuest(Quest quest)
    {
        UIManager.Instance.UpdateQuestPanel(quest.moneyRequired, quest.woodRequired, quest.meatRequired, quest.questName);
    }

    /// <summary>
    /// Resta de recursos de la mision al inventario actual
    /// </summary>
    public void AdvanceQuest()
    {
        Debug.Log("Mision completa");

        Player.SetMoney(Player.GetMoney() - quests[currentQuestIndex].moneyRequired);
        Player.SetWood(Player.GetWood() - quests[currentQuestIndex].woodRequired);
        Player.SetMeat(Player.GetMeat() - quests[currentQuestIndex].meatRequired);

        Player.UpdateAllResources();

        if(currentQuestIndex< quests.Length -1)
        {
            currentQuestIndex++;
            ShowQuest(quests[currentQuestIndex]);
        }
        else
        {
            Debug.Log("Todas las misiones completadas");
        }
    }
}
