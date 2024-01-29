using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class Tooltip : MonoBehaviour
{
    public Transform start;
    public Transform end;

    [Tooltip("Draw the line with an offset to this GameObject's origin")]
    public Vector3 offset = new Vector3(0, -0.05f, 0);

    private LineRenderer line;

    private void Awake()
    {
        line = this.GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(end.position.x, end.position.y + 0.3f, end.position.z);
        line.SetPosition(0, start.position + offset);
        line.SetPosition(1, end.position);

        //Rotate the label so that it always faces the user
        this.transform.LookAt(Camera.main.transform);
        
        this.transform.Rotate(Vector3.up, 180);
    }
}
