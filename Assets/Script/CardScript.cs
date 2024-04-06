using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    [SerializeField] Shapes shapeIndicator;
    [SerializeField] GameObject highlight;
    Animator animator;
    public GameObject shape;
    public bool highlighted;
    int r, c;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        highlighted = false;
        highlight.SetActive(false);
    }

    public void SetShape(GameObject newShape, int index)
    {
        shapeIndicator = (Shapes)index;
        shape = Instantiate(newShape , new Vector3(transform.position.x,transform.position.y +1,transform.position.z),Quaternion.identity);
        shape.SetActive(false);
    }

    public void SetLocation(int row,int col)
    {
        r = row;
        c= col;
    }

    public void Flip()
    {
        animator.SetBool("Flip", true);
        Invoke(nameof(ShowShape), 0.35f);

    }

    public void SetBack()
    {
        highlight.SetActive(false);
        animator.SetBool("Flip", false);
        shape.SetActive(false);
        highlighted = false;
    }

    void ShowShape()
    {
        shape.SetActive(true);
    }

    public void RemoveFromBoard()
    {
        Destroy(shape);
        Destroy(gameObject);
    }

    public int GetRow()
    {
        return r;
    }

    public int GetCol()
    {
        return c;
    }
    private void OnMouseDown()
    {
        if (BoardScript.Instance.playable && !highlighted)
        {
            highlighted = true;
            highlight.SetActive(true);
            BoardScript.Instance.HighLightCard(r, c);
        }
    }

}
