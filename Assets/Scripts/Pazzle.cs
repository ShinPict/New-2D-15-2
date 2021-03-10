using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pazzle: MonoBehaviour
{
    //設定と導入
    const int PanelXNum = 3;
    const int PanelYNum = 3;
    const int AllPanelNum = PanelXNum * PanelYNum;

    int[] Panel = new int[AllPanelNum];
   

    // スプライト画像格納用の配列
    Sprite[] sprites;
    GameObject[] PanelObj;

    Vector2[] BasePos;
    // Start is called before the first frame update
    void Start() 
    {
        // スプライトを配列にロードする
        sprites = Resources.LoadAll<Sprite>("image_resize");

        // 空のゲームオブジェクトを作成
        GameObject obj = new GameObject();
        PanelObj = new GameObject[sprites.Length];
        BasePos = new Vector2[sprites.Length];
        // スプライトの数だけループ
        for (int i = 0; i < sprites.Length; i++)
        {
            // 空のゲームオブジェクトを生成する、横に並べる
            PanelObj[i] = Instantiate(obj, new Vector3(2.14f * (i % 3), -2.45f * (i / 3), 0), Quaternion.identity) as GameObject;

            // 生成したゲームオブジェクトはTransformのみなのでSpriteRendererをスクリプトから追加、スプライトに配列の画像を代入
            SpriteRenderer sr = PanelObj[i].AddComponent<SpriteRenderer>();
            sr.sprite = sprites[i];
            PanelObj[i].AddComponent<BoxCollider2D>();

            PanelObj[i].name = i.ToString();
            PanelObj[i].tag = "Panel";
            BasePos[i].x = PanelObj[i].transform.position.x;
            BasePos[i].y = PanelObj[i].transform.position.y;
        }


    }
    void Change(int x, int y)　
    {
        int p1 = y * PanelXNum + x; // クリックしたパネル番号
        int p2 = -1; // 次に移動する予定のパネル番号

        // 左端に空白がある場合は左へ移動
        if (x > 0 && Panel[p1 - 1] == AllPanelNum - 1)
        {
            p2 = p1 - 1;
        }
        // 右に空白がある場合は右へ移動
        if (x < PanelXNum - 1 && Panel[p1 + 1] == AllPanelNum - 1)
        {
            p2 = p1 + 1;
        }

        // 上に空白がある場合は上へ移動
        if (y > 0 && Panel[p1 - PanelYNum] == AllPanelNum - 1)
        {
            p2 = p1 - PanelYNum;
        }
        // 下に空白がある場合は下へ移動
        if (y < PanelYNum - 1 && Panel[p1 + PanelYNum] == AllPanelNum - 1)
        {
            p2 = p1 + PanelYNum;
        }

        if (p2 != -1)
        {
            Panel[p2] = Panel[p1]; // クリックしたパネルを次の場所へ移動
            Panel[p1] = AllPanelNum - 1; // クリックしたパネルを空白にする
        }
        for (int i = 0; i < AllPanelNum; i++)
        {
            if (Panel[i] < AllPanelNum - 1)
            {
                PanelObj[Panel[i]].transform.position = BasePos[i];
                PanelObj[Panel[i]].name = i.ToString();
            }
            else
            {
                Vector2 temp = PanelObj[Panel[i]].transform.position;
                temp.y = 2.0f;
                PanelObj[Panel[i]].transform.position = temp;
                PanelObj[Panel[i]].name = i.ToString();
            }
        }
    }
    public void OnClick()
    {
        {
            for (int i = 0; i < AllPanelNum; i++)
            {
                Panel[i] = i;
            }

            for (int i = 0; i < AllPanelNum * 1000; i++)
            {
                Change(Random.Range(0, PanelXNum), Random.Range(0, PanelYNum));
            }
        }
    }
    //GameObject clickedGameObject;
   
    public void GameMain()
    {
        GameObject clickedGameObject;
        if (Input.GetMouseButtonDown(0))
        {
            clickedGameObject = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            int x = 0, y = 0;
            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
                x = int.Parse(clickedGameObject.name) % PanelXNum;
                y = int.Parse(clickedGameObject.name) / PanelYNum;
            }

            Change(x, y);
            bool clear = true;
            for (int i = 0; i < AllPanelNum; i++)
            {
                if (Panel[i] != i)
                    clear = false;
            }
            if (clear)
            {
                for (int i = 0; i < AllPanelNum; i++)
                {
                    PanelObj[i].transform.position = BasePos[i];
                }
            }
        }
    }

    void Update()
    {
        GameMain();
    }
}