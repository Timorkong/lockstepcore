using Pool;
using UnityEngine;


public class FixedVec3
{
    public Vector3 vector3;

    public FixedNumber x;

    public FixedNumber y;

    public FixedNumber z;

    public static FixedVec3 GetPool()
    {
        var ret = ObjectPool<FixedVec3>.Instance.GetItem();

        ret.x = FixedNumber.GetPool();

        ret.y = FixedNumber.GetPool();

        ret.z = FixedNumber.GetPool();

        return ret;
    }

    public void PoolRecover()
    {
        x.PoolRecover();

        y.PoolRecover();

        z.PoolRecover();

        x = null;

        y = null;

        z = null;

        ObjectPool<FixedVec3>.Instance.ReleaseItem(this);
    }

    public FixedVec3 SetInt(int x , int y , int z)
    {
        this.x.SetInt(x);

        this.y.SetInt(y);

        this.z.SetInt(z);

        return this;
    }

    public FixedVec3 SetFixed(FixedNumber x, FixedNumber y, FixedNumber z)
    {
        this.x.CopyFrom(x);

        this.y.CopyFrom(y);

        this.z.CopyFrom(z);

        return this;
    }

    public FixedVec3 SetFloat(float x, float y, float z)
    {
        this.x.SetFloat(x);

        this.y.SetFloat(y);

        this.z.SetFloat(z);

        return this;
    }

    public FixedVec3 SetWf(int x , int y , int z)
    {
        this.x.SetWF(x);

        this.y.SetWF(y);

        this.z.SetWF(z);

        return this;
    }

    public static FixedVec3 operator +(FixedVec3 v1 , FixedVec3 v2)
    {
        v1.x = v1.x + v2.x;

        v1.y = v1.y + v2.y;

        v1.z = v1.z + v2.z;

        return v1;
    }

    public static FixedVec3 operator -(FixedVec3 v1, FixedVec3 v2)
    {
        v1.x = v1.x - v2.x;

        v1.y = v1.y - v2.y;

        v1.z = v1.z - v2.z;

        return v1;
    }

    public static FixedVec3 operator *(FixedVec3 v1, FixedNumber v2)
    {
        v1.x = v1.x * v2;

        v1.y = v1.y * v2;

        v1.z = v1.z * v2;

        return v1;
    }

    public static FixedVec3 operator /(FixedVec3 v1, FixedNumber v2)
    {
        v1.x = v1.x / v2;

        v1.y = v1.y / v2;

        v1.z = v1.z / v2;

        return v1;
    }

    public FixedVec3 CopyFrom(FixedVec3 copy)
    {
        this.x.CopyFrom(copy.x);

        this.y.CopyFrom(copy.y);

        this.z.CopyFrom(copy.z);

        return this;
    }

    public FixedVec3 Dup()
    {
        var ret = GetPool();

        ret.CopyFrom(this);

        return ret;
    }

    public FixedNumber Squart()
    {
        var _x = FixedNumber.GetPool().Square();

        var _y = FixedNumber.GetPool().Square();

        var _z = FixedNumber.GetPool().Square();

        _x = _x + _y + _z;

        _y.PoolRecover();

        _z.PoolRecover();

        return _x;
    }

    public FixedNumber Sqrt()
    {
        var _square = this.Squart();

        return _square.sqrt();
    }

    public FixedVec3 NormalizeSelf()
    {
        var _square = this.Squart();

        if (_square.IsZero() == false)
        {
            var _sqrt = this.Sqrt();

            var _one = FixedNumber.GetPool().One();

            var len = _one / _sqrt;

            x = x * len;

            y = y * len;

            z = z * len;

            _one.PoolRecover();
        }

        _square.PoolRecover();

        return this;
    }

    public Vector3 EncodeVec3()
    {
        return new Vector3(this.x.rawVal , this.y.rawVal , this.z.rawVal);
    }

    public override string ToString()
    {
        string ret;

        ret = string.Format("x = {0} y = {1} z = {2}", x.rawVal, y.rawVal, z.rawVal);

        return ret;
    }
}
