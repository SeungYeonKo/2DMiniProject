using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    public Image HowToImage; 
    private bool _isTrigger;

    private void Start()
    {
        _isTrigger = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isTrigger)
        {
            _isTrigger = true;
            Debug.Log("HowToPlay!");
            HowToImage.gameObject.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _isTrigger)
        {
            _isTrigger = false;
            Debug.Log("나감!");
            HowToImage.gameObject.SetActive(false);
        }

    }

}