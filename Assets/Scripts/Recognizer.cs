using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Recognizer : MonoBehaviour
{
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public Transform movementSource;
    private float inputThreshold = 0.1f;
    private bool isMoving = false;
    private float distanceThreshold = 0.05f;

    public GameObject debugCubePrefab;
    public bool creationMode;
    public string newGestureName;

    private float recognitionThreshold = 0.8f;

    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { }
    public UnityStringEvent OnRecognized;

    private List<Gesture> trainingSet = new List<Gesture>();
    private List<Vector3> positionsList = new List<Vector3>();

    private string streamingAssetsPath = Application.streamingAssetsPath;

    void Awake()
    {
        BetterStreamingAssets.Initialize();
    }

    void Start()
    {
        /*string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
         foreach(var item in gestureFiles)
         {
             trainingSet.Add(GestureIO.ReadGestureFromFile(item));           
         }*/

        /* TextAsset[] xmlFiles = Resources.LoadAll<TextAsset>("");
         if (xmlFiles.Length > 0)
         {
             foreach (TextAsset item in xmlFiles)
             {
                 string gesture = item.ToString();
                 Debug.Log(gesture);
                 trainingSet.Add(GestureIO.ReadGestureFromXML(gesture));
             }
         }*/

        if (Application.platform == RuntimePlatform.Android)
            streamingAssetsPath = "jar:file://" + Application.dataPath + "!/assets";

        string[] gestureFiles = BetterStreamingAssets.GetFiles("/", "*.xml", SearchOption.TopDirectoryOnly);
        foreach (var item in gestureFiles)
        {
            StartCoroutine(GetXml(item));
            //trainingSet.Add(GestureIO.ReadGestureFromFile(streamingAssetsPath + "/" + item));
        }
    }

    
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreshold);
        //��� ����
        if (!isMoving && isPressed)
        {
            StartMovement();
        }
        //��� ����
        else if (isMoving && !isPressed)
        {
            EndMovement();
        }
        //��� ���� ��
        else if (isMoving && isPressed)
        {
            UpdateMovement();
        }
    }

    void StartMovement()
    {
        //Debug.Log("��ŸƮ");
        isMoving = true;
        positionsList.Clear();
        positionsList.Add(movementSource.position);
        if (debugCubePrefab)
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 0.6f);
    }
    void EndMovement()
    {
        //Debug.Log("����");
        isMoving = false;
        Point[] pointArray = new Point[positionsList.Count];
        if (pointArray.Length < 2)
            return;
        for (int i = 0; i < positionsList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }
        Gesture newGesture = new Gesture(pointArray);      
        if (creationMode) //������ ����
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);
            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }      
        else //������ �ν�
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log(result.GestureClass +  result.Score);
            if (result.Score > recognitionThreshold)
            {
                OnRecognized.Invoke(result.GestureClass);
            }
        }
    }
    void UpdateMovement()
    {
        //Debug.Log("�����̴� ��");
        Vector3 lastPosition = positionsList[positionsList.Count - 1];
        if (Vector3.Distance(movementSource.position, lastPosition) > distanceThreshold)
        {
            positionsList.Add(movementSource.position);
            if (debugCubePrefab)
                Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 2f);
        }
    }

    IEnumerator GetXml(string item)
    {
        //����Ͽ����� xml���� �ε尡 ���� �ʾ� UnityWebRequest�� ���
        using (UnityWebRequest request = UnityWebRequest.Get($"{streamingAssetsPath}/{item}"))
        {
            yield return request.SendWebRequest();
            string text = request.downloadHandler.text;
            //string gesture = text.ToString();
            trainingSet.Add(GestureIO.ReadGestureFromXML(text));
        }
    }
}
