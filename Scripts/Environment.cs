using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {
    [SerializeField]
    private GameObject road;
    [SerializeField]
    private GameObject[] vehicles;
    [SerializeField]
    private GameObject[] trees;
    private float _roadWidth;
    private const int MAX_SECTIONS_COUNT = 5;
    private const float ENVIRONMENT_SPEED = 20.0f;
    private Queue<GameObject> _sections = new Queue<GameObject>();

    private void Start() {
        _roadWidth = road.GetComponent<Renderer>().bounds.size.z;

        while (_sections.Count < MAX_SECTIONS_COUNT) {
            SpawnSection();
            SpawnEnvironmentDetails();
        }
        SpawnVehicles();

        InvokeRepeating("DestroyInvisibleSection", 1f, 1f);
    }

    private void Update() {
        foreach (Transform child in transform) {
            child.position += -Vector3.forward * Time.deltaTime * ENVIRONMENT_SPEED;
        }
    }

    private void SpawnSection() {
        Vector3 offset;

        if (_sections.Count == 0) {
            offset = road.transform.position;
        }
        else {
            offset = _sections.Last().transform.position + new Vector3(0, 0, _roadWidth);
        }

        GameObject section = Instantiate(road, offset, road.transform.rotation);

        section.transform.SetParent(gameObject.transform);
        _sections.Enqueue(section);
    }

    private void SpawnVehicles() {
        GameObject lastSection = _sections.Last();

        // Spawn vehicles that move forward
        foreach (int i in Enumerable.Range(1, Random.Range(1, 4))) {
            GameObject randomVehicle = vehicles[Random.Range(0, vehicles.Length)];

            Instantiate(
                randomVehicle,
                randomVehicle.transform.position + lastSection.transform.position + new Vector3(3.5f * (i - 1) + 1.8f, 0, 0),
                Quaternion.Euler(0f, -90.0f, 0f)
            ).transform.SetParent(lastSection.transform);
        }
    }

    private void SpawnEnvironmentDetails() {
        GameObject lastSection = _sections.Last();


    }

    private void DestroyInvisibleSection() {
        if (_sections.Peek().transform.position.z < -_roadWidth * 1.5f) {
            Destroy(_sections.Dequeue());
            SpawnSection();
            SpawnVehicles();
            SpawnEnvironmentDetails();
        }
    }
}
