using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public static partial class MyMath
{
    public static Unity.Mathematics.Random CreateRandom()
    {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random();
        random.InitState((uint)System.DateTime.Now.Ticks);
        return random;
    }
    /// <summary>
    /// [0,1)
    /// </summary>    
    public static float RandomFloat()
    {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random();
        random.InitState((uint)System.DateTime.Now.Ticks);
        return random.NextFloat();
    }

    /// <summary>
    /// [min,max)
    /// </summary>    
    public static float RandomFloat(float min, float max)
    {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random();
        random.InitState((uint)System.DateTime.Now.Ticks);
        return random.NextFloat(min, max);
    }

    /// <summary>
    /// [min,max)
    /// </summary>    
    public static int RandomInt(int min, int max)
    {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random();
        random.InitState((uint)System.DateTime.Now.Ticks);
        return random.NextInt(min, max);
    }

    /// <summary>
    /// [0,max)
    /// </summary>    
    public static int RandomInt(int max)
    {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random();
        random.InitState((uint)System.DateTime.Now.Ticks);
        return random.NextInt(max);
    }

    public static quaternion RotTo(quaternion from, quaternion to, float max_degree)
    {
        max_degree = math.abs(max_degree);
        if (max_degree < math.EPSILON)
            return from;

        //1. 计算 from 和 to之间的角度差
        float delta_degree = AngleDegree(from, to);
        delta_degree = math.abs(delta_degree);

        //2. lerp
        if (delta_degree < max_degree)
            return to;
        return math.slerp(from, to, max_degree / delta_degree);
    }

    //Y轴的degree
    public static float AngleDegreeY(quaternion from, quaternion to)
    {
        quaternion delta_rot = math.mul(math.inverse(from), to);
        float3 dir_delta = math.mul(delta_rot, math.forward());
        if (math.abs(dir_delta.z) < float.Epsilon)
            return 0;

        float angle = math.degrees(math.atan2(dir_delta.x, dir_delta.z));
        return angle;
    }

    //Y轴的degree
    /*
    public static float AngleDegreeY(quaternion from, quaternion to)
    {
        float3 dir_from = math.mul(from, math.forward());
        float3 dir_to = math.mul(to, math.forward());
        dir_from.y = 0;
        dir_to.y = 0;

        float len_from = math.length(dir_from);
        float len_to = math.length(dir_to);
        if (len_from < math.EPSILON || len_to < math.EPSILON)
            return 0;

        dir_from = dir_from / len_from;
        dir_to = dir_to / len_to;

        float dot = math.clamp(math.dot(dir_from, dir_to), -1F, 1F);
        float delta_degree = math.degrees(math.acos(dot));
        return delta_degree;
    }
    */

    // 返回 Degree
    public static float AngleDegree(quaternion from, quaternion to)
    {
        float3 dir_from = math.mul(from, math.forward());
        float3 dir_to = math.mul(to, math.forward());
        float denominator = math.sqrt(math.lengthsq(dir_from) * math.lengthsq(dir_to));
        if (denominator < 1e-15F)
            return 0;
        float dot = math.clamp(math.dot(dir_from, dir_to) / denominator, -1F, 1F);
        float delta_degree = math.degrees(math.acos(dot));
        return delta_degree;
    }

    public static float MoveTo(float from, float to, float max_value)
    {
        float dt = to - from;

        //相同
        if (math.abs(dt) < float.Epsilon)
            return to;

        if (dt > 0)
            return from + math.min(dt, math.abs(max_value));
        else
            return from - math.min(-dt, math.abs(max_value));
    }

    public static float3 MoveTo(float3 from, float3 to, float max_dist)
    {
        float total_dist = math.distance(to, from);
        if (total_dist < math.EPSILON || total_dist < max_dist)
            return to;
        float p = math.clamp(max_dist / total_dist, 0, 1);
        return math.lerp(from, to, p);
    }


    public static float3 ToEuler(quaternion q, math.RotationOrder order = math.RotationOrder.Default)
    {
        const float epsilon = 1e-6f;

        //prepare the data
        var qv = q.value;
        var d1 = qv * qv.wwww * new float4(2.0f); //xw, yw, zw, ww
        var d2 = qv * qv.yzxw * new float4(2.0f); //xy, yz, zx, ww
        var d3 = qv * qv;
        var euler = new float3(0.0f);

        const float CUTOFF = (1.0f - 2.0f * epsilon) * (1.0f - 2.0f * epsilon);

        switch (order)
        {
            case math.RotationOrder.ZYX:
                {
                    var y1 = d2.z + d1.y;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = -d2.x + d1.z;
                        var x2 = d3.x + d3.w - d3.y - d3.z;
                        var z1 = -d2.y + d1.x;
                        var z2 = d3.z + d3.w - d3.y - d3.x;
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), math.atan2(z1, z2));
                    }
                    else //zxz
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.z, d1.y, d2.y, d1.x);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), 0.0f);
                    }

                    break;
                }

            case math.RotationOrder.ZXY:
                {
                    var y1 = d2.y - d1.x;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = d2.x + d1.z;
                        var x2 = d3.y + d3.w - d3.x - d3.z;
                        var z1 = d2.z + d1.y;
                        var z2 = d3.z + d3.w - d3.x - d3.y;
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), math.atan2(z1, z2));
                    }
                    else //zxz
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.z, d1.y, d2.y, d1.x);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), 0.0f);
                    }

                    break;
                }

            case math.RotationOrder.YXZ:
                {
                    var y1 = d2.y + d1.x;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = -d2.z + d1.y;
                        var x2 = d3.z + d3.w - d3.x - d3.y;
                        var z1 = -d2.x + d1.z;
                        var z2 = d3.y + d3.w - d3.z - d3.x;
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), math.atan2(z1, z2));
                    }
                    else //yzy
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.x, d1.z, d2.y, d1.x);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), 0.0f);
                    }

                    break;
                }

            case math.RotationOrder.YZX:
                {
                    var y1 = d2.x - d1.z;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = d2.z + d1.y;
                        var x2 = d3.x + d3.w - d3.z - d3.y;
                        var z1 = d2.y + d1.x;
                        var z2 = d3.y + d3.w - d3.x - d3.z;
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), math.atan2(z1, z2));
                    }
                    else //yxy
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.x, d1.z, d2.y, d1.x);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), 0.0f);
                    }

                    break;
                }

            case math.RotationOrder.XZY:
                {
                    var y1 = d2.x + d1.z;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = -d2.y + d1.x;
                        var x2 = d3.y + d3.w - d3.z - d3.x;
                        var z1 = -d2.z + d1.y;
                        var z2 = d3.x + d3.w - d3.y - d3.z;
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), math.atan2(z1, z2));
                    }
                    else //xyx
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.x, d1.z, d2.z, d1.y);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), math.asin(y1), 0.0f);
                    }

                    break;
                }

            case math.RotationOrder.XYZ:
                {
                    var y1 = d2.z - d1.y;
                    if (y1 * y1 < CUTOFF)
                    {
                        var x1 = d2.y + d1.x;
                        var x2 = d3.z + d3.w - d3.y - d3.x;
                        var z1 = d2.x + d1.z;
                        var z2 = d3.x + d3.w - d3.y - d3.z;
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), math.atan2(z1, z2));
                    }
                    else   //xzx
                    {
                        y1 = math.clamp(y1, -1.0f, 1.0f);
                        var abcd = new float4(d2.z, d1.y, d2.x, d1.z);
                        var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                        var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                        euler = new float3(math.atan2(x1, x2), -math.asin(y1), 0.0f);
                    }

                    break;
                }
        }

        return _eulerReorderBack(euler, order);
    }

    private static float3 _eulerReorderBack(float3 euler, math.RotationOrder order)
    {
        switch (order)
        {
            case math.RotationOrder.XZY:
                return euler.xzy;
            case math.RotationOrder.YZX:
                return euler.zxy;
            case math.RotationOrder.YXZ:
                return euler.yxz;
            case math.RotationOrder.ZXY:
                return euler.yzx;
            case math.RotationOrder.ZYX:
                return euler.zyx;
            case math.RotationOrder.XYZ:
            default:
                return euler;
        }
    }

    public static void Shuffle<T>(DynamicBuffer<T> list) where T : unmanaged
    {
        int count = list.Length;
        for (var i = 0; i < count; i++)
        {
            var t = list[i];
            var r = RandomInt(i, count);
            list[i] = list[r];
            list[r] = t;
        }
    }
}

