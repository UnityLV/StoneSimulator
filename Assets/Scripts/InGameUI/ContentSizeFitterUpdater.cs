using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace ChatDB.PinMessage
{
    [RequireComponent(typeof(ContentSizeFitter))]
    public class ContentSizeFitterUpdater : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(UpdateContentSizeFilter());
        }


        private IEnumerator UpdateContentSizeFilter()
        {
            ContentSizeFitter contentSizeFitter = GetComponent<ContentSizeFitter>();

            int framesToUpdate = 5;

            for (int i = 0; i <  framesToUpdate; i++)
            {
                contentSizeFitter.enabled = false;
                yield return null;
                contentSizeFitter.enabled = true;
            }
        }
    }
}