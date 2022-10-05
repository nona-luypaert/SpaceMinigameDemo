using UnityEngine;

public class DrawPath : MonoBehaviour
{
    public Transform[] paths;
    public int currentPath;
    private Vector2 _gizmoPosition;

    private void OnDrawGizmos()
    {
        Vector2 p0 = paths[currentPath].GetChild(0).position;
        Vector2 p1 = paths[currentPath].GetChild(1).position;
        Vector2 p2 = paths[currentPath].GetChild(2).position;
        Vector2 p3 = paths[currentPath].GetChild(3).position;
        
        for (float t = 0; t <= 1; t += 0.05f)
        {
            _gizmoPosition = Mathf.Pow(1 - t, 3) * p0 
                             + 3 * Mathf.Pow(1 - t, 2) * t * p1 
                             + 3 * (1 - t) * Mathf.Pow(t, 2) * p2 
                             + Mathf.Pow(t, 3) * p3;

            switch (currentPath)
            {
                case 0 :
                    Gizmos.color = Color.cyan;
                    break;
                case 1 :
                    Gizmos.color = Color.magenta;
                    break;
                case 2 : 
                    Gizmos.color = Color.green;
                    break;
            }
            
            Gizmos.DrawSphere(_gizmoPosition, 5f);
        }
        
        Gizmos.DrawLine(new Vector2(p0.x, p0.y), new Vector2(p1.x, p1.y));
        Gizmos.DrawLine(new Vector2(p2.x, p2.y), new Vector2(p3.x, p3.y));
    }
}
