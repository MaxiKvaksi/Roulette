
using UnityEngine;

public class RouletteScript : MonoBehaviour {


    public byte twistingSpeed = 2;//скорость вращения на максимальной скорости
    public float twistingTime = 2;//время вращения на максимальной скорости

    private float tempTwistingTime;//переменная для подсчёта оставшегося времени кручения на полной скорости
    private float accelerationMultiplier = 0;//коэффициент ускорения рулетки
    private float brakingMultiplier = 2;//коэффициент замедления рулетки
    private float startBrakingMultiplier;//
    private float brakingLower;//плавное замедление рулетки
    private enum States {stop = 0, acceleration, twisting, braking};//стадии вращения рулетки
    private States rouletteState = States.stop;//начальная стадия
    int indexOfElement;//текущий выдающиющий элемент
    private bool isBrakeLow;//режим докручивания рулетки
    private byte angleToStop;

    void Start()
    {
        startBrakingMultiplier = twistingSpeed;
        brakingMultiplier = twistingSpeed;
        brakingLower = brakingMultiplier / 720;
    }
    void Update()
    {
        switch (rouletteState)
        {
            case States.acceleration: Acceleration(); break;
            case States.twisting: Twisting(); break;
            case States.braking:Braking(); break;
        }
    }

    public void SpinningStart(int indexOfElement)
    {
        tempTwistingTime = twistingTime;
        brakingMultiplier = startBrakingMultiplier;
        rouletteState = States.acceleration;
        this.indexOfElement = indexOfElement;
        Debug.Log(this.indexOfElement);
    }

    void Acceleration()//разгон рулетки
    {
        if (accelerationMultiplier < twistingSpeed)
        {
            gameObject.transform.Rotate(0, 0, -Time.deltaTime - accelerationMultiplier);
            accelerationMultiplier += 0.01f;
        }
        else
        {
            rouletteState = States.twisting;
            accelerationMultiplier = 0;
        }
    }

    void Twisting()//вращение рулетки на максимальной скорости
    {
        if (tempTwistingTime > 0)
        {
            gameObject.transform.Rotate(0, 0, -(-Time.deltaTime + twistingSpeed));
            tempTwistingTime -= 0.01f;
        }
        else
        {
            rouletteState = States.braking;
        }
    }

    void Braking()//замедление рулетки
    {
        gameObject.transform.Rotate(0, 0, -(-Time.deltaTime + brakingMultiplier));
        if (angleToStop != 0)
        {
            if (brakingMultiplier > 0.05f)
            {
                brakingMultiplier -= brakingLower;
                angleToStop = 1;
                Debug.Log("is");
            }
            if(isCurrentAngle())angleToStop--;
            if (angleToStop == 0)
            {
                rouletteState = States.stop;
               // GameManager.Instance.Result(GetAngleRes(gameObject.transform.eulerAngles.z));
                angleToStop = (byte)(startBrakingMultiplier * 20);
                return;
            }
            return;
        }
        if (isCurrentAngle())
        {
            if (angleToStop == 0)
            {
                angleToStop = (byte)(startBrakingMultiplier * 20);
            }
        }
    }

    public void SkipIt()
    {
        gameObject.transform.eulerAngles = new Vector3(0, 0, GetAngleRes(indexOfElement));
        rouletteState = States.stop;
        GameManager.Instance.Result(GetAngleRes(gameObject.transform.eulerAngles.z));
        angleToStop = (byte)(startBrakingMultiplier * 20);
    }

    private int GetAngleRes(float angle)//получение текущего элемента, на который указывает стрелка
    {
        if (angle >= 0)
        {
            return ((int)angle / 45);
        }
        else
        {
            return 8 - ((int)-angle / 45 + 4);
        }
    }
    private float GetAngleRes(int element)
    {
        return element * 45 + Random.Range(5.0f, 35.0f);
    }

    private bool isCurrentAngle()
    {
        return GetAngleRes(gameObject.transform.eulerAngles.z + 20) == indexOfElement;
    }
}
