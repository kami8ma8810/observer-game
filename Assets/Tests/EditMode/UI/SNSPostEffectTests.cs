using NUnit.Framework;
using UnityEngine;
using JusticeManGO.UI;
using System.Collections.Generic;

namespace JusticeManGO.Tests.EditMode
{
    public class SNSPostEffectTests
    {
        private SNSPostEffect postEffect;
        private GameObject effectObject;

        [SetUp]
        public void SetUp()
        {
            effectObject = new GameObject("TestSNSEffect");
            postEffect = effectObject.AddComponent<SNSPostEffect>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(effectObject);
        }

        [Test]
        public void CreatePost_ShouldReturnPostData()
        {
            var config = new SNSPostEffect.PostConfiguration
            {
                Message = "路上喫煙を発見！",
                ImageDescription = "喫煙者の写真",
                Tags = new string[] { "#正義マン", "#路上喫煙", "#マナー違反" }
            };
            
            var post = postEffect.CreatePost(config);
            
            Assert.IsNotNull(post);
            Assert.AreEqual(config.Message, post.Message);
            Assert.AreEqual(3, post.Tags.Length);
        }

        [Test]
        public void SimulateReactions_ShouldGenerateRandomReactions()
        {
            var post = new SNSPostEffect.PostData
            {
                Message = "テスト投稿",
                PostTime = System.DateTime.Now
            };
            
            var reactions = postEffect.SimulateReactions(post, 0.7f, 0.3f);
            
            Assert.Greater(reactions.Likes, 0);
            Assert.GreaterOrEqual(reactions.Retweets, 0);
            Assert.IsNotNull(reactions.Comments);
        }

        [Test]
        public void GenerateComments_WithHighBacklash_ShouldReturnNegativeComments()
        {
            var comments = postEffect.GenerateComments(0.8f, 5);
            
            int negativeCount = 0;
            foreach (var comment in comments)
            {
                if (comment.IsNegative)
                    negativeCount++;
            }
            
            Assert.Greater(negativeCount, comments.Count / 2);
        }

        [Test]
        public void GenerateComments_WithLowBacklash_ShouldReturnPositiveComments()
        {
            var comments = postEffect.GenerateComments(0.1f, 5);
            
            int positiveCount = 0;
            foreach (var comment in comments)
            {
                if (!comment.IsNegative)
                    positiveCount++;
            }
            
            Assert.Greater(positiveCount, comments.Count / 2);
        }

        [Test]
        public void PlayPostAnimation_ShouldTriggerAnimation()
        {
            postEffect.Initialize();
            
            var config = new SNSPostEffect.PostConfiguration
            {
                Message = "アニメーションテスト"
            };
            
            bool animationStarted = postEffect.PlayPostAnimation(config, 2f);
            
            Assert.IsTrue(animationStarted);
            Assert.IsTrue(postEffect.IsAnimating());
        }

        [Test]
        public void GetTrendingHashtags_ShouldReturnPopularTags()
        {
            var tags = postEffect.GetTrendingHashtags();
            
            Assert.IsNotNull(tags);
            Assert.Greater(tags.Length, 0);
            Assert.IsTrue(tags[0].StartsWith("#"));
        }

        [Test]
        public void CalculateViralPotential_WithGoodTiming_ShouldReturnHighValue()
        {
            var post = new SNSPostEffect.PostData
            {
                Message = "話題の投稿",
                Tags = new string[] { "#トレンド", "#バズる" }
            };
            
            float potential = postEffect.CalculateViralPotential(post, true);
            
            Assert.Greater(potential, 0.5f);
        }

        [Test]
        public void FormatNumber_ShouldReturnFormattedString()
        {
            Assert.AreEqual("1.2K", postEffect.FormatNumber(1234));
            Assert.AreEqual("10K", postEffect.FormatNumber(10000));
            Assert.AreEqual("1.5M", postEffect.FormatNumber(1500000));
            Assert.AreEqual("999", postEffect.FormatNumber(999));
        }
    }
}