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
    /// �V�����Q�[���I�u�W�F�N�g���K�v�ɂȂ����Ƃ��ɌĂ΂��B
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
    /// �v�f���\�������Ƃ��̏���
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="index"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void ProvideData(Transform transform, int index)
    {
        transform.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
    }

    /// <summary>
    /// �v�f���s�v�ɂȂ����Ƃ�
    /// </summary>
    /// <param name="trans"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void ReturnObject(Transform trans)
    {
        Destroy(trans.gameObject);
    }
}
