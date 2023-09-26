using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Debugging
{
    public class UpdateAllContentSizeFitter : MonoBehaviour
    {
        private ContentSizeFitter[] _filters;

        public async void UpdateCoupleTimes()
        {
            _filters = GetComponentsInChildren<ContentSizeFitter>();
            int updateCount = 1;
            for (int i = 0; i < updateCount; i++)
            {
                await Task.Delay(10);
                UpdateFilters();
                Debug.Log("Update Chat UI Couple Times");
            }
        }

        public void UpdateFilters()
        {
            foreach (var contentSizeFitter in _filters)
            {
                contentSizeFitter.enabled = false;
            }

            foreach (var contentSizeFitter in _filters)
            {
                contentSizeFitter.enabled = true;
            }
        }
    }
}