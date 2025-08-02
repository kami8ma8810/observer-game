using NUnit.Framework;
using UnityEngine;
using JusticeManGO.Camera;

namespace JusticeManGO.Tests.EditMode
{
    public class CameraFollowControllerTests
    {
        private CameraFollowController cameraController;
        private GameObject cameraGameObject;
        private GameObject targetGameObject;

        [SetUp]
        public void SetUp()
        {
            cameraGameObject = new GameObject("TestCamera");
            cameraGameObject.AddComponent<UnityEngine.Camera>();
            
            targetGameObject = new GameObject("TestTarget");
            
            cameraController = cameraGameObject.AddComponent<CameraFollowController>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(cameraGameObject);
            Object.DestroyImmediate(targetGameObject);
        }

        [Test]
        public void SetTarget_ShouldSetFollowTarget()
        {
            cameraController.SetTarget(targetGameObject.transform);
            
            Assert.AreEqual(targetGameObject.transform, cameraController.Target);
        }

        [Test]
        public void FollowTarget_WhenTargetMovesRight_CameraXPositionShouldFollow()
        {
            cameraController.SetTarget(targetGameObject.transform);
            cameraController.SetFollowOffset(new Vector3(0, 0, -10));
            
            targetGameObject.transform.position = new Vector3(5f, 0, 0);
            cameraController.UpdateCameraPosition();
            
            Assert.AreEqual(5f, cameraGameObject.transform.position.x, 0.01f);
        }

        [Test]
        public void FollowTarget_ShouldMaintainYPosition()
        {
            cameraController.SetTarget(targetGameObject.transform);
            cameraController.SetFollowOffset(new Vector3(0, 2f, -10));
            float initialY = 2f;
            
            targetGameObject.transform.position = new Vector3(5f, 1f, 0);
            cameraController.UpdateCameraPosition();
            
            Assert.AreEqual(initialY, cameraGameObject.transform.position.y, 0.01f);
        }

        [Test]
        public void FollowTarget_ShouldRespectBounds()
        {
            cameraController.SetTarget(targetGameObject.transform);
            cameraController.SetBounds(new Bounds(Vector3.zero, new Vector3(10f, 10f, 0)));
            cameraController.SetFollowOffset(new Vector3(0, 0, -10));
            
            targetGameObject.transform.position = new Vector3(15f, 0, 0);
            cameraController.UpdateCameraPosition();
            
            Assert.LessOrEqual(cameraGameObject.transform.position.x, 5f);
        }

        [Test]
        public void FollowTarget_WithSmoothingEnabled_ShouldLerpPosition()
        {
            cameraController.SetTarget(targetGameObject.transform);
            cameraController.SetFollowOffset(new Vector3(0, 0, -10));
            cameraController.EnableSmoothing(true, 0.1f);
            
            Vector3 initialPosition = cameraGameObject.transform.position;
            targetGameObject.transform.position = new Vector3(10f, 0, 0);
            
            cameraController.UpdateCameraPosition();
            
            float distance = Vector3.Distance(initialPosition, cameraGameObject.transform.position);
            Assert.Greater(distance, 0f);
            Assert.Less(cameraGameObject.transform.position.x, 10f);
        }
    }
}