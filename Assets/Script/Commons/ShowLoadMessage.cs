
using UnityEngine;

namespace UnityMugen.Interface
{

    public class ShowLoadMessage : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            Instantiate(Resources.Load<ShowLoadMessage>("object path"));
        }
    }
}