using UnityEngine;

class QuaternionInterpolator : Interpolator
{
    private Quaternion start, end;
    private QuaternionInterpolationStep stepFunction = null;

    public void SetInitialData(Quaternion start, Quaternion end, QuaternionInterpolationStep stepFunction)
    {
        this.start = start;
        this.end = end;
        this.stepFunction = stepFunction;
    }

    public override void Update()
    {
        base.Update();
        float pctg = Percentage;
        Quaternion result = Quaternion.Lerp(start, end, pctg);
        stepFunction(result, pctg);
    }
}