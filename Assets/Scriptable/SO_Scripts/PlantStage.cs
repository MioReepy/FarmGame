using System.Collections;
using UnityEngine;

namespace InventorySpace
{
    public class PlantStage: MonoBehaviour
    {
        [SerializeField] private float _duration = 10f;
        internal bool _isReady;

        private void Start()
        {
            StartCoroutine(ChangeStage());
        }

        private IEnumerator ChangeStage()
        {
            for (int i = 0; i < gameObject.transform.childCount - 1; i++)
            {
                yield return new WaitForSeconds(_duration);
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
                gameObject.transform.GetChild(i + 1).gameObject.SetActive(true);

                if (i == gameObject.transform.childCount - 2)
                {
                    _isReady = true;
                    Debug.Log(_isReady);
                }
            }
        }
    }
}