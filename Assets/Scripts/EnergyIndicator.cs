using UnityEngine;

public class EnergyIndicator : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private GameManager _gameManager;
    [SerializeField] private Sprite checkMark;
    [SerializeField] private Sprite cross;
    

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.energy == 0) _spriteRenderer.sprite = cross;
        else _spriteRenderer.sprite = checkMark;
    }
}
