﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBreak : MonoBehaviour
{
    public GameObject BreakEffect;
    public float Torque;
    public float Power;
    private float DamageCount;
    private float HP = 1000;
    bool Bung;
    GameObject Score;
    // 自身の子要素を管理するリスト
    List<GameObject> myParts = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // 自分の子要素をチェック
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.AddComponent<Rigidbody>();
            child.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            // 子要素リストにパーツを追加
            myParts.Add(child.gameObject);

        }
        DamageCount = 0;
        Score = GameObject.Find("Text");//スクリプトを参照
        Bung = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= DamageCount)//ダメージがHPを超えると破壊
        {
            Bung = true;

        }
        else
        {
            Bung = false;
        }

        if (Bung)
        {
            Explode();
            Instantiate(BreakEffect, transform.position, Quaternion.identity);
            Score.GetComponent<Score>().GetBuilding();//Scoreのメソッド実行
            DamageCount = 0;
        }
    }

    //キックダメージ
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "KickCollision")
        {
            DamageCount += 150;
            Shake(0.25f, 0.1f);
        }
    }




    void Explode()
    {

        // 各パーツをふっとばす
        foreach (GameObject obj in myParts)
        {
            Vector3 forcePower = new Vector3(Random.Range(0.0f, Power), Random.Range(-Power * 0.5f, Power * 0.5f), Random.Range(-Power * 0.5f, Power * 0.5f));
            Vector3 TorquePower = new Vector3(Random.Range(-Torque, Torque), Random.Range(-Torque, Torque), Random.Range(-Torque, Torque));

            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().AddForce(forcePower, ForceMode.Impulse);
            obj.GetComponent<Rigidbody>().AddTorque(TorquePower, ForceMode.Impulse);
            //5秒後に消す
            Destroy(gameObject, 5.0f);
        }
    }
    //揺らす
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = transform.localPosition;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = pos;
    }
}
