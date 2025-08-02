using NUnit.Framework;
using UnityEngine;
using JusticeManGO.Player;

namespace JusticeManGO.Tests.EditMode
{
    public class PlayerMovementControllerTests
    {
        private PlayerMovementController playerController;
        private GameObject playerGameObject;

        [SetUp]
        public void SetUp()
        {
            playerGameObject = new GameObject("TestPlayer");
            playerController = playerGameObject.AddComponent<PlayerMovementController>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(playerGameObject);
        }

        [Test]
        public void Move_WithPositiveInput_ShouldMoveRight()
        {
            float moveSpeed = 5f;
            playerController.SetMoveSpeed(moveSpeed);
            
            Vector3 initialPosition = playerGameObject.transform.position;
            playerController.Move(1f, 0.1f);
            
            Assert.Greater(playerGameObject.transform.position.x, initialPosition.x);
        }

        [Test]
        public void Move_WithNegativeInput_ShouldMoveLeft()
        {
            float moveSpeed = 5f;
            playerController.SetMoveSpeed(moveSpeed);
            
            Vector3 initialPosition = playerGameObject.transform.position;
            playerController.Move(-1f, 0.1f);
            
            Assert.Less(playerGameObject.transform.position.x, initialPosition.x);
        }

        [Test]
        public void Move_WithZeroInput_ShouldNotMove()
        {
            playerController.SetMoveSpeed(5f);
            
            Vector3 initialPosition = playerGameObject.transform.position;
            playerController.Move(0f, 0.1f);
            
            Assert.AreEqual(initialPosition, playerGameObject.transform.position);
        }

        [Test]
        public void SetMoveSpeed_ShouldUpdateSpeed()
        {
            float newSpeed = 10f;
            playerController.SetMoveSpeed(newSpeed);
            
            Assert.AreEqual(newSpeed, playerController.MoveSpeed);
        }

        [Test]
        public void Move_ShouldRespectMovementBounds()
        {
            playerController.SetMoveSpeed(5f);
            playerController.SetMovementBounds(-5f, 5f);
            
            playerGameObject.transform.position = new Vector3(4.9f, 0, 0);
            playerController.Move(1f, 1f);
            
            Assert.LessOrEqual(playerGameObject.transform.position.x, 5f);
        }

        [Test]
        public void IsMoving_WhenMoving_ShouldReturnTrue()
        {
            playerController.SetMoveSpeed(5f);
            playerController.Move(1f, 0.1f);
            
            Assert.IsTrue(playerController.IsMoving);
        }

        [Test]
        public void IsMoving_WhenNotMoving_ShouldReturnFalse()
        {
            playerController.SetMoveSpeed(5f);
            playerController.Move(0f, 0.1f);
            
            Assert.IsFalse(playerController.IsMoving);
        }

        [Test]
        public void GetFacingDirection_AfterMovingRight_ShouldReturn1()
        {
            playerController.SetMoveSpeed(5f);
            playerController.Move(1f, 0.1f);
            
            Assert.AreEqual(1, playerController.FacingDirection);
        }

        [Test]
        public void GetFacingDirection_AfterMovingLeft_ShouldReturnMinus1()
        {
            playerController.SetMoveSpeed(5f);
            playerController.Move(-1f, 0.1f);
            
            Assert.AreEqual(-1, playerController.FacingDirection);
        }
    }
}