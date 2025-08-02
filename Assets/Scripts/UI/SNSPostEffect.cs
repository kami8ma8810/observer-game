using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JusticeManGO.UI
{
    public class SNSPostEffect : MonoBehaviour
    {
        [System.Serializable]
        public class PostConfiguration
        {
            public string Message;
            public string ImageDescription;
            public string[] Tags;
        }

        [System.Serializable]
        public class PostData
        {
            public string Message;
            public string ImageDescription;
            public string[] Tags;
            public DateTime PostTime;
            public int PostId;
        }

        [System.Serializable]
        public class ReactionData
        {
            public int Likes;
            public int Retweets;
            public List<Comment> Comments;
        }

        [System.Serializable]
        public class Comment
        {
            public string Text;
            public string Username;
            public bool IsNegative;
            public DateTime CommentTime;
        }

        private bool isAnimating = false;
        private GameObject postUI;
        
        private readonly string[] positiveComments = {
            "よくやった！",
            "こういう人がいるから街が汚くなる",
            "もっと取り締まるべき",
            "正義の味方だ！",
            "私も見かけたら通報します"
        };
        
        private readonly string[] negativeComments = {
            "やりすぎでは？",
            "正義マン乙",
            "他にやることないの？",
            "監視社会怖い",
            "もっと寛容になれよ"
        };
        
        private readonly string[] neutralComments = {
            "難しい問題だね",
            "気持ちはわかる",
            "うーん...",
            "なるほど",
            "そうなんだ"
        };
        
        private readonly string[] trendingTags = {
            "#正義マン",
            "#マナー違反",
            "#監視社会",
            "#SNS通報",
            "#街の安全"
        };

        private int nextPostId = 1;

        public void Initialize()
        {
            CreatePostUI();
        }

        private void CreatePostUI()
        {
            if (postUI == null)
            {
                postUI = new GameObject("PostUI");
                postUI.transform.SetParent(transform, false);
                postUI.SetActive(false);
            }
        }

        public PostData CreatePost(PostConfiguration config)
        {
            return new PostData
            {
                Message = config.Message,
                ImageDescription = config.ImageDescription,
                Tags = config.Tags ?? new string[0],
                PostTime = DateTime.Now,
                PostId = nextPostId++
            };
        }

        public ReactionData SimulateReactions(PostData post, float successRate, float backlashRate)
        {
            var reactions = new ReactionData
            {
                Comments = new List<Comment>()
            };
            
            int baseLikes = UnityEngine.Random.Range(10, 50);
            reactions.Likes = Mathf.RoundToInt(baseLikes * (1 + successRate));
            
            float retweetChance = successRate * 0.5f;
            if (UnityEngine.Random.value < retweetChance)
            {
                reactions.Retweets = UnityEngine.Random.Range(1, reactions.Likes / 2);
            }
            
            int commentCount = UnityEngine.Random.Range(3, 10);
            reactions.Comments = GenerateComments(backlashRate, commentCount);
            
            return reactions;
        }

        public List<Comment> GenerateComments(float backlashRate, int count)
        {
            var comments = new List<Comment>();
            
            for (int i = 0; i < count; i++)
            {
                float random = UnityEngine.Random.value;
                string text;
                bool isNegative;
                
                if (random < backlashRate)
                {
                    text = negativeComments[UnityEngine.Random.Range(0, negativeComments.Length)];
                    isNegative = true;
                }
                else if (random < backlashRate + 0.3f)
                {
                    text = neutralComments[UnityEngine.Random.Range(0, neutralComments.Length)];
                    isNegative = false;
                }
                else
                {
                    text = positiveComments[UnityEngine.Random.Range(0, positiveComments.Length)];
                    isNegative = false;
                }
                
                comments.Add(new Comment
                {
                    Text = text,
                    Username = GenerateUsername(),
                    IsNegative = isNegative,
                    CommentTime = DateTime.Now.AddSeconds(UnityEngine.Random.Range(10, 300))
                });
            }
            
            return comments.OrderBy(c => c.CommentTime).ToList();
        }

        private string GenerateUsername()
        {
            string[] prefixes = { "user", "justice", "citizen", "observer", "watcher" };
            string prefix = prefixes[UnityEngine.Random.Range(0, prefixes.Length)];
            int number = UnityEngine.Random.Range(100, 9999);
            return $"{prefix}{number}";
        }

        public bool PlayPostAnimation(PostConfiguration config, float duration)
        {
            if (isAnimating) return false;
            
            StartCoroutine(AnimatePost(config, duration));
            return true;
        }

        private IEnumerator AnimatePost(PostConfiguration config, float duration)
        {
            isAnimating = true;
            
            if (postUI != null)
            {
                postUI.SetActive(true);
            }
            
            yield return new WaitForSeconds(duration);
            
            if (postUI != null)
            {
                postUI.SetActive(false);
            }
            
            isAnimating = false;
        }

        public string[] GetTrendingHashtags()
        {
            return trendingTags.OrderBy(x => UnityEngine.Random.value).Take(3).ToArray();
        }

        public float CalculateViralPotential(PostData post, bool goodTiming)
        {
            float potential = 0.3f;
            
            if (post.Tags != null && post.Tags.Length > 0)
            {
                foreach (var tag in post.Tags)
                {
                    if (trendingTags.Contains(tag))
                    {
                        potential += 0.1f;
                    }
                }
            }
            
            if (goodTiming)
            {
                potential += 0.3f;
            }
            
            if (post.Message.Length > 50 && post.Message.Length < 200)
            {
                potential += 0.1f;
            }
            
            return Mathf.Clamp01(potential);
        }

        public string FormatNumber(int number)
        {
            if (number >= 1000000)
            {
                return $"{number / 1000000f:0.#}M";
            }
            else if (number >= 10000)
            {
                return $"{number / 1000}K";
            }
            else if (number >= 1000)
            {
                return $"{number / 1000f:0.#}K";
            }
            else
            {
                return number.ToString();
            }
        }

        public bool IsAnimating()
        {
            return isAnimating;
        }
    }
}