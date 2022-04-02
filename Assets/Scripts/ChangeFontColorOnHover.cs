using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFontColorOnHover : MonoBehaviour
{
    private Text buttonText;
    private Button button;

    public bool whiteText;

    // Start is called before the first frame update
    void Awake()
    {
        buttonText = this.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter()
    {
        buttonText.color = new Color(234f/255f, 196f/255f, 173f/255f, 128f);
    }

    public void OnPointerExit()
    {
        if (whiteText)
        {
            buttonText.color = Color.white;
        }
        else
        {
            buttonText.color = Color.black;

        }
    }
}
