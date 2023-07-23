using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen;

namespace UnityMugen.Interface
{

    public class ShowMessage : MonoBehaviour
    {
        public GameObject message;

        public Sprite none, ok, error, warning;

        private Vector3 anchoredPosition;
        private static int totalMessage;

        public void Message(TipePopupMessage tipePopupMessage, string _message)
        {
            GameObject messageInst = Instantiate(message);

            Animator animator = messageInst.GetComponent<Animator>();

            Image image = messageInst.transform.Find("TipeMessage").GetComponent<Image>();
            Text text = messageInst.GetComponentInChildren<Text>();

            if (tipePopupMessage == TipePopupMessage.None)
                image.sprite = none;
            if (tipePopupMessage == TipePopupMessage.OK)
                image.sprite = ok;
            else if (tipePopupMessage == TipePopupMessage.Error)
                image.sprite = error;
            else if (tipePopupMessage == TipePopupMessage.Warning)
                image.sprite = warning;

            text.text = _message;

            messageInst.transform.SetParent(transform, false);
            Vector3 posMessage = messageInst.transform.localPosition;
            messageInst.transform.localPosition = new Vector3(posMessage.x, posMessage.y - (52 * totalMessage), posMessage.z);
            totalMessage++;

            StartCoroutine(AutomaticDestroy(animator, messageInst));
        }

        IEnumerator AutomaticDestroy(Animator animator, GameObject messageInst)
        {
            yield return new WaitForSeconds(5);
            animator.Play("HideMessage", 0);
            yield return new WaitForSeconds(0.4f);
            Destroy(messageInst);

            totalMessage--;
        }

    }
}