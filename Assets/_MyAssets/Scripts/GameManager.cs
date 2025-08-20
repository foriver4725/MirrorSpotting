using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;
using PlayerInput = StarterAssets.StarterAssetsInputs;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject titleUI;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private TextMeshProUGUI leftText;

    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private LayerMask dummyObjectsLayerMask;
    [SerializeField] private float lookingLength = 10.0f;

    [SerializeField] private AudioSource foundAs;

    public static GameManager Instance { get; set; } = null;
    public byte LeftAmount { get => leftAmount; set => leftAmount = value; }
    public RaycastHit[] HitsThisFrame { get; private set; } = new RaycastHit[64];
    public int HitsCountThisFrame { get; private set; } = 0;

    private bool isPaused = true;

    private byte _leftAmount = 5;
    private byte leftAmount
    {
        get => _leftAmount;
        set
        {
            if (isPaused)
            {
                Debug.LogWarning("Cannot change left amount while paused.");
                return;
            }

            if (_leftAmount == 0)
            {
                Debug.LogWarning("Left amount is already zero.");
                return;
            }

            _leftAmount = value;
            leftText.text = $"<color=#00ffff>{leftAmount}</color>  Left";

            if (_leftAmount == 0)
                OnFoundAll(destroyCancellationToken).Forget();
        }
    }

    private void Start() => Impl(destroyCancellationToken);

    private void Impl(CancellationToken ct)
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        SetPaused(isPaused);

        PauseImpl(ct).Forget();
        UpdateLookingObjects(ct).Forget();
    }

    private async UniTaskVoid PauseImpl(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(0.2f, cancellationToken: ct);
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space), cancellationToken: ct);
            SetPaused(isPaused = !isPaused);
        }
    }

    private void SetPaused(bool isPaused)
    {
        if (isPaused)
        {
            titleUI.SetActive(true);
            mainUI.SetActive(false);
            SetCursorState(true);
            PlayerInput.CanGetInput = false;
        }
        else
        {
            titleUI.SetActive(false);
            mainUI.SetActive(true);
            SetCursorState(false);
            PlayerInput.CanGetInput = true;
        }
    }

    private void SetCursorState(bool doShow)
    {
        if (doShow)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private async UniTaskVoid UpdateLookingObjects(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.DelayFrame(4, cancellationToken: ct);
            HitsCountThisFrame = isPaused ? 0 :
                Physics.RaycastNonAlloc(playerCameraRoot.position, playerCameraRoot.forward,
                    HitsThisFrame, lookingLength, dummyObjectsLayerMask);
        }
    }

    private async UniTaskVoid OnFoundAll(CancellationToken ct)
    {
        leftText.text = "<color=#ffff00>Found All!</color>\n<size=60>Press U to Restart</size>";

        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.U), cancellationToken: ct);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayFoundSE()
    {
        foundAs.Play();
    }

    // Draw ray for debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(playerCameraRoot.position, playerCameraRoot.forward * lookingLength);
    }
}