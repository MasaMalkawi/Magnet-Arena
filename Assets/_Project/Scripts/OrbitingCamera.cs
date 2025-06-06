﻿using UnityEngine;
using Photon.Pun; 


    public class OrbitingCamera : MonoBehaviour
    {
        public Transform target;
        public float verticalOffset = 0f;
        public float distance = 5f;
        public float sensitivity = 100f;

        private float yRot = 0f;
        private float xRot = 20f;

        private PhotonView photonView;

        private void Start()
        {
            sensitivity *= 10f;

           
            photonView = target.GetComponentInParent<PhotonView>();

            
            if (photonView != null && !photonView.IsMine)
            {
                gameObject.SetActive(false);
            }
        }

        private void LateUpdate()
        {
            if (photonView == null || !photonView.IsMine)
                return;

            yRot += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            xRot -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            xRot = Mathf.Clamp(xRot, 0f, 75f);

            Quaternion worldRotation = transform.parent != null ? transform.parent.rotation : Quaternion.FromToRotation(Vector3.up, target.up);
            Quaternion cameraRotation = worldRotation * Quaternion.Euler(xRot, yRot, 0f);
            Vector3 targetToCamera = cameraRotation * new Vector3(0f, 0f, -distance);

            transform.SetPositionAndRotation(target.TransformPoint(0f, verticalOffset, 0f) + targetToCamera, cameraRotation);
        }
    }

