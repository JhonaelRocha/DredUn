using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public int currentItems;
    private int maxItems;

    public Text itemsLabelText;

    void Start()
    {
        maxItems = GameObject.FindGameObjectsWithTag("Item").Length;
        AtualizarItemsLabel();
    }

    void Update()
    {
        if(currentItems == maxItems)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void AtualizarItemsLabel()
    {
        itemsLabelText.text = $"{currentItems} / {maxItems}";
    }
}
