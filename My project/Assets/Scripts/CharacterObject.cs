using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private List<Vector3> destinations;
    [SerializeField] private int destinationId;
    [SerializeField] private GameObject characterRenderer;
    public Grid grid;

    private CharacterRenderer _characterRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        _characterRenderer = characterRenderer.GetComponent<CharacterRenderer>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Concrete(CharacterClass type, String side)
    {
        _characterRenderer.SetSprite(RenderManager.Instance.GetCharacterImage(type, side));
        _characterRenderer.SetController(RenderManager.Instance.GetCharacterController(type, side));
    }

    public void SetSpeed(float inputSpeed)
    {
        if (inputSpeed > 0)
        {
            SetAnimation(transform.position, destinations[destinationId]);
        }
        speed = inputSpeed;
        _characterRenderer.SetSpeed(speed);
    }

    public void SetDestinations(List<Vector3Int> dests)
    {
        destinations.Clear();
        for (int i = 0; i < dests.Count; i++)
        {
            Vector3 position = grid.CellToWorld(new Vector3Int(dests[i].x - 8,
                dests[i].y - 8, dests[i].z));
            position.y += 0.24f;
            destinations.Add(position);
        }
        destinationId = 0;
    }

    void SetAnimation(Vector3 cur, Vector3 dest)
    {
        Vector3 movement = destinations[destinationId] - transform.position;
        switch (movement.y)
        {
            case > 0.1f:
                _characterRenderer.SetAnimation(0, 1);
                break;
            case < -0.1f:
                _characterRenderer.SetAnimation(0, -1);
                break;
            default:
            {
                switch (movement.x)
                {
                    case > 0.1f:
                        _characterRenderer.SetAnimation(1, 0);
                        break;
                    case < -0.1f:
                        _characterRenderer.SetAnimation(-1, 0);
                        break;
                    default:
                        _characterRenderer.SetAnimation(0, 0);
                        break;
                }
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (destinationId < destinations.Count)
        {
            Vector3 dest = destinations[destinationId];
            if (speed > 0.1f)
            {
                if (Vector3.Distance(transform.position, dest) > 0.02f)
                    transform.position = Vector3.MoveTowards(transform.position, dest
                        , speed * Time.deltaTime);
                else
                {
                    destinationId += 1;
                    if (destinationId < destinations.Count)
                    {
                        SetAnimation(transform.position, destinations[destinationId]);
                    }
                    else
                    {
                        SetSpeed(0);
                    }
                }
            }
        }
    }
}
