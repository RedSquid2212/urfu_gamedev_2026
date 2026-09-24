using System;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left,
    Right,
}

public class CubesController : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    
    [SerializeField] private int radius;

    [SerializeField] private int speed;
    
    [SerializeField] private Direction direction;

    private const int cubesCount = 8;

    private readonly List<Transform> cubes = new();
    private int lastRadius = 0;
    private void Awake()
    {
        for (var i = 0; i < cubesCount; i++)
        {
            var cube = Instantiate(cubePrefab, transform);
            cubes.Add(cube.transform);
        }
        
        LayoutCubes();
    }
    
    private void Update()
    {
        if (Mathf.Abs(radius - lastRadius) >= 1)
        {
            LayoutCubes();
        }
        
        var yAngle = direction == Direction.Left ? speed * Time.deltaTime : -speed * Time.deltaTime;
        transform.Rotate(0, yAngle, 0);
    }

    private void LayoutCubes()
    {
        var step = Mathf.PI * 2 / cubes.Count;

        for (var i = 0; i < cubes.Count; i++)
        {
            var angle = i * step;
            var offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            cubes[i].localPosition = offset;
        }

        lastRadius = radius;
    }
}
