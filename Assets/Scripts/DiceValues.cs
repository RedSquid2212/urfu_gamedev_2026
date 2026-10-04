using UnityEngine;

public class DiceValues : MonoBehaviour
{
    private int[] diceValues = { 4, 3, 2, 5, 1, 6 };

    private static readonly Vector3[] Normals =
    {
        Vector3.right, Vector3.left,
        Vector3.up,    Vector3.down,
        Vector3.forward, Vector3.back
    };

    public int GetTopValue()
    {
        var best = -2f;
        var index = 0;

        for (var i = 0; i < 6; i++)
        {
            var dot = Vector3.Dot(transform.TransformDirection(Normals[i]), Vector3.up);
            if (dot <= best) continue;
            best = dot;
            index = i;
        }

        return diceValues[index];
    }
}
