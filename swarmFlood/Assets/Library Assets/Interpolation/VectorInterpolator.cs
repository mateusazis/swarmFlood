using UnityEngine;

class VectorInterpolator : Interpolator
{
    private Vector3 start, end;
    private VectorInterpolationStep stepFunction = null;

    public void SetInitialData(Vector3 start, Vector3 end, VectorInterpolationStep stepFunction)
    {
        this.start = start;
        this.end = end;
        this.stepFunction = stepFunction;
    }

    public override void Update()
    {
        base.Update();
        float pctg = Percentage;
        Vector3 result = Vector3.Lerp(start, end, pctg);
        stepFunction(result, pctg);
    }
}