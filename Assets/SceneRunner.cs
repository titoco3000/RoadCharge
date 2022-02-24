using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRunner : MonoBehaviour
{
    public int roadCount = 20;

    public float Velocity;

    public float obstacleChance = 0.3f;
    public float endDesaccelRate = 1;
    public float resetDelay = 2f;

    public GameObject RoadPrefab;
    public GameObject[] obstPrefabs;
    public GameObject[] scenarioPrefabs;
    public GameObject[] grassPrefabs;

    public bool carroBateu = false;
    public Transform Carro;

    private List<Transform> RoadBlocks;

    public bool ObstaculosHabilitados = false;
    public bool Parado = false;

    public UnityEngine.UI.Text Text;
    float distanciaAndada;

    private int lastObstacle;

    void Start()
    {
        ClearScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (carroBateu)
        {
            Carro.position += new Vector3(Velocity * Time.deltaTime, 0, 0);
        }
        for (int i = 0; i < roadCount; i++)
        {
            Vector3 pos = RoadBlocks[i].position;
            pos += new Vector3(Velocity, 0, 0) * Time.deltaTime;
            if(pos.x > transform.position.x + roadCount)
            {
                pos = transform.position;
                NewStep(RoadBlocks[i]);
                SetBackground(RoadBlocks[i]);
            }
            RoadBlocks[i].position = pos;
            
        }
        if (Parado)
        {
            Velocity = Mathf.Lerp(Velocity, 0, Time.deltaTime * endDesaccelRate);
        }
        if(ObstaculosHabilitados)
        {
            distanciaAndada += Velocity * Time.deltaTime * 5;
            Text.text = distanciaAndada.ToString("F2") + "m";
        }
    }
    void NewStep(Transform bloco)
    {
        for (int i = bloco.childCount-1; i >= 2; i--)
        {
            Destroy(bloco.GetChild(i).gameObject);
        }


        if (ObstaculosHabilitados)
        {
            //chance de colocar algum obstaculo
            if(Random.Range(0f,1f) <= obstacleChance)
            {
                //1 ou -1
                int p = (Random.Range(0, 2) == 0 ? 1 : -1);

                int obstID = Random.Range(0, obstPrefabs.Length);
                //esse bloco aqui é para reduzir o numero de obstaculos repitidos; se fosse para impedir de vez,
                //bastaria trocar por um while loop
                if(obstID == lastObstacle)
                    obstID = Random.Range(0, obstPrefabs.Length);

                Transform novoObstaculo = Instantiate(obstPrefabs[obstID], bloco).transform;
                novoObstaculo.localScale = new Vector3(novoObstaculo.localScale.x , novoObstaculo.localScale.y, novoObstaculo.localScale.z * p);
                novoObstaculo.localPosition = Vector3.zero;
                
            }
        }
    }

    private void SetBackground(Transform bloco)
    {
        Transform novoCenario = Instantiate(scenarioPrefabs[Random.Range(0, scenarioPrefabs.Length)], bloco).transform;
        novoCenario.position = bloco.position + new Vector3(0,0,-1);
    }

    public void Parar()
    {
        ObstaculosHabilitados = false;
        carroBateu = true;
        Parado = true;
        GetComponent<GameManager>().Invoke("ResetGame", resetDelay);

    }

    void TrySetHighscore(float score)
    {
        float currentScore = PlayerPrefs.GetFloat("HighScore", 0);
        if (score > currentScore)
        {
            currentScore = score;
        }
        PlayerPrefs.SetFloat("HighScore", currentScore);
    }


    public void ClearScene()
    {
        TrySetHighscore(distanciaAndada);
        Text.text = "Highscore: " + PlayerPrefs.GetFloat("HighScore", 0).ToString("F2") + "m";


        carroBateu = false;
        distanciaAndada = 0;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        RoadBlocks = new List<Transform>();
        for (int i = 0; i < roadCount; i++)
        {
            //1 ou -1
            int p = (Random.Range(0, 2) == 0 ? 1 : -1);

            Transform newRoad = Instantiate(RoadPrefab, transform).transform;

            newRoad.localScale = new Vector3(newRoad.localScale.x, newRoad.localScale.y, newRoad.localScale.z * p);
            newRoad.position = new Vector3(transform.position.x + i, transform.position.y, transform.position.z);

            Transform novoMato = Instantiate(grassPrefabs[Random.Range(0, grassPrefabs.Length)], newRoad).transform;
            novoMato.position = newRoad.position + new Vector3(0, 0, 0.5f);

            SetBackground(newRoad);
            RoadBlocks.Add(newRoad);
        }
    }
}
