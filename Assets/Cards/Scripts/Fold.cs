using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class Fold : MonoBehaviour
    {
        public static Fold FoldStatic;

        private void Start()
        {
            FoldStatic = this;
        }

        public void CardsDie(Transform card)
        {
            StartCoroutine(MoveToFold(card));
        }

        public IEnumerator MoveToFold(Transform card)
        {
            while (Vector3.Distance(card.position, transform.position) >= 1)
            {
                card.position = Vector3.Lerp(card.position, transform.position, (float)0.6*Time.deltaTime);
                yield return null;
            }
            StopCoroutine(MoveToFold(card));
        }
    }
}
