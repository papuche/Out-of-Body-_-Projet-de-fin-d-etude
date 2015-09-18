using UnityEngine;
using System.Collections;

public class DepthImageViewer : MonoBehaviour 
{
	// the KinectManager instance
	private KinectManager manager;

	// the foreground texture
	private Texture2D foregroundTex;
	
	// rectangle taken by the foreground texture (in pixels)
	private Rect foregroundRect;
	private Vector2 foregroundOfs;

	// game objects to contain the joint colliders
	private GameObject[] jointColliders = null;
	private int numColliders = 0;

	private int depthImageWidth;
	private int depthImageHeight;
	

	void Start () 
	{
		manager = KinectManager.Instance;

		if(manager && manager.IsInitialized())
		{
			KinectInterop.SensorData sensorData = manager.GetSensorData();

			if(sensorData != null && sensorData.sensorInterface != null)
			{
				// get depth image size
				depthImageWidth = sensorData.depthImageWidth;
				depthImageHeight = sensorData.depthImageHeight;

				// calculate the foreground rectangle
				Rect cameraRect = Camera.main.pixelRect;
				float rectHeight = cameraRect.height;
				float rectWidth = cameraRect.width;
				
				if(rectWidth > rectHeight)
					rectWidth = rectHeight * depthImageWidth / depthImageHeight;
				else
					rectHeight = rectWidth * depthImageHeight / depthImageWidth;
				
				foregroundOfs = new Vector2((cameraRect.width - rectWidth) / 2, (cameraRect.height - rectHeight) / 2);
				foregroundRect = new Rect(foregroundOfs.x, cameraRect.height - foregroundOfs.y, rectWidth, -rectHeight);
				
				// create joint colliders
				numColliders = sensorData.jointCount;
				jointColliders = new GameObject[numColliders];
				
				for(int i = 0; i < numColliders; i++)
				{
					string sColObjectName = ((KinectInterop.JointType)i).ToString() + "Collider";
					jointColliders[i] = new GameObject(sColObjectName);
					jointColliders[i].transform.parent = transform;
					
					SphereCollider collider = jointColliders[i].AddComponent<SphereCollider>();
					collider.radius = 1f;
				}
			}
		}

	}
	
	void Update () 
	{
		// get the users texture
		if(manager && manager.IsInitialized())
		{
			foregroundTex = manager.GetUsersLblTex();
		}

		if(manager && manager.IsUserDetected())
		{
			long userId = manager.GetPrimaryUserID();

			// update colliders
			for(int i = 0; i < numColliders; i++)
			{
				if(manager.IsJointTracked(userId, i))
				{
					Vector3 posJoint = manager.GetJointKinectPosition(userId, i);

					if(posJoint != Vector3.zero)
					{
						// convert the joint 3d position to depth 2d coordinates
						Vector2 posDepth = manager.MapSpacePointToDepthCoords(posJoint);
						
						float scaledX = posDepth.x * foregroundRect.width / depthImageWidth;
						float scaledY = posDepth.y * -foregroundRect.height / depthImageHeight;

						float screenX = foregroundOfs.x + scaledX;
						float screenY = Camera.main.pixelHeight - (foregroundOfs.y + scaledY);
						float zDistance = posJoint.z - Camera.main.transform.position.z;
						
						Vector3 posScreen = new Vector3(screenX, screenY, zDistance);
						Vector3 posCollider = Camera.main.ScreenToWorldPoint(posScreen);

						jointColliders[i].transform.position = posCollider;
					}
				}
			}
		}

	}

	void OnGUI()
	{
		if(foregroundTex)
		{
			GUI.DrawTexture(foregroundRect, foregroundTex);
		}
	}

}
