using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeControl : MonoBehaviour
{
    float speed; //下降速度，值越低下降越快
    float spacing;//间距，每次下降移动的值
    float mTimer;//计时
    GameManager gameManager;
    Transform mPivot;//方块旋转的中心
    bool isStopDown = false;

    public void Init(float speed,float spacing)
    {
        //初始化设置物体生成的位置和给一些变量赋值
        gameManager = GameObject.Find("BG").GetComponent<GameManager>();
        transform.SetParent(GameObject.Find("ShapePool").transform);
        if(gameObject.name.Contains("Shape (3)"))
        {
            transform.localPosition = new Vector2(-spacing / 2, 0);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
        this.speed = speed;
        this.spacing = spacing;
        mPivot = transform.Find("Pivot");
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopDown)
        {
            return;
        }
        mTimer += Time.deltaTime;
        if(mTimer >= speed)
        {
            mTimer = 0;
            //时间到了，方块向下移动一格
            SetPositionOrRotate(new Vector3(0, -spacing, 0), 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //按下左方向键，控制方块向左移动一格
            SetPositionOrRotate(new Vector3(-spacing, 0, 0), 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //按下右方向键，控制方块向右移动一格
            SetPositionOrRotate(new Vector3(spacing, 0, 0), 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //按上方向键，旋转方块
            SetPositionOrRotate(new Vector3(0, 0, 0), 90);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //按下向下方向键，加快移动速度
            speed = speed / 2;
        }     
    }

    /// <summary>
    /// 设置这个形状的位置或者形状
    /// </summary>
    /// <param name="pos">移动位置</param>
    /// <param name="rotate">旋转角度</param>
    private void SetPositionOrRotate(Vector3 pos,float rotate)
    {
        if(isStopDown)
        {
            return;
        }
        if(pos != Vector3.zero)
        {
            transform.position += pos;
            Debug.Log(transform.position);
            if (gameManager.CheckPos(transform) == false)
            {
                transform.position -= pos;
                if(pos.y != 0)
                {
                    isStopDown = true;
                    gameManager.SaveShape(transform);
                    Debug.Log("删除了脚本");
                    Destroy(this);//删除自身这个脚本组件，不再接受控制了
                }
            }
            else
            {
                //如果移动合法，根据左右移动或者向下移动播放不同的音效
                if(pos.y != 0)
                {
                    gameManager.PlayAudio(0);//播放下落的音效
                }
                else
                {
                    gameManager.PlayAudio(2);//播放移动和旋转的音效
                }
            }
            
        }
        if(rotate != 0)
        {
            transform.RotateAround(mPivot.position,Vector3.forward, rotate);
            if(gameManager.CheckPos(transform) == false)
            {
                transform.RotateAround(mPivot.position, Vector3.forward, -rotate);
            }
            else
            {
                gameManager.PlayAudio(2);//播放移动和旋转的音效
                //播放声音
            }
        }
    }
}
