using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

//EnemyFOV 스크립트를 활용하기위한 커스텀에디터임을 명시
[CustomEditor(typeof(Monster))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        Monster fov = (Monster)target;
        //시야각(원주) 시작점의 좌표를 계산(주어진 각도의 1/2 지점부터)
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        Handles.color = Color.green;
        //원점좌표, 노멀벡터, 원의 반지름 사이즈
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.viewRange);

        Handles.color = new Color(255, 1, 1, 0.2f);
        //SolidArd = 부채꼴
        //원점좌표, 노멀벡터, 부채꼴 시작 각도, 부채꼴 각도, 부채꼴 반지름
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.viewAngle, fov.viewRange);

        Handles.Label(fov.transform.position + (fov.transform.forward * 2f), fov.viewAngle.ToString());
    }
}
#endif
