using UnityEngine;
using UnityEngine.UI;

public class AV_Test : MonoBehaviour
{
    [SerializeField] private SO_BobaOrder[] orderOptions;
    [SerializeField] private GameObject ordersBar;
    [SerializeField] private int maxOrders;
    [SerializeField] private GameObject orderPrefab;
    [SerializeField] private GameObject orderEmptySprite;

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
        // create a game object and grab the slot for sprites to be added to
        GameObject go = Instantiate(orderPrefab);
        GameObject slot = go.GetComponent<Order>().GetSpriteSlot();

        // create another go to hold image component for each sprite
        GameObject temp1 = Instantiate(orderEmptySprite);
        temp1.GetComponent<Image>().sprite = bo.GetCupSprite();

        GameObject temp2 = Instantiate(orderEmptySprite);
        temp2.GetComponent<Image>().sprite = bo.GetMixerSprite();

        GameObject temp3 = Instantiate(orderEmptySprite);
        temp3.GetComponent<Image>().sprite = bo.GetSolidsSprite();

        GameObject temp4 = Instantiate(orderEmptySprite);
        temp4.GetComponent<Image>().sprite = bo.GetIceSprite();

        // add go's with images to slot
        // if template images are left blank dont add the GO
        if (bo.GetCupSprite() != null)
            temp1.transform.parent = slot.transform;
        if (bo.GetMixerSprite() != null)
            temp2.transform.parent = slot.transform;
        if (bo.GetSolidsSprite() != null)
            temp3.transform.parent = slot.transform;
        if (bo.GetIceSprite() != null)
            temp4.transform.parent = slot.transform;

        // add completed order to bar
        go.transform.parent = ordersBar.transform;
        
        // TODO keep track of current orders maybe?
    }
}
