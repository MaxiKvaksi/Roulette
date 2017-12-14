using UnityEngine;

public class VariantsScript: MonoBehaviour {

    public GameObject[] variantsList;
    public byte elementCount;

    private byte[] probability = new byte[1000];

    void Awake()
    {
        PreparationOfProbabilityArray();
    }

    void PreparationOfProbabilityArray()
    {
        float sumOfProbabilities = 0;
        float[] normalizedProbabilities = new float[variantsList.Length];
        foreach (GameObject element in variantsList)
        {
            sumOfProbabilities += element.GetComponent<Element>().chance; //суммирование вероятнностей элементов
        }
        for (int i = 0; i < variantsList.Length; i++)
        {
            normalizedProbabilities[i] = 
                variantsList[i].GetComponent<Element>().chance/sumOfProbabilities;//нормализация вероятностей
        }
       
        int currentIndexOfProbabilityArray = 0;
        for (byte i = 0; i < variantsList.Length; i++)//создание массива вероятностей появления элемента
        {
            int temp = 0;
            while ((int)(normalizedProbabilities[i] * probability.Length) > temp)
            {
                probability[currentIndexOfProbabilityArray] = i;
                temp++;
                currentIndexOfProbabilityArray++;
            }
        }

        for (int i = probability.Length - 1; i >= 1; i--)//перемешивание массива вероятностей
        {
            int j = Random.Range(0, i + 1);
            var temp = probability[j];
            probability[j] = probability[i];
            probability[i] = temp;
        }
    }//подготовка массива вероятностей

    public int GetMagicValue()//получение рандомного значения из массива вероятностей
    {
        return probability[Random.Range(0, probability.Length)];
    }
}
