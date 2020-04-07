﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Player : MonoBehaviour
{
    private NavMeshAgent agent = null;
    private Animator animator = null;
    [SerializeField] private string playerName = "";
    [SerializeField] private GameObject target = null;
    [SerializeField] private char[] instructionPL = null;
    [SerializeField] private int tabPos = 0;
    [SerializeField] private float distanceChange = 0.5f;
    [SerializeField] private TextMeshPro textName = null;
    [SerializeField] private Material[] mPL = null;
    [SerializeField] private Vector3 currentPosition = new Vector3(0,0,0);
    private Renderer m_Renderer = null;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        m_Renderer = transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        m_Renderer.material = mPL[Random.Range(0, mPL.Length)];
        textName.color = new Color32((byte)Random.Range(0, 150), (byte)Random.Range(0, 150), (byte)Random.Range(0, 150),255);
    }

    void Update()
    {
        RaycastHit hit;
        if (!Physics.Raycast(target.transform.position, target.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            if (instructionPL.Length > 0 && tabPos <= instructionPL.Length)
            {
                isDead = true;
                StartCoroutine(Saut());
                Destroy(gameObject.transform.parent.gameObject, 1f);
            }
        }
        if (!isDead)
        {
            if (distanceChange > Vector3.Distance(target.transform.position, transform.position) && !agent.isStopped)
            {
                if (instructionPL.Length > 0 && tabPos < instructionPL.Length)
                {
                    currentPosition = target.transform.position;
                    if (instructionPL[tabPos] == 'h')
                    {
                        target.transform.position = new Vector3(target.transform.position.x + 2, target.transform.position.y, target.transform.position.z);
                    }
                    if (instructionPL[tabPos] == 'b')
                    {
                        target.transform.position = new Vector3(target.transform.position.x - 2, target.transform.position.y, target.transform.position.z);
                    }
                    if (instructionPL[tabPos] == 'g')
                    {
                        target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z + 2);
                    }
                    if (instructionPL[tabPos] == 'd')
                    {
                        target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z - 2);
                    }
                    tabPos++;
                }
            }
            agent.SetDestination(target.transform.position);
        }
        animator.SetFloat("Speed", agent.desiredVelocity.magnitude);
    }

    public void SetInstruction(char[] instruction)
    {
        instructionPL = instruction;
        tabPos = 1;
    }

    public void SetPlayerName(string name)
    {
        this.playerName = name;
        textName.text = name;
    }

    IEnumerator Saut()
    {
        agent.enabled = false;
        animator.SetBool("Jump", true);
        float timer = 0.3f;
        while (true)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x,target.transform.position.x, Time.deltaTime * 0.2f), transform.position.y, Mathf.Lerp(transform.position.z, target.transform.position.z, Time.deltaTime * 0.2f));
            timer -= Time.deltaTime;
            if (timer >= 0)
            {
                transform.Translate(Vector3.up * Time.deltaTime * 0.3f, Space.World);
            }
            else
            {
                transform.Translate(Vector3.down * Time.deltaTime * 1.5f, Space.World);
            }
            yield return null;
        }
    }
}