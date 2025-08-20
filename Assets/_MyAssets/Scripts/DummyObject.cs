using UnityEngine;

public class DummyObject : MonoBehaviour
{
    [SerializeField] private GameObject relatedOnlyMirrorObjectRoot;

    private bool hasFound = false;
    private float lookingTime = 0.0f;

    private static readonly float LookingDuration = 3.0f;

    private void Update()
    {
        if (hasFound)
            return;

        // まだ未発見

        // 何も見ていない(or ポーズ中)なら、見つめるカウントをリセット
        int lookingObjectsAmount = GameManager.Instance.HitsCountThisFrame;
        if (lookingObjectsAmount == 0)
        {
            lookingTime = 0.0f;
            return;
        }

        // 自身のオブジェクトが含まれているか調べる
        for (int i = 0; i < lookingObjectsAmount; i++)
        {
            if (GameManager.Instance.HitsThisFrame[i].collider.gameObject == gameObject)
            {
                // 含まれていた
                {
                    Debug.Log($"Looking at {gameObject.name} for {lookingTime} seconds...");

                    // 見つめる時間を加算
                    lookingTime += Time.deltaTime;

                    // 見つめる時間が経過したら、見つかったとする
                    if (lookingTime >= LookingDuration)
                    {
                        hasFound = true;
                        lookingTime = 0.0f; // リセット

                        GameManager.Instance.LeftAmount--;

                        GameManager.Instance.PlayFoundSE();

                        // 部屋を元に戻す
                        if (relatedOnlyMirrorObjectRoot != null)
                            SetLayerToThisAndAllChildren(relatedOnlyMirrorObjectRoot, 0);
                        this.gameObject.SetActive(false);
                    }
                }

                return;
            }
        }

        // 自身のオブジェクトが含まれていなかったら、見つめるカウントをリセット
        lookingTime = 0.0f;
    }

    private void SetLayerToThisAndAllChildren(GameObject obj, int layer)
    {
        obj.layer = layer;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            SetLayerToThisAndAllChildren(obj.transform.GetChild(i).gameObject, layer);
        }
    }
}
