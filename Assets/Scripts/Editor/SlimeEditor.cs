using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SlimeController))]
public class SlimeEditor : Editor
{
    private void OnSceneGUI()
    {
        SlimeController slime = (SlimeController)target;

        if (slime.patrolMode == SlimeController.PatrolMode.Linear)
        {
            // Handle Left Limit
            Vector3 leftPos = new Vector3(slime.leftLimit, slime.transform.position.y, slime.transform.position.z);
            EditorGUI.BeginChangeCheck();
            Vector3 newLeft = Handles.FreeMoveHandle(leftPos, 0.2f, Vector3.zero, Handles.SphereHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(slime, "Change Slime Left Limit");
                slime.leftLimit = newLeft.x;
            }

            // Handle Right Limit
            Vector3 rightPos = new Vector3(slime.rightLimit, slime.transform.position.y, slime.transform.position.z);
            EditorGUI.BeginChangeCheck();
            Vector3 newRight = Handles.FreeMoveHandle(rightPos, 0.2f, Vector3.zero, Handles.SphereHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(slime, "Change Slime Right Limit");
                slime.rightLimit = newRight.x;
            }

            Handles.Label(leftPos + Vector3.up * 0.4f, "Left Limit");
            Handles.Label(rightPos + Vector3.up * 0.4f, "Right Limit");
        }
        else if (slime.patrolPoints != null)
        {
            for (int i = 0; i < slime.patrolPoints.Length; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPos = Handles.FreeMoveHandle(slime.patrolPoints[i], 0.2f, Vector3.zero, Handles.SphereHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(slime, "Move Patrol Waypoint");
                    slime.patrolPoints[i] = newPos;
                }
                Handles.Label(slime.patrolPoints[i] + Vector3.up * 0.4f, "Waypoint " + i);
            }
        }
    }
}
