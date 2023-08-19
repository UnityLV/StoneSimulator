using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace Debugging
{
    public class UpdateAllContentSizeFitter : MonoBehaviour
    {
        public async void UpdateCoupleTimes()
        {
            int updateCount = 5;
            for (int i = 0; i < updateCount; i++)
            {
                await Task.Delay(10);
                UpdateFilters();
            }
        }

        public void UpdateFilters()
        {
            var filters = GetComponentsInChildren<ContentSizeFitter>();
            foreach (var contentSizeFitter in filters)
            {
                contentSizeFitter.enabled = false;
            }
            
            foreach (var contentSizeFitter in filters)
            {
                contentSizeFitter.enabled = true;
            }
        }
    }
}