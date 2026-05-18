using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyController), true)]
public class EnemyEditor : Editor
{
    private SerializedProperty patrolMode;
    private SerializedProperty contactDamage;
    private SerializedProperty moveSpeed;

    // Linear properties
    private SerializedProperty leftOffset;
    private SerializedProperty rightOffset;
    private SerializedProperty isMovingRight;

    // Waypoint properties
    private SerializedProperty patrolPoints;
    private SerializedProperty currentWaypoint;
    private SerializedProperty pointStopThreshold;

    // Edge Detection properties
    private SerializedProperty detectionPoint;
    private SerializedProperty wallCheckDistance;
    private SerializedProperty floorCheckDistance;
    private SerializedProperty whatIsGround;
    private SerializedProperty turnCooldown;

    private void OnEnable()
    {
        patrolMode = serializedObject.FindProperty("patrolMode");
        contactDamage = serializedObject.FindProperty("contactDamage");
        moveSpeed = serializedObject.FindProperty("moveSpeed");

        leftOffset = serializedObject.FindProperty("leftOffset");
        rightOffset = serializedObject.FindProperty("rightOffset");
        isMovingRight = serializedObject.FindProperty("isMovingRight");

        patrolPoints = serializedObject.FindProperty("patrolPoints");
        currentWaypoint = serializedObject.FindProperty("currentWaypoint");
        pointStopThreshold = serializedObject.FindProperty("pointStopThreshold");

        detectionPoint = serializedObject.FindProperty("detectionPoint");
        wallCheckDistance = serializedObject.FindProperty("wallCheckDistance");
        floorCheckDistance = serializedObject.FindProperty("floorCheckDistance");
        whatIsGround = serializedObject.FindProperty("whatIsGround");
        turnCooldown = serializedObject.FindProperty("turnCooldown");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Base Enemy Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(contactDamage);
        EditorGUILayout.PropertyField(moveSpeed);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Patrol Configuration", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(patrolMode);

        EnemyController.PatrolMode mode = (EnemyController.PatrolMode)patrolMode.enumValueIndex;

        EditorGUI.indentLevel++;
        switch (mode)
        {
            case EnemyController.PatrolMode.Linear:
                EditorGUILayout.PropertyField(leftOffset);
                EditorGUILayout.PropertyField(rightOffset);
                EditorGUILayout.PropertyField(isMovingRight);
                break;

            case EnemyController.PatrolMode.Waypoints:
                EditorGUILayout.PropertyField(patrolPoints);
                EditorGUILayout.PropertyField(currentWaypoint);
                EditorGUILayout.PropertyField(pointStopThreshold);
                break;

            case EnemyController.PatrolMode.EdgeDetection:
                EditorGUILayout.PropertyField(detectionPoint);
                EditorGUILayout.PropertyField(wallCheckDistance);
                EditorGUILayout.PropertyField(floorCheckDistance);
                EditorGUILayout.PropertyField(whatIsGround);
                EditorGUILayout.PropertyField(turnCooldown);
                break;

            case EnemyController.PatrolMode.ChasePlayer:
                EditorGUILayout.HelpBox("Enemy will move towards the Player when active.", MessageType.Info);
                break;
        }
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        EnemyController enemy = (EnemyController)target;

        if (enemy.patrolMode == EnemyController.PatrolMode.Linear)
        {
            Vector3 center = enemy.transform.position;

            // Handle Left Limit (Relative)
            Vector3 leftPos = new Vector3(center.x + enemy.leftOffset, center.y, center.z);
            EditorGUI.BeginChangeCheck();
            Vector3 newLeft = Handles.FreeMoveHandle(leftPos, 0.2f, Vector3.zero, Handles.SphereHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(enemy, "Change Enemy Left Offset");
                enemy.leftOffset = newLeft.x - center.x;
            }

            // Handle Right Limit (Relative)
            Vector3 rightPos = new Vector3(center.x + enemy.rightOffset, center.y, center.z);
            EditorGUI.BeginChangeCheck();
            Vector3 newRight = Handles.FreeMoveHandle(rightPos, 0.2f, Vector3.zero, Handles.SphereHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(enemy, "Change Enemy Right Offset");
                enemy.rightOffset = newRight.x - center.x;
            }

            Handles.Label(leftPos + Vector3.up * 0.4f, "Left Offset");
            Handles.Label(rightPos + Vector3.up * 0.4f, "Right Offset");
        }
        else if (enemy.patrolPoints != null && enemy.patrolMode == EnemyController.PatrolMode.Waypoints)
        {
            for (int i = 0; i < enemy.patrolPoints.Length; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPos = Handles.FreeMoveHandle(enemy.patrolPoints[i], 0.2f, Vector3.zero, Handles.SphereHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(enemy, "Move Patrol Waypoint");
                    enemy.patrolPoints[i] = newPos;
                }
                Handles.Label(enemy.patrolPoints[i] + Vector3.up * 0.4f, "Waypoint " + i);
            }
        }
    }
}



