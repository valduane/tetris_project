﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Blocks : MonoBehaviour
{

    float falling;
    public AudioClip BreakSound;

    void Start()
    {
        if (!isDontCollide())
        {
            int SceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(SceneIndex);
        }
    }

    
    void Update()
    {
        Movement();
    }

    bool isDontCollide()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Boundaries.round(child.position);
            if (!Boundaries.inside(v))
                return false;
            if (Boundaries.grid[(int)v.x, (int)v.y] != null && Boundaries.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    void updateGrid()
    {
        for (int y = 0; y < Boundaries.h; ++y)
            for (int x = 0; x < Boundaries.w; ++x)
                if (Boundaries.grid[x, y] != null)
                    if (Boundaries.grid[x, y].parent == transform)
                        Boundaries.grid[x, y] = null;
        foreach (Transform child in transform)
        {
            Vector2 v = Boundaries.round(child.position);
            Boundaries.grid[(int)v.x, (int)v.y] = child;
        }
    }

    void Movement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (isDontCollide())
                updateGrid();
            else
                transform.position += new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            if (isDontCollide())
                updateGrid();
            else
                transform.position += new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);
            if (isDontCollide())
                updateGrid();
            else
                transform.Rotate(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - falling >= 1)
        {
            transform.position += new Vector3(0, -1, 0);
            if (isDontCollide())
            {
                updateGrid();
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                Boundaries.deleteAll();
                AudioSource.PlayClipAtPoint(BreakSound, Camera.main.transform.position);
                FindObjectOfType<Spawn>().Next();
                enabled = false;
            }
            falling = Time.time;
        }
    }
}

