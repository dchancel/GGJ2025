using UnityEngine;
using UnityEngine.UI;

public class AV_Test : MonoBehaviour
{
    [SerializeField] private SO_BobaOrder[] orderOptions;
    [SerializeField] private GameObject ordersBar;
    [SerializeField] private int maxOrders;
    [SerializeField] private GameObject orderPrefab;

    void Awake()
    {
        AddOrders();
    }

    private void AddOrders()
    {
        for (int i = 0; i < maxOrders; i++)
        {
            int r = Random.Range(0, orderOptions.Length);
            Debug.Log(r);
            CreateOrder(orderOptions[r]);
        }
    }

    private void CreateOrder(SO_BobaOrder bo)
    {
        // prefab inst
        GameObject go = Instantiate(orderPrefab);
        GameObject slot = go.GetComponent<Order>().SpritesSlot;

        // create image parent
        GameObject temp1 = new GameObject();
        temp1.AddComponent<Image>().sprite = bo.GetCupSprite();
        
        GameObject temp2 = new GameObject();
        temp2.AddComponent<Image>().sprite = bo.GetMixerSprite();

        GameObject temp3 = new GameObject();
        temp3.AddComponent<Image>().sprite = bo.GetSolidsSprite();

        GameObject temp4 = new GameObject();
        temp4.AddComponent<Image>().sprite = bo.GetIceSprite();

        temp1.transform.parent = slot.transform;
        temp2.transform.parent = slot.transform;
        temp3.transform.parent = slot.transform;
        temp4.transform.parent = slot.transform;

        go.transform.parent = ordersBar.transform;
    }
}
