using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FilpDir
{
    LeftToRight,
    RightToLeft
}

public class BookFilp : MonoBehaviour
{
  
    public RectTransform m_leftPageRectTrans;
    public RectTransform m_rightPageRectTrans;
    public Transform m_filpPageRoot;

    [Range(2,30)]
    [Header("翻页速度")]
    public float m_filpSpeedByDegree;



    private RectTransform m_rectTrans;


    #region 翻转专用
    bool m_isFilping;
    //翻转页的另一面
    private Transform m_filpPageAnotherFace;
    //将要展示的下一页或者是上一页
    private Transform m_willShowPage;
    private Transform m_willShowPageMask;


    #endregion

    private void Awake()
    {
        m_rectTrans = GetComponent<RectTransform>();
    }

    private void Start()
    {
    }

    void FilpBookPage(FilpDir filpDir)
    {
        //首先生成将要使用的翻转页
        GenerateFilpPage(filpDir);
        if (filpDir == FilpDir.LeftToRight)
            StartCoroutine("FilpFromLeftToRight");
        else
            StartCoroutine("FilpFromRightToLeft");
    }
    void GenerateFilpPage(FilpDir filpDir)
    {
        if (filpDir == FilpDir.LeftToRight)
        {
            //根据右边页，生成左边页的另一面
            m_filpPageAnotherFace = Instantiate(m_rightPageRectTrans, m_filpPageRoot);
            //因为锚点布局引起的拉伸，通过Reset其宽度来恢复大小
            m_filpPageAnotherFace.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_rightPageRectTrans.GetComponent<RectTransform>().rect.height);
            //因为恢复大小之后，由于锚点造成的自动居中可以通过再次修改其位置来进行修正
            m_filpPageAnotherFace.position = m_rightPageRectTrans.position;
            //旋转到指定位置，准备翻页
            m_filpPageAnotherFace.RotateAround(new Vector2(transform.position.x, transform.position.y), Vector3.forward, 180);

            //根据左边页生成下一页的部分
            //首先生成遮罩，遮罩的长度应该不小于书页的对角线长度（由于锚点设置会自动高度增加）
            m_willShowPageMask = Instantiate(m_leftPageRectTrans, m_filpPageRoot);
            //旋转到适当的位置
            m_willShowPageMask.RotateAround(new Vector2(transform.position.x, transform.position.y), Vector3.forward, 90);
            //设置其透明度,alpha值为1（只为了mask的作用）
            m_willShowPageMask.GetComponent<Image>().color *= new Vector4(1, 1, 1, 1.0f / 180);
            //添加Mask
            m_willShowPageMask.gameObject.AddComponent<Mask>();

            //创建下一页，也就是将要展示的右边那一页

            //首先是克隆，但是由于其父节点的原因，会发生一些形变，以及位移，接下来的操作就是一步步纠正
            m_willShowPage = Instantiate(m_leftPageRectTrans, m_willShowPageMask);
            //重新设置其宽度等于页的宽度，高度等于页的高度，最后将其与其父节点反向旋转相同的角度，最后纠正其位置
            m_willShowPage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_leftPageRectTrans.GetComponent<RectTransform>().rect.width);
            m_willShowPage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_leftPageRectTrans.GetComponent<RectTransform>().rect.height);
            m_willShowPage.RotateAround(new Vector2(transform.position.x, transform.position.y), -Vector3.forward, 90);
            m_willShowPage.position = m_leftPageRectTrans.position;

        }
        else if (filpDir == FilpDir.RightToLeft)
        {
            //根据左边页，生成右边页的另一面
            m_filpPageAnotherFace = Instantiate(m_leftPageRectTrans, m_filpPageRoot);
            //因为锚点布局引起的拉伸，通过Reset其宽度来恢复大小
            m_filpPageAnotherFace.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_leftPageRectTrans.GetComponent<RectTransform>().rect.height);
            //因为恢复大小之后，由于锚点造成的自动居中可以通过再次修改其位置来进行修正
            m_filpPageAnotherFace.position = m_leftPageRectTrans.position;
            //旋转到指定位置，准备翻页
            m_filpPageAnotherFace.RotateAround(new Vector2(transform.position.x, transform.position.y), -Vector3.forward, 180);

            //根据右边页生成下一页的部分
            //首先生成遮罩，遮罩的长度应该不小于书页的对角线长度（由于锚点设置会自动高度增加）
            m_willShowPageMask = Instantiate(m_rightPageRectTrans, m_filpPageRoot);
            //旋转到适当的位置
            m_willShowPageMask.RotateAround(new Vector2(transform.position.x, transform.position.y), -Vector3.forward, 90);
            //设置其透明度,alpha值为1（只为了mask的作用）
            m_willShowPageMask.GetComponent<Image>().color *= new Vector4(1, 1, 1, 1.0f / 180);
            //添加Mask
            m_willShowPageMask.gameObject.AddComponent<Mask>();

            //创建下一页，也就是将要展示的右边那一页

            //首先是克隆，但是由于其父节点的原因，会发生一些形变，以及位移，接下来的操作就是一步步纠正
            m_willShowPage = Instantiate(m_rightPageRectTrans, m_willShowPageMask);
            //重新设置其宽度等于页的宽度，高度等于页的高度，最后将其与其父节点反向旋转相同的角度，最后纠正其位置
            m_willShowPage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_rightPageRectTrans.GetComponent<RectTransform>().rect.width);
            m_willShowPage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_rightPageRectTrans.GetComponent<RectTransform>().rect.height);
            m_willShowPage.RotateAround(new Vector2(transform.position.x, transform.position.y), Vector3.forward, 90);
            m_willShowPage.position = m_rightPageRectTrans.position;
        }
        //随机颜色
        m_filpPageAnotherFace.GetComponent<Image>().color = new Color(Random.Range(0f, 1), Random.Range(0f, 1), Random.Range(0f, 1), 1);
        m_willShowPage.GetComponent<Image>().color = new Color(Random.Range(0f, 1), Random.Range(0f, 1), Random.Range(0f, 1), 1);


    }
    //Left-->Right
    IEnumerator FilpFromLeftToRight()
    {
        m_isFilping = true;
        float halfOfFilpSpeedByDegree = m_filpSpeedByDegree / 2;
        while (true)
        {
            
            m_filpPageAnotherFace.RotateAround(transform.position, -Vector3.forward, m_filpSpeedByDegree);
            Debug.Log("z值"+m_filpPageAnotherFace.eulerAngles.z);
            m_willShowPageMask.RotateAround(transform.position, -Vector3.forward, halfOfFilpSpeedByDegree);
            m_willShowPage.RotateAround(transform.position, Vector3.forward, halfOfFilpSpeedByDegree);
            if (m_filpPageAnotherFace.eulerAngles.z > 180)
                break;
            yield return null;
        }
        Debug.Log("翻页结束");
        //将现有的颜色复制，然后销毁临时产生的对象
        m_rightPageRectTrans.GetComponent<Image>().color = m_filpPageAnotherFace.GetComponent<Image>().color;
        m_leftPageRectTrans.GetComponent<Image>().color = m_willShowPage.GetComponent<Image>().color;
        Destroy(m_willShowPage.gameObject);
        Destroy(m_willShowPageMask.gameObject);
        Destroy(m_filpPageAnotherFace.gameObject);

        m_isFilping = false;
    }


    //Right-->Left
    IEnumerator FilpFromRightToLeft()
    {
        m_isFilping = true;
        float halfOfFilpSpeedByDegree = m_filpSpeedByDegree / 2;
        while (true)
        {

            m_filpPageAnotherFace.RotateAround(transform.position, Vector3.forward, m_filpSpeedByDegree);
            Debug.Log("z值" + m_filpPageAnotherFace.eulerAngles.z);
            m_willShowPageMask.RotateAround(transform.position, Vector3.forward, halfOfFilpSpeedByDegree);
            m_willShowPage.RotateAround(transform.position, -Vector3.forward, halfOfFilpSpeedByDegree);
            if (m_filpPageAnotherFace.eulerAngles.z < 180)
                break;
            yield return null;
        }
        Debug.Log("翻页结束");
        //将现有的颜色复制，然后销毁临时产生的对象
        m_leftPageRectTrans.GetComponent<Image>().color = m_filpPageAnotherFace.GetComponent<Image>().color;
        m_rightPageRectTrans.GetComponent<Image>().color = m_willShowPage.GetComponent<Image>().color;
        Destroy(m_willShowPage.gameObject);
        Destroy(m_willShowPageMask.gameObject);
        Destroy(m_filpPageAnotherFace.gameObject);


        m_isFilping = false;
    }

    

    private void Update()
    {
        if(!m_isFilping&&Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FilpBookPage(FilpDir.RightToLeft);
            
        }
        else if(!m_isFilping && Input.GetKeyDown(KeyCode.RightArrow))
        {
            FilpBookPage(FilpDir.LeftToRight);
        }
    }

}
