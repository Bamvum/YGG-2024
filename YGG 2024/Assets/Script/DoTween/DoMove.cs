using DG.Tweening;
using UnityEngine;

public class DoMove : MonoBehaviour
{
    private float defaultX = 0;
    private float defaultY = 0;

    [Header("New Pos")]
    public float x = 0;
    public float y = 0;
    public float MoveSpeed = 0.8f;
    private void Awake(){
        defaultX = gameObject.transform.localPosition.x;
        defaultY = gameObject.transform.localPosition.y;
    }
    private void OnEnable(){
        Utilities.DisableAllButtons(gameObject);
        gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(x,y), MoveSpeed).SetEase(Ease.InOutSine).OnComplete(() => Utilities.EnableAllButtons(gameObject));
    }
    public async void Close(){
        Utilities.DisableAllButtons(gameObject);
        await gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(defaultX, defaultY), MoveSpeed).SetEase(Ease.InOutSine).OnComplete(() => {
            Utilities.EnableAllButtons(gameObject);
            gameObject.SetActive(false);
        }).AsyncWaitForCompletion();
    }
}
