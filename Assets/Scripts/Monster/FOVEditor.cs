using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

//EnemyFOV ��ũ��Ʈ�� Ȱ���ϱ����� Ŀ���ҿ��������� ����
[CustomEditor(typeof(Monster))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        Monster fov = (Monster)target;
        //�þ߰�(����) �������� ��ǥ�� ���(�־��� ������ 1/2 ��������)
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        Handles.color = Color.green;
        //������ǥ, ��ֺ���, ���� ������ ������
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.viewRange);

        Handles.color = new Color(255, 1, 1, 0.2f);
        //SolidArd = ��ä��
        //������ǥ, ��ֺ���, ��ä�� ���� ����, ��ä�� ����, ��ä�� ������
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.viewAngle, fov.viewRange);

        Handles.Label(fov.transform.position + (fov.transform.forward * 2f), fov.viewAngle.ToString());
    }
}
#endif
