using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(EnemyController))]

public class EnemyControllerEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyController controller = (EnemyController)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(controller.transform.position, Vector3.up, Vector3.forward, 360, controller.radius);

        Vector3 viewAngle1 = DirectionFromAngle(controller.transform.eulerAngles.y, -controller.angle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(controller.transform.eulerAngles.y, controller.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(controller.transform.position, controller.transform.position + viewAngle1 * controller.radius);
        Handles.DrawLine(controller.transform.position, controller.transform.position + viewAngle2 * controller.radius);

        if (controller.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(controller.transform.position, controller.playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
