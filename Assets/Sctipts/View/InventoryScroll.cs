using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LoopScrollRect))]
public class InventoryScroll : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
{
    public GameObject prefab;

    private LoopScrollRect scrollRect;
    public void Start()
    {
        scrollRect = GetComponent<LoopScrollRect>();

        scrollRect.prefabSource = this;
        scrollRect.dataSource = this;
        scrollRect.totalCount = 100;/*GameViewMaster.Instance.GetActivePlayerComponent().hasItems.Count;*/
        scrollRect.RefillCells();
    }
    /// <summary>
    /// 新しくゲームオブジェクトが必要になったときに呼ばれる。
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public GameObject GetObject(int index)
    {
        Debug.Log(index);
        return Instantiate(prefab);
    }

    /// <summary>
    /// 要素が表示されるときの処理
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="index"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void ProvideData(Transform transform, int index)
    {
        transform.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
    }

    /// <summary>
    /// 要素が不要になったとき
    /// </summary>
    /// <param name="trans"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void ReturnObject(Transform trans)
    {
        Destroy(trans.gameObject);
    }
}
