using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Instructions : MonoBehaviour
{

    public TextMeshProUGUI text;

    public float FadeInSpeed = 1;
    public float FadeOutSpeed = 1;
    public float textDelay = 1;

    // Start is called before the first frame update
    void Start()
    {
        //check if is touch
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            text.text = "Touch the screen to control the car";
        else    
            text.text = "Use   <voffset=0.3em><sprite=0><voffset=0em>and   <voffset=0.3em><sprite=1><voffset=0em>to control the car";
    
    }

    public void ShowInstructions()
    {
        StartCoroutine(FadeinAndOut());
    }



    IEnumerator FadeinAndOut()
    {
        Color txtColor = text.color;
        txtColor.a = 0;
        while (txtColor.a < 1)
        {
            txtColor.a = Mathf.MoveTowards(txtColor.a, 1, FadeInSpeed * Time.deltaTime);
            text.color = txtColor;
            yield return 0;
        }
        yield return new WaitForSeconds(textDelay);
        while(txtColor.a > 0)
        {
            txtColor.a = Mathf.MoveTowards(txtColor.a, 0, FadeOutSpeed * Time.deltaTime);
            text.color = txtColor;
            yield return 0;
        }
    }

}
