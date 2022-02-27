using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHandler : MonoBehaviour
{
    public GameObject prefab;
    public int populationSize = 1;

    [Range(0.1f, 10f)] public float Gamespeed = 1f;
    public int[] layers = new int[3] { 5, 3, 2 };

    public List<NeuralNetwork> networks;
    public List<Monster> Monsters;

    void Start()
    {
        
        InitNetworks();
        CreateMonsters();
    }

    public void InitNetworks()
    {
        networks = new List<NeuralNetwork>();
        for (int instance = 0; instance < populationSize; instance++)
        {
            NeuralNetwork network = new NeuralNetwork(layers);
            network.Load("Assets/Scripts/Save.txt");
            networks.Add(network);
        }
    }

    public void CreateMonsters()
    {
        Monsters = new List<Monster>();
        for (int i = 0; i < populationSize; i++)
        {
            Monster monster = (Instantiate(prefab, new Vector3(2f, 0.2f, 0), new Quaternion(0,0,0,0))).GetComponent<Monster>();
            monster.network = networks[i];//deploys network to each learner
            Monsters.Add(monster);

        }
    }

}