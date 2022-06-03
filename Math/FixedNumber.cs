using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class FixedNumber
{
    public float rawVal = 0;

    public FixedNumber()
    {
        this.rawVal = 0;
    }

    public FixedNumber One()
    {
        this.rawVal = 1;

        return this;
    }

    public FixedNumber Zero()
    {
        this.rawVal = 0;

        return this;    
    }

    public FixedNumber SetInt(int val)
    {
        this.rawVal = val;

        return this;
    }


    public FixedNumber SetWF(int val)
    {
        rawVal = val * 1.0f / GloableLogic.VALUE_10000;

        return this;
    }

    public FixedNumber SetFloat(float val)
    {
        val = val * GloableLogic.VALUE_10000;

        var vInt = Mathf.FloorToInt(val);

        return SetWF(vInt);
    }

    public FixedNumber CopyFrom(FixedNumber copy)
    {
        rawVal = copy.rawVal;

        return this;
    }

    public static FixedNumber operator + (FixedNumber v1 , FixedNumber v2)
    {
        v1.SetFloat(v1.rawVal + v2.rawVal);

        return v1;
    }

    public static FixedNumber operator -(FixedNumber v1, FixedNumber v2)
    {
        v1.SetFloat(v1.rawVal - v2.rawVal);

        return v1;
    }

    public static FixedNumber operator *(FixedNumber v1, FixedNumber v2)
    {
        v1.SetFloat(v1.rawVal * v2.rawVal);

        return v1;
    }

    public static FixedNumber operator /(FixedNumber v1, FixedNumber v2)
    {
        v1.SetFloat(v1.rawVal / v2.rawVal);

        return v1;
    }

    public static FixedNumber GetPool()
    {
        var ret = ObjectPool<FixedNumber>.Instance.GetItem();

        return ret;
    }

    public FixedNumber sqrt()
    {
        if(this.rawVal < 0)
        {
            Debug.LogError("负数无法开方");

            return this;
        }

        var sq = Mathf.Sqrt(this.rawVal);

        this.SetFloat(sq);

        return this; ;
    }

    public void PoolRecover()
    {
        this.rawVal = 0;

        ObjectPool<FixedNumber>.Instance.ReleaseItem(this);
    }

    public FixedNumber Square()
    {
        var tmp = GetPool().CopyFrom(this);

        tmp = tmp * tmp;

        this.CopyFrom(tmp);

        tmp.PoolRecover();

        return this;
    }

    public bool IsMore(FixedNumber comp)
    {
        return this.rawVal > comp.rawVal;
    }

    public bool IsMoreAndEqual(FixedNumber comp)
    {
        return this.rawVal >= comp.rawVal;
    }

    public bool Equal(FixedNumber comp)
    {
        return this.rawVal == comp.rawVal;
    }

    public bool IsLess(FixedNumber comp)
    {
        return this.rawVal < comp.rawVal;
    }

    public bool IsLessAndEqual(FixedNumber comp)
    {
        return this.rawVal <= comp.rawVal;
    }

    public bool IsZero()
    {
        return this.rawVal == 0;
    }
}
