using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advice : MonoBehaviour
{
    public void setActive()
    {
        StartCoroutine(Order());

    }

    IEnumerator Order()
    {
        this.gameObject.SetActive(true);
        yield return new WaitForSeconds(10.0f);
        this.gameObject.SetActive(false);
    }

    public void setText(string Text)
    {
        this.gameObject.GetComponent<UnityEngine.UI.Text>().text = Text;
    }
}
