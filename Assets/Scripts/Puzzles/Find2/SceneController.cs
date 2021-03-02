using UnityEngine;
using System.Collections;

namespace _Quest.Find2
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private MemoryCard originalCard;
        [SerializeField] private Sprite[] images;
        [SerializeField] private Transform root;
        [SerializeField] private Transform[] cardPlaces;
        [SerializeField] private Puzzle puzzle;
        public bool isClone;
        private SceneController clone;

        public const int gridRows = 4; //2
        public const int gridCols = 4;
        public const float offsetX = 1.5f;
        public const float offsetZ = 1.65f;

        private MemoryCard _firstRevealed = null;
        private MemoryCard _secondRevealed = null;

        private int score = 0;
        private int winScore;
        // Use this for initialization
        void OnEnable()
        {
            if (!isClone)
            {
                clone = puzzle.clone.controller.GetComponent<SceneController>();
                clone.isClone = true;
                score = 0;
                //Vector3 startPos = originalCard.transform.localPosition;
                int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 }; //0-3
                winScore = numbers.Length;
                numbers = ShuffleArray(numbers);

                int i = 0;
                int j = 0;
                MemoryCard card = null;
                for (i = 0; i < gridCols; i++)
                {
                    for (j = 0; j < gridRows; j++)
                    {
                        int index = i * gridCols + j;
                        card = Instantiate(originalCard, cardPlaces[index]);
                        int id = numbers[index];
                        card.SetCard(id, images[id]);
                        //float posX = offsetX * i + startPos.x;
                        //float posZ = -(offsetZ * j) + startPos.z;
                        card.transform.position = cardPlaces[index].position;
                        card.transform.rotation = cardPlaces[index].rotation;
                        card.Controller = this;

                        MemoryCard cloneCard = Instantiate(originalCard, clone.cardPlaces[index]);
                        cloneCard.transform.position = clone.cardPlaces[index].position;
                        cloneCard.SetCard(id, images[id]);
                        cloneCard.transform.rotation = clone.cardPlaces[index].rotation;
                        cloneCard.Controller = clone;
                        card.clone = cloneCard;
                    }
                }
            }
            
        }

        private int[] ShuffleArray(int[] numbers)
        {
            int[] newArray = numbers.Clone() as int[];
            for (int i = 0; i < newArray.Length; i++)
            {
                int tmp = newArray[i];
                int r = Random.Range(i, newArray.Length);
                newArray[i] = newArray[r];
                newArray[r] = tmp;
            }
            return newArray;
        }

        public bool canReveal
        {
            get
            {
                return _secondRevealed == null;
            }
        }

        public void CardRevealed(MemoryCard card)
        {
            if (_firstRevealed == null)
            {
                _firstRevealed = card;
            }
            else
            {
                _secondRevealed = card;
                StartCoroutine(CheckMatch());
            }
        }

        private IEnumerator CheckMatch()
        {
            if (_firstRevealed.id != _secondRevealed.id)
            {
                yield return new WaitForSeconds(0.5f);
                _firstRevealed.Unreveal();
                _secondRevealed.Unreveal();
            }
            else
            {
                score += 2;

                if (score == winScore)
                {
                    puzzle.Solve();
                }
            }
            _firstRevealed = null;
            _secondRevealed = null;
        }
    }
}