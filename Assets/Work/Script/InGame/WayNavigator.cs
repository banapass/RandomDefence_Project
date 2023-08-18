using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayNavigator : MonoBehaviour
{
    private TrailRenderer trail;
    private List<Vector3> path;
    private int currPathIndex;
    private bool isDestination;
    private float addTime;
    [SerializeField] private float navigateSpeed = 5;
    IEnumerator navigateCo;

    private const float NEXT_NAVIGATE_DELAY = 3f;

    private void Awake()
    {
        TryGetComponent<TrailRenderer>(out trail);
        navigateCo = Navigate();
    }
    private void OnEnable()
    {
        GameManager.OnChangedGameState += OnChangedGameState;
    }
    private void OnDisable()
    {
        GameManager.OnChangedGameState -= OnChangedGameState;
    }

    public void SetPath(List<Vector3> _path)
    {
        path = _path;
    }
    public void StartNavigate()
    {
        trail.enabled = true;
        transform.position = path[0];
        trail.Clear();
        currPathIndex = 0;

        StartCoroutine(navigateCo);
    }
    public void StopNavigate()
    {
        trail.enabled = false;
        currPathIndex = 0;
        addTime = 0;
        StopCoroutine(navigateCo);
    }
    private bool IsArrivalNextDestination(Vector3 _nextPos)
    {
        return GetDistance(_nextPos) <= 0.0f;
    }
    private float GetDistance(Vector3 _nextPos)
    {
        return (_nextPos - transform.position).sqrMagnitude;
    }
    private void MoveToPoint(Vector3 _nextPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, _nextPos, navigateSpeed * Time.deltaTime);
    }
    public void FollowPath()
    {
        if (isDestination) return;

        if (IsArrivalNextDestination(path[currPathIndex]))
        {
            currPathIndex++;
            isDestination = path.Count <= currPathIndex;
        }
        else
        {
            MoveToPoint(path[currPathIndex]);
        }

    }

    private IEnumerator Navigate()
    {
        while (true)
        {
            yield return null;
            if (isDestination)
            {
                addTime += Time.deltaTime;
                if (addTime < NEXT_NAVIGATE_DELAY) continue;

                addTime = 0;
                currPathIndex = 0;
                transform.position = path[currPathIndex];
                trail.Clear();
                isDestination = false;

            }
            else
            {
                FollowPath();
            }
        }
    }

    private void OnChangedGameState(GameState _state)
    {
        if (_state == GameState.BreakTime)
        {
            StartNavigate();
        }
        else
        {
            StopNavigate();
        }
    }
}
