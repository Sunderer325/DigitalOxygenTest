using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float startDelay = default;
    [SerializeField] GameObject cage = default;
    [SerializeField] Lift lift = default;
    [SerializeField] GameObject pickupPoint = default;
    [SerializeField] GameObject endGamePoint = default;

    public bool IsOpening;
    public bool IsEnding;

    new AudioPrefab audio;

    private static LevelManager _instance;
    public static LevelManager Instance { 
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<LevelManager>();
            }
            return _instance;
        } 
    }

    private void Start()
    {
        audio = GetComponent<AudioPrefab>();
    }

    public IEnumerator Opening()
    {
        IsOpening = true;
        lift.gameObject.SetActive(false);
        cage.SetActive(true);

        BoxCollider2D[] colliders = cage.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = true;
        }

        yield return new WaitForSeconds(startDelay);

        foreach(BoxCollider2D collider in colliders)
        {
            collider.enabled = false;
        }
        audio.Play("cage_open");
        IsOpening = false;
    }

    public IEnumerator Ending() {
        IsEnding = true;
        cage.SetActive(false);
        lift.gameObject.SetActive(true);

        audio.PlayLoop("chain");
        while(lift.transform.position.y > pickupPoint.transform.position.y)
        {
            lift.movement.Move(Vector2.down * lift.moveSpeed * Time.deltaTime);
            yield return null;
        }
        audio.Stop();

        Player target = FindObjectOfType<Player>();
        while (!lift.trigger.bounds.Contains(target.transform.position))
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        target.DisableMovement();
        target.transform.SetParent(lift.transform);
        yield return new WaitForSeconds(2);

        audio.PlayLoop("chain");
        while (true)
        {
            lift.movement.Move(Vector2.up * lift.moveSpeed * Time.deltaTime);

            if (lift.transform.position.y > endGamePoint.transform.position.y)
                break;

            yield return null;
        }
        audio.Stop();
        IsEnding = false;
    }
}
