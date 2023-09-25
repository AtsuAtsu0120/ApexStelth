using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

[RequireComponent(typeof(LoopScrollRect))]
public class InventoryScroll : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
{
    public GameObject prefab;

    public LoopScrollRect scrollRect { get; private set; }

    private ObjectPool<GameObject> pool;
    public void Start()
    {
        pool = new ObjectPool<GameObject>
            (
                () => Instantiate(prefab),
                obj => obj.SetActive(true),
                obj =>
                {
                    obj.transform.SetParent(transform);
                    obj.SetActive(false);
                }
            );

        scrollRect = GetComponent<LoopScrollRect>();

        scrollRect.prefabSource = this;
        scrollRect.dataSource = this;
        scrollRect.totalCount = GameViewMaster.Instance.GetActivePlayerComponent().hasItems.Count;
        scrollRect.RefillCells();

        transform.parent.gameObject.SetActive(false);
    }
    /// <summary>
    /// 新しくゲームオブジェクトが必要になったときに呼ばれる。
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public GameObject GetObject(int index)
    {
        return pool.Get();
    }

    /// <summary>
    /// 要素が表示されるときの処理
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="index"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void ProvideData(Transform transform, int index)
    {
        var items = GameViewMaster.Instance.GetActivePlayerComponent().hasItems;

        transform.GetComponentInChildren<TextMeshProUGUI>().text = items[index].name;
    }

    /// <summary>
    /// 要素が不要になったとき
    /// </summary>
    /// <param name="trans"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void ReturnObject(Transform trans)
    {
        pool.Release(trans.gameObject);
    }
}
