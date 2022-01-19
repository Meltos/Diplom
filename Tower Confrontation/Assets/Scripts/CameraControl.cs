using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Camera cam; // ������
    [SerializeField] private float cam_sens_rotate; // �������� ��������
    [SerializeField] private float cam_sens_move; // �������� ��������
    [SerializeField] private float cam_wheel;

    private float camX;
    private float camY;
    private float camZ;
    private Transform camTr;
    private float ypos;
    private float oldX;
    private float oldZ;
    private float oldY;
    private float shag;

    #region MONO

    void Start()
    {
        shag = 0.6818263264242849f;
        camX = cam.transform.rotation.x;
        camY = cam.transform.rotation.y;
        ypos = 2;
        camTr = cam.transform;
        camZ = Vector3.Distance(Vector3.zero, camTr.position);
    }

    #endregion

    #region BODY

    void Update()
    {
        camX = 0f;
        camY = 0f;
        camZ = 0f;
        oldX = camTr.position.x;
        oldZ = camTr.position.z;
        oldY = camTr.position.y;
        // ����� ��������� ����
        // � ��������� - ����������� ����
        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");
        // � ������ �����
        float mW = Input.GetAxis("Mouse ScrollWheel");

        //// ���� ������ ������ �����
        //if (Input.GetAxis("Fire2") > 0)
        //{
        //    if (mX != 0)
        //    {
        //        camX += mX * cam_sens_rotate;
        //        camTr.transform.Rotate(Vector3.up, camX); // ������ ��� "�����"
        //    }
        //    if (mY != 0)
        //    {
        //        camY -= mY * cam_sens_rotate;
        //        camTr.transform.Rotate(Vector3.right, camY); // ������ �� ��� "������"
        //    }
        //}
        // ���� ������� ������� ������ ����
        if (mW != 0)
        {
            camZ = mW * cam_wheel;
            // ����� ���������:
            // �������� ��������� �������� ������ (���� �������� ������)
            // �� ���������� ������ "������" ��� "�����"[� ���������� �� ���������������� ��������� ������� �����]
            // � ���������� ��������� � ��������� ������
            camTr.transform.position += (Vector3)(camTr.transform.rotation * (camZ > 0 ? Vector3.forward * camZ : Vector3.back * (-camZ)));
            if (camTr.transform.position.y <= 1 || camTr.transform.position.y >= 10 || camTr.position.x >= 5 + (ypos+1) * shag || camTr.position.x <= -17 + (ypos - 1) * shag || camTr.position.z >= 12 + (ypos+1) * shag || camTr.position.z <= -14 + (ypos-1) * shag)
            {
                camTr.position = new Vector3(oldX, oldY, oldZ);
                ypos = oldY;
            }
            else
            {
                ypos = camTr.transform.position.y;
            }
        }

        // ���� ������ ������� �����
        if (Input.GetAxis("Fire3") > 0)
        {
            bool proverka = true;
            mX = -mX * cam_sens_move;
            mY = -mY * cam_sens_move;
            camTr.position += (Vector3)(camTr.rotation * (                      // ��������� ������ (���� �������� ������) �������� ��
                (mX > 0 ? Vector3.right * mX : Vector3.left * (-mX)) +          // ���������� ������ "�����"/"������" [� ���������� �� ���������������� ��������� ������] ����
                (mY > 0 ? Vector3.forward * mY : Vector3.back * (-mY))               // ���������� ������  "�����"/"����" [� ���������� �� ���������������� ��������� ������]
                ));                                                          // �� � ���� ������ ������, ����� position & rotation ��������� ������ ���� ���
            if (camTr.position.x >= 5 + ypos * shag || camTr.position.x <= -17 + ypos * shag)
            {
                camTr.position = new Vector3(oldX, oldY, camTr.position.z);
                proverka = false;
            }
            if (camTr.position.z >= 12 + ypos * shag || camTr.position.z <= -14 + ypos * shag) 
            {
                camTr.position = new Vector3(camTr.position.x, oldY, oldZ);
                proverka = false;
            }
            if (proverka == true)
            {
                camTr.position = new Vector3(camTr.position.x, ypos, camTr.position.z);
            }
        }
    }

    #endregion
}
