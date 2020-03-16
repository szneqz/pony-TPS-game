using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Mimics : NetworkBehaviour {

    private Player2 playerScript;
    private SkinnedMeshRenderer character;
    private float time = 0.0f;
    private float value = 0.0f;
    private bool changing = false;
    private int sign = 1;
    [SyncVar(hook = "OnChangeExp")]
    private int Expression = -1;
    [SyncVar(hook = "OnChangeExp2")]
    private int Expression2 = -1;

    void Start()
    {
        playerScript = GetComponent<Player2>();
        character = transform.Find("Character_Wings").GetComponent<SkinnedMeshRenderer>();
    }

    void OnChangeExp(int exp)
    {
        character.SetBlendShapeWeight(3, 0);
        character.SetBlendShapeWeight(6, 0);
        character.SetBlendShapeWeight(9, 0);
        character.SetBlendShapeWeight(12, 0);
        character.SetBlendShapeWeight(15, 0);
        Expression = exp;
        if (Expression > -1)
            character.SetBlendShapeWeight(Expression, 100);
    }

    void OnChangeExp2(int exp)
    {
        character.SetBlendShapeWeight(21, 0);
        character.SetBlendShapeWeight(27, 0);
        character.SetBlendShapeWeight(30, 0);
        Expression2 = exp;
        if (Expression2 > -1)
            character.SetBlendShapeWeight(Expression2, 100);
    }

    [Command]
    void CmdUpdateExpressions(int a, int b)
    {
        Expression = a;
        Expression2 = b;
    }

    void Update()
    {
        if (!playerScript.Dead)
        {
            if (!changing)
            {
                if (time > 0.0f)
                {
                    time -= Time.deltaTime;
                }
                else
                {
                    time = Random.value * 3.0f + 5.0f;
                    changing = true;
                }
            }
            else
            {
                value += sign * Time.deltaTime * 1000.0f;
                character.SetBlendShapeWeight(18, value);
                if (value > 100.0f)
                    sign = -1;
                if (value < 0.0f)
                {
                    value = 0.0f;
                    sign = 1;
                    changing = false;
                }
            }
            //*********************************************************
            if (isLocalPlayer)
            {
                if (Input.GetKey(KeyCode.Keypad0))
                {
                    Expression = -1;
                    Expression2 = -1;
                }
                if (Input.GetKey(KeyCode.Keypad1))
                {
                    Expression = 3;
                    Expression2 = -1;
                }
                if (Input.GetKey(KeyCode.Keypad2))
                {
                    Expression = 6;
                    Expression2 = -1;
                }
                if (Input.GetKey(KeyCode.Keypad3))
                {
                    Expression = 9;
                    Expression2 = 21;
                }
                if (Input.GetKey(KeyCode.Keypad4))
                {
                    Expression = 12;
                    Expression2 = 30;
                }
                if (Input.GetKey(KeyCode.Keypad5))
                {
                    Expression = 15;
                    Expression2 = 27;
                }
                CmdUpdateExpressions(Expression, Expression2);
            }
        }
    }
}
