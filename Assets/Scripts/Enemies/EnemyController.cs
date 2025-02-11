using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyController : NetworkBehaviour
{
    [SerializeField] private EnemyStatsSO _enemyStatsSO;
    private Transform _route;
    private float _tParam;
    private Vector3 _objectPosition;
    private float _speed;
    private bool _coroutineAllowed;
    private float _curveLength;
    private const int samplePoints = 50;


    void OnEnable()
    {
        _speed = _enemyStatsSO.speed;
        _route = GameObject.FindGameObjectWithTag("Route").transform;

        _tParam = 0f;
        _coroutineAllowed = true;
    }

    void Update()
    {
        if (_coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute());
        }
    }

    private IEnumerator GoByTheRoute()
    {
        _coroutineAllowed = false;

        int pointsCount = _route.childCount;
        Vector3[] controlPoints = new Vector3[pointsCount];

        for (int i = 0; i < pointsCount; i++)
        {
            controlPoints[i] = _route.GetChild(i).position;
        }

        _curveLength = ApproximateCurveLength(controlPoints);
        float travelDistance = 0f;
        float step = _speed / _curveLength;

        while (travelDistance < 1)
        {
            _tParam = travelDistance;
            _objectPosition = CalculateBezierPoint(_tParam, controlPoints);
            transform.position = _objectPosition;
            travelDistance += step * Time.deltaTime;
            yield return null;
        }

        _tParam = 0;

        _coroutineAllowed = true;
    }

    private Vector3 CalculateBezierPoint(float t, Vector3[] points)
    {
        if (points.Length == 1)
            return points[0];

        Vector3[] newPoints = new Vector3[points.Length - 1];
        for (int i = 0; i < points.Length - 1; i++)
        {
            newPoints[i] = Vector3.Lerp(points[i], points[i + 1], t);
        }

        return CalculateBezierPoint(t, newPoints);
    }

    private float ApproximateCurveLength(Vector3[] points)
    {
        float length = 0f;
        Vector3 prevPoint = points[0];

        for (int i = 1; i <= samplePoints; i++)
        {
            float t = i / (float)samplePoints;
            Vector3 currentPoint = CalculateBezierPoint(t, points);
            length += Vector3.Distance(prevPoint, currentPoint);
            prevPoint = currentPoint;
        }

        return length;
    }
}
